namespace dev.kaldiroglu.Visitor.Checkout.Problem;

/// <summary>
/// Stage three: the tax class tests the type of each item. With three items the tax and the
/// shipping are correct.
/// </summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/Visitor.Demo -- checkout-problem-stage-three</c>.
/// </remarks>
public static class StageThreeMain
{
    public static void Run()
    {
        var cart = SampleCart.ThreeItems();
        Console.WriteLine("Stage three, type tests: tax " + new TypeTestTax().Total(cart)
                + ", shipping " + new TypeTestShipping().Total(cart));
    }
}
