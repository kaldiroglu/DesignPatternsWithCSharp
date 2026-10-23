namespace dev.kaldiroglu.Visitor.Animal.Problem;

/// <summary>
/// One <c>Feed</c> for every animal, which tests the type and casts.
/// </summary>
public class Feeder
{
    public void Feed(IAnimal a)
    {
        if (a is Dog)
        {
            Dog dog = (Dog)a;
            dog.Eat();
        }
        else if (a is Cat)
        {
            Cat cat = (Cat)a;
            cat.Eat();
        }
    }
}
