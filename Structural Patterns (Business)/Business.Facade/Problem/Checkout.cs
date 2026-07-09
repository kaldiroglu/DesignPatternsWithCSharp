namespace DevKaldiroglu.DP.Structural.Facade.Problem;

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

public class CheckoutController
{
    private readonly InventoryService _inventory = new();
    private readonly PricingService _pricing = new();
    private readonly TaxService _tax = new();
    private readonly PaymentGateway _payment = new();
    private readonly ShippingService _shipping = new();
    private readonly OrderRepository _orders = new();
    private readonly NotificationService _notify = new();

    public string PlaceOrder(Cart cart, string paymentMethod, string addressZip)
    {
        string reservation;
        try { reservation = _inventory.Reserve(cart); }
        catch (Exception e) { return "FAILED:OUT_OF_STOCK:" + e.Message; }

        var subtotal = _pricing.Price(cart);
        var total = subtotal + _tax.Tax(subtotal, addressZip);

        string paymentId;
        try { paymentId = _payment.Charge(cart.CustomerId, total, paymentMethod); }
        catch (Exception e) { _inventory.Release(reservation); return "FAILED:PAYMENT:" + e.Message; }

        var orderId = "ORD-" + Guid.NewGuid();
        try { _shipping.CreateShipment(orderId, addressZip); }
        catch (Exception e)
        {
            _payment.Refund(paymentId);
            _inventory.Release(reservation);
            return "FAILED:SHIPPING:" + e.Message;
        }

        _orders.Save(orderId, cart, total);
        _notify.SendConfirmation(cart.CustomerId, orderId);
        return "OK:" + orderId;
    }
}

public static class ProblemDemo
{
    public static void Run()
    {
        var c = new CheckoutController();
        var cart = new Cart("cust-1", new[] { new CartLine("SKU-A", 2) });
        Console.WriteLine(c.PlaceOrder(cart, "card-good", "94107"));
        Console.WriteLine(c.PlaceOrder(cart, "declined",  "94107"));
        Console.WriteLine(c.PlaceOrder(cart, "card-good", "99999"));
    }
}
