namespace dev.kaldiroglu.Visitor.Animal.Pattern2;

public class Dog : AbstractAnimal
{
    public Dog(string name) : base(name)
    {
    }

    public override void Eat()
    {
        Console.WriteLine("Woof");
    }

    public override void Accept(IFeeder feeder)
    {
        if (feeder is DogFeeder)
            feeder.Feed(this);
        else
            Console.WriteLine("Nop, I don't want this feeder");
    }
}
