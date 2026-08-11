namespace dev.kaldiroglu.Decorator.Toast.Solution;

/// <summary>A <b>ConcreteDecorator</b>: one topping, one class, one price.</summary>
public class Ketchup : Topping
{
    public Ketchup(IToastable component) : base(component, "Ketchup", 1)
    {
    }
}
