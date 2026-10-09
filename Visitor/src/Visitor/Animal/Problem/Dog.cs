namespace dev.kaldiroglu.Visitor.Animal.Problem;

public class Dog : IAnimal
{
    public void Eat()
    {
        Console.WriteLine("Woof");
    }
}
