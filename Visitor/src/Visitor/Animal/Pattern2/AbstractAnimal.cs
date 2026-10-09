namespace dev.kaldiroglu.Visitor.Animal.Pattern2;

/// <summary>
/// Holds the name. Java's abstract class does not mention <c>eat</c> and <c>accept</c>; a C#
/// abstract class must declare every member of its interface, so both are declared
/// <c>abstract</c> here.
/// </summary>
public abstract class AbstractAnimal : IAnimal
{
    private readonly string name;

    public AbstractAnimal(string name)
    {
        this.name = name;
    }

    public string Name => name;

    public abstract void Eat();

    public abstract void Accept(IFeeder feeder);
}
