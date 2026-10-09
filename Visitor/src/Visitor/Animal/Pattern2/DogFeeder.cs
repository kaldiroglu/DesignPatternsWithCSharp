namespace dev.kaldiroglu.Visitor.Animal.Pattern2;

public class DogFeeder : IFeeder
{
    public void Feed(IAnimal a)
    {
        Console.WriteLine("Feeding the dog " + a.Name);
    }
}
