namespace dev.kaldiroglu.State.Order.Solution;

/// <summary>
/// A shipped order is asked to cancel. The Shipped state does not allow it, so the default in
/// IOrderState refuses.
/// </summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/State.Demo -- order-refused-request</c>.
/// </remarks>
public static class RefusedRequestMain
{
    public static void Run()
    {
        var order = new Order();
        order.Pay();
        order.Ship("TR-1");
        try
        {
            order.Cancel();
        }
        catch (InvalidOperationException refused)
        {
            Console.WriteLine("Cancel a shipped order: " + refused.Message);
        }
    }
}
