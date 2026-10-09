namespace dev.kaldiroglu.Visitor.Animal.Pattern2;

public class Cat : AbstractAnimal
{
    public Cat(string name) : base(name)
    {
    }

    public override void Eat()
    {
        Console.WriteLine("Meeoow");
    }

    public override void Accept(IFeeder feeder)
    {
        if (feeder is CatFeeder)
            feeder.Feed(this);
        else
            Console.WriteLine("Nop, I don't want this feeder");
    }
}
