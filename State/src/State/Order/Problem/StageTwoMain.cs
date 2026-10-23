namespace dev.kaldiroglu.State.Order.Problem;

/// <summary>
/// Stage two: one status and a switch. A shipped order refuses to be cancelled.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/State.Demo -- order-problem-stage-two</c>.
/// </remarks>
public static class StageTwoMain
{
    public static void Run()
    {
        var switching = new SwitchingOrder();
        switching.Pay();
        switching.Ship("TR-1");
        try
        {
            switching.Cancel();
        }
        catch (InvalidOperationException refused)
        {
            Console.WriteLine("Stage two, one switch:   " + refused.Message);
        }
    }
}
