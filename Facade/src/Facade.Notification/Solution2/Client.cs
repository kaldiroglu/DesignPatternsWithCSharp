using dev.kaldiroglu.Facade.Notification.Solution;

namespace dev.kaldiroglu.Facade.Notification.Solution2;

/// <summary>
/// Each client type picks its own channel subset. The facade still owns dispatch,
/// constraints, logging and error isolation.
/// </summary>
public class Client
{
    private readonly NotificationFacade _notifier;

    public Client(NotificationFacade notifier)
    {
        _notifier = notifier;
    }

    // OrderService: email + SMS (customer-facing)
    public void ConfirmOrder(User user, string orderId)
    {
        Console.WriteLine($"Order {orderId} confirmed.");

        string title = $"Order #{orderId} Confirmed";
        string body = $"Your order #{orderId} has been confirmed.";

        var result = _notifier.Notify(user, new HashSet<Channel> { Channel.Email, Channel.Sms }, title, body);
        Console.WriteLine($"  → {result}");
    }

    // AlertService: Slack + push only (ops + mobile)
    public void FireAlert(User user, string alertMessage)
    {
        Console.WriteLine($"ALERT: {alertMessage}");

        var result = _notifier.Notify(user, new HashSet<Channel> { Channel.Slack, Channel.Push },
            $"[ALERT] {alertMessage}", alertMessage);
        Console.WriteLine($"  → {result}");
    }

    // PasswordResetService: email only (security policy)
    public void SendResetLink(User user, string token)
    {
        Console.WriteLine($"Password reset for {user.Id}");

        string body = $"Click here to reset: https://app.com/reset?t={token}";

        var result = _notifier.Notify(user, new HashSet<Channel> { Channel.Email }, "Password Reset", body);
        Console.WriteLine($"  → {result}");
    }

    public static void Run()
    {
        var config = new NotificationConfig(
            "smtp.company.com", 587, "noreply", "***",
            "AC123", "token", "+1555000",
            "https://hooks.slack.com/...",
            "/secrets/firebase.json",
            "/var/log/notifications.log");

        var client = new Client(new NotificationFacade(config));

        var user = new User("user_42")
        {
            Email = "alice@example.com",
            Phone = "+15551234567",
            SlackChannel = "#alerts",
            DeviceToken = "dev_token_abc123",
        };

        Console.WriteLine("=== Order: email + SMS ===");
        client.ConfirmOrder(user, "ORD-9876");

        Console.WriteLine("\n=== Alert: slack + push ===");
        client.FireAlert(user, "CPU at 95% on prod-03");

        Console.WriteLine("\n=== Password reset: email only ===");
        client.SendResetLink(user, "aB3xZ9kL");
    }
}
