namespace dev.kaldiroglu.Visitor.Animal.Pattern2;

/// <summary>
/// Holds the name. A C# abstract class must declare every member of its interface, so both are declared
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
