namespace dev.kaldiroglu.Decorator.Toast.Problem;

/// <summary>BEFORE: every item on the menu is a class.</summary>
public interface IToast
{
    int CalculatePrice();

    string Name { get; }
}
