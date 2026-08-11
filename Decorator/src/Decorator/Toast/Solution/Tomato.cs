namespace dev.kaldiroglu.Decorator.Toast.Solution;

/// <summary>A <b>ConcreteDecorator</b>: one topping, one class, one price.</summary>
public class Tomato : Topping
{
    public Tomato(IToastable component) : base(component, "Tomato", 2)
    {
    }
}
