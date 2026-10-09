namespace dev.kaldiroglu.Visitor.Animal.Pattern1;

public class Dog : IAnimal
{
    public void Eat()
    {
        Console.WriteLine("Woof");
    }

    public void Accept(Feeder feeder)
    {
        feeder.Feed(this);
    }
}
