namespace dev.kaldiroglu.Decorator.Toast.Problem;

/// <summary>One item on the menu, and one class for it.</summary>
public class CheeseTomatoToast : AbstractToast
{
    public CheeseTomatoToast() => NameValue = "Cheese tomato toast";

    public override int CalculatePrice()
    {
        // Copied from CheeseToast, plus a tomato nobody else prices.
        return 5 + 2;
    }
}
