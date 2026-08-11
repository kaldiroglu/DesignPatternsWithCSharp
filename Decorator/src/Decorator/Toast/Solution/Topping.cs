namespace dev.kaldiroglu.Decorator.Toast.Solution;

/// <summary>
/// The <b>Decorator</b>: a topping <i>is</i> a toast and <i>has</i> a toast.
/// <para>
/// Five of these replace the thirty-one classes a class-per-combination menu would need,
/// and each knows its own price — so repricing cheddar is an edit in one place.
/// </para>
/// </summary>
public abstract class Topping : IToastable
{
    protected readonly IToastable Component;
    private readonly string _name;
    private readonly int _price;

    protected Topping(IToastable component, string name, int price)
    {
        Component = component ?? throw new ArgumentNullException(
            nameof(component), "a topping must be added to something");
        _name = name;
        _price = price;
    }

    /// <summary>Forward, then add. The topping never asks what it is sitting on.</summary>
    public int CalculatePrice() => Component.CalculatePrice() + _price;

    public IReadOnlyList<Topping> GetToppings()
    {
        // Copy before adding. The list handed back belongs to whoever asked for it, and a
        // decorator that appended to a list it did not own would corrupt the toast below.
        var toppings = new List<Topping>(Component.GetToppings()) { this };
        return toppings.AsReadOnly();
    }

    public string Name => _name;

    public int Price => _price;

    public IToastable GetComponent() => Component;

    public override string ToString() => $"Topping [name={_name}, price={_price}]";
}
