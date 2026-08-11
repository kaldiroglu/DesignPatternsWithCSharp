namespace dev.kaldiroglu.Decorator.Toast.Problem;

/// <summary>One item on the menu, and one class for it.</summary>
public class SausageToast : AbstractToast
{
    public SausageToast() => NameValue = "Sausage toast";

    public override int CalculatePrice()
    {
        // bread plus sucuk, welded together the same way.
        return 6;
    }
}
