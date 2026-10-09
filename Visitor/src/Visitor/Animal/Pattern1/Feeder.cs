namespace dev.kaldiroglu.Visitor.Animal.Pattern1;

/// <summary>
/// One <c>Feed</c> for each kind of animal. The animal's <c>Accept</c> calls the right one,
/// because inside <c>Dog.Accept</c> the static type of <c>this</c> is <c>Dog</c>. The
/// commented-out <c>Feed(Animal)</c> in the Java is not ported.
/// </summary>
public class Feeder
{
    public void Feed(Dog d)
    {
        d.Eat();
    }

    public void Feed(Cat c)
    {
        c.Eat();
    }
}
