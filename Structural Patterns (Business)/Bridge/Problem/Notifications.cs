namespace DevKaldiroglu.DP.Structural.Bridge.Problem;

public class EmailOrderConfirmation
{
    public void Send(string to, string orderId, decimal total) =>
        Console.WriteLine($"EMAIL to={to} subject='Order {orderId} confirmed' " +
                          $"body='<h1>Thanks!</h1><p>Total: ${total}</p>'");
}

public class SmsOrderConfirmation
{
    public void Send(string phone, string orderId, decimal total) =>
        Console.WriteLine($"SMS to={phone} text='Order {orderId} confirmed. Total ${total}'");
}

public class SlackOrderConfirmation
{
    public void Send(string channel, string orderId, decimal total) =>
        Console.WriteLine($"SLACK to={channel} blocks=[Order {orderId} confirmed | total=${total}]");
}

public class EmailPasswordReset
{
    public void Send(string to, string resetLink) =>
        Console.WriteLine($"EMAIL to={to} subject='Reset your password' " +
                          $"body='<a href=\"{resetLink}\">Click to reset</a>'");
}

public class SmsPasswordReset
{
    public void Send(string phone, string resetLink) =>
        Console.WriteLine($"SMS to={phone} text='Reset: {resetLink}'");
}

public static class ProblemDemo
{
    public static void Run()
    {
        new EmailOrderConfirmation().Send("a@example.com", "ORD-1", 49.95m);
        new SmsOrderConfirmation().Send("+15555550100", "ORD-1", 49.95m);
        new SlackOrderConfirmation().Send("#sales", "ORD-1", 49.95m);
        new EmailPasswordReset().Send("a@example.com", "https://example.com/reset/abc");
        new SmsPasswordReset().Send("+15555550100", "https://example.com/reset/abc");
    }
}
