namespace dev.kaldiroglu.Visitor.Animal.Pattern2;

public class CatFeeder : IFeeder
{
    public void Feed(IAnimal a)
    {
        Console.WriteLine("Feeding the cat " + a.Name);
    }
}
