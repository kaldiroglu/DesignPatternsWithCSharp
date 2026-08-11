namespace dev.kaldiroglu.Decorator.Toast.Problem;

/// <summary>What the menu items share: a name, and nothing else.</summary>
public abstract class AbstractToast : IToast
{
    protected string NameValue = "";

    public string Name => NameValue;

    public abstract int CalculatePrice();

    public override string ToString() => $"{NameValue} = {CalculatePrice()}";
}
