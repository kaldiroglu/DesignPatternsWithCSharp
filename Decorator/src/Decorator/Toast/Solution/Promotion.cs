namespace dev.kaldiroglu.Decorator.Toast.Solution;

/// <summary>
/// A decorator that <b>multiplies</b> rather than adds — and where it sits changes the
/// bill. Outermost it discounts everything; innermost it discounts only the bread.
/// <para>
/// It is also a decorator that is not a topping, which is why it implements
/// <see cref="IToastable"/> directly rather than extending <see cref="Topping"/>.
/// </para>
/// </summary>
public class Promotion : IToastable
{
    private readonly IToastable _component;
    private readonly string _description;
    private readonly int _percentageOff;

    public Promotion(IToastable component, string description, int percentageOff)
    {
        _component = component ?? throw new ArgumentNullException(nameof(component));
        _description = description;
        _percentageOff = percentageOff;
    }

    public int CalculatePrice() => _component.CalculatePrice() * (100 - _percentageOff) / 100;

    /// <summary>A discount is not a topping, so it adds nothing here.</summary>
    public IReadOnlyList<Topping> GetToppings() => _component.GetToppings();

    public override string ToString() => $"Promotion [{_description}, {_percentageOff}% off]";
}
