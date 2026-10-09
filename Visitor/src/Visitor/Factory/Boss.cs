namespace dev.kaldiroglu.Visitor.Factory;

public class Boss
{
    public Boss(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public string Name { get; }

    public int Age { get; }

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}
