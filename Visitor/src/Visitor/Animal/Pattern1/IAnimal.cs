namespace dev.kaldiroglu.Visitor.Animal.Pattern1;

public interface IAnimal
{
    void Eat();

    void Accept(Feeder feeder);
}
