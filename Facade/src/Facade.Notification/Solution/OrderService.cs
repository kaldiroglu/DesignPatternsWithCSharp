namespace dev.kaldiroglu.Facade.Notification.Solution;

/// <summary>
/// AFTER the Facade pattern. The client depends on ONE class
/// (<see cref="NotificationFacade"/>), calls ONE method, and gets a structured
/// result back. Adding a fifth channel (WhatsApp, Teams, ...) changes zero lines
/// here — the change lives inside the facade.
/// </summary>
public sealed class OrderService
{
    private readonly NotificationFacade _notifier;

    public OrderService(NotificationFacade notifier)
    {
        _notifier = notifier;
    }

    public NotificationResult ConfirmOrder(User user, string orderId)
    {
        // -- business logic --
        Console.WriteLine($"Order {orderId} confirmed.");

        string title = $"Order #{orderId} Confirmed";
        string body = $"Your order #{orderId} has been confirmed.";

        // The whole notification block is now one line.
        NotificationResult result = _notifier.Notify(user, title, body);
        Console.WriteLine($"Notification result: {result}");
        return result;
    }
}
