namespace dev.kaldiroglu.Decorator.Toast.Solution;

/// <summary>
/// The <b>Component</b>: bread and toppings answer the same two questions, so a topping
/// can sit on either.
/// </summary>
public interface IToastable
{
    int CalculatePrice();

    IReadOnlyList<Topping> GetToppings();
}
