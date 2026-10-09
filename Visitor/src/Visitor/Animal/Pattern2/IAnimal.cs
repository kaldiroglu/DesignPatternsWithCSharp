namespace dev.kaldiroglu.Visitor.Animal.Pattern2;

public interface IAnimal
{
    void Eat();

    string Name { get; }

    void Accept(IFeeder feeder);
}
