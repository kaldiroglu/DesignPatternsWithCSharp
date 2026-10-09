namespace dev.kaldiroglu.Visitor.Animal.Pattern1;

public class Cat : IAnimal
{
    public void Eat()
    {
        Console.WriteLine("Meeoow");
    }

    public void Accept(Feeder feeder)
    {
        feeder.Feed(this);
    }
}
