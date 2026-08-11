namespace dev.kaldiroglu.Decorator.Toast.Solution;

/// <summary>The <b>ConcreteComponent</b>: the only thing that knows a price by itself.</summary>
public class ToastBread : IToastable
{
    private readonly string _name;
    private readonly int _price;

    public ToastBread() : this("Toast bread", 5)
    {
    }

    public ToastBread(string name, int price)
    {
        _name = name;
        _price = price;
    }

    public string Name => _name;

    public int CalculatePrice() => _price;

    public IReadOnlyList<Topping> GetToppings() => [];

    public override string ToString() => $"{_name} ({_price})";
}
