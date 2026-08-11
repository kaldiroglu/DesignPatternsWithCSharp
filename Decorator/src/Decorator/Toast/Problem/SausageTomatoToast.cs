namespace dev.kaldiroglu.Decorator.Toast.Problem;

/// <summary>One item on the menu, and one class for it.</summary>
public class SausageTomatoToast : AbstractToast
{
    public SausageTomatoToast() => NameValue = "Sausage tomato toast";

    public override int CalculatePrice()
    {
        // Copied from SausageToast. The tomato is priced twice in this package now.
        return 6 + 2;
    }
}
