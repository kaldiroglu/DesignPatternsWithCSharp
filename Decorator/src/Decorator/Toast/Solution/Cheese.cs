namespace dev.kaldiroglu.Decorator.Toast.Solution;

/// <summary>A <b>ConcreteDecorator</b>: one topping, one class, one price.</summary>
public class Cheese : Topping
{
    public Cheese(IToastable component) : base(component, "Cheddar cheese", 3)
    {
    }
}
