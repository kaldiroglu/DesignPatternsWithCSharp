namespace DevKaldiroglu.DP.Structural.Facade.Solution;

public sealed record CartLine(string Sku, int Quantity);
public sealed record Cart(string CustomerId, IReadOnlyList<CartLine> Lines);

public class InventoryService
{
    public string Reserve(Cart cart)
    {
        foreach (var l in cart.Lines)
            if (l.Sku == "OOS") throw new Exception("OutOfStock:" + l.Sku);
        return "RES-" + cart.CustomerId;
    }
    public void Release(string id) => Console.WriteLine("released " + id);
}

public class PricingService
{
    public decimal Price(Cart cart) => cart.Lines.Count * 25m;
}

public class TaxService
{
    public decimal Tax(decimal subtotal, string addressZip) => subtotal * 0.08m;
}

public class PaymentGateway
{
    public string Charge(string customerId, decimal amount, string paymentMethod)
    {
        if (paymentMethod == "declined") throw new Exception("payment declined");
        return "PAY-" + customerId;
    }
    public void Refund(string id) => Console.WriteLine("refunded " + id);
}

public class ShippingService
{
    public string CreateShipment(string orderId, string addressZip)
    {
        if (addressZip == "99999") throw new Exception("undeliverable");
        return "SHIP-" + orderId;
    }
}

public class OrderRepository
{
    public void Save(string orderId, Cart cart, decimal total) =>
        Console.WriteLine($"saved {orderId} total=${total}");
}

public class NotificationService
{
    public void SendConfirmation(string customerId, string orderId) =>
        Console.WriteLine($"notified {customerId} for {orderId}");
}

public abstract record CheckoutResult
{
    private CheckoutResult() { }
    public sealed record Placed(string OrderId) : CheckoutResult;
    public sealed record Failed(FailureReason Reason, string Detail) : CheckoutResult;
}

public enum FailureReason { OutOfStock, PaymentDeclined, Undeliverable, Unknown }

public class CheckoutFacade
{
    private readonly InventoryService _inventory;
    private readonly PricingService _pricing;
    private readonly TaxService _tax;
    private readonly PaymentGateway _payment;
    private readonly ShippingService _shipping;
    private readonly OrderRepository _orders;
    private readonly NotificationService _notify;

    public CheckoutFacade(InventoryService inventory, PricingService pricing, TaxService tax,
                          PaymentGateway payment, ShippingService shipping, OrderRepository orders,
                          NotificationService notify)
    {
        _inventory = inventory; _pricing = pricing; _tax = tax;
        _payment = payment; _shipping = shipping;
        _orders = orders; _notify = notify;
    }

    public CheckoutResult PlaceOrder(Cart cart, string paymentMethod, string addressZip)
    {
        string reservation;
        try { reservation = _inventory.Reserve(cart); }
        catch (Exception e) { return new CheckoutResult.Failed(FailureReason.OutOfStock, e.Message); }

        var subtotal = _pricing.Price(cart);
        var total = subtotal + _tax.Tax(subtotal, addressZip);

        string paymentId;
        try { paymentId = _payment.Charge(cart.CustomerId, total, paymentMethod); }
        catch (Exception e)
        {
            _inventory.Release(reservation);
            return new CheckoutResult.Failed(FailureReason.PaymentDeclined, e.Message);
        }

        var orderId = "ORD-" + Guid.NewGuid();
        try { _shipping.CreateShipment(orderId, addressZip); }
        catch (Exception e)
        {
            _payment.Refund(paymentId);
            _inventory.Release(reservation);
            return new CheckoutResult.Failed(FailureReason.Undeliverable, e.Message);
        }

        _orders.Save(orderId, cart, total);
        _notify.SendConfirmation(cart.CustomerId, orderId);
        return new CheckoutResult.Placed(orderId);
    }
}

public class CheckoutController
{
    private readonly CheckoutFacade _facade;
    public CheckoutController(CheckoutFacade facade) => _facade = facade;
    public CheckoutResult PlaceOrder(Cart cart, string paymentMethod, string addressZip) =>
        _facade.PlaceOrder(cart, paymentMethod, addressZip);
}

public static class SolutionDemo
{
    public static void Run()
    {
        var facade = new CheckoutFacade(
            new InventoryService(), new PricingService(), new TaxService(),
            new PaymentGateway(), new ShippingService(),
            new OrderRepository(), new NotificationService());

        var controller = new CheckoutController(facade);
        var cart = new Cart("cust-1", new[] { new CartLine("SKU-A", 2) });

        Print(controller.PlaceOrder(cart, "card-good", "94107"));
        Print(controller.PlaceOrder(cart, "declined",  "94107"));
        Print(controller.PlaceOrder(cart, "card-good", "99999"));
    }

    private static void Print(CheckoutResult r) =>
        Console.WriteLine(r switch
        {
            CheckoutResult.Placed p => "OK:" + p.OrderId,
            CheckoutResult.Failed f => $"FAILED:{f.Reason} ({f.Detail})",
            _ => "?"
        });
}
