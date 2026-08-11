namespace dev.kaldiroglu.Decorator.Toast.Solution;

/// <summary>A <b>ConcreteDecorator</b>: one topping, one class, one price.</summary>
public class Salad : Topping
{
    public Salad(IToastable component) : base(component, "Russian salad", 2)
    {
    }
}
