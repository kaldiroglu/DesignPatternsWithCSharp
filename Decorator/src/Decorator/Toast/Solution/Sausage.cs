namespace dev.kaldiroglu.Decorator.Toast.Solution;

/// <summary>A <b>ConcreteDecorator</b>: one topping, one class, one price.</summary>
public class Sausage : Topping
{
    public Sausage(IToastable component) : base(component, "Sucuk sausage", 3)
    {
    }
}
