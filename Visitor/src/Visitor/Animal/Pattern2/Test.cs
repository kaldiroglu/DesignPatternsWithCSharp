namespace dev.kaldiroglu.Visitor.Animal.Pattern2;

/// <summary>A dog accepts the dog feeder, and a cat accepts the cat feeder.</summary>
/// <remarks>
/// The Java original's <c>main</c>; its commented-out lines, which pass the wrong feeder, are
/// not ported. Run it with <c>dotnet run --project src/Visitor.Demo -- animal-pattern2</c>.
/// </remarks>
public static class Test
{
    public static void Run()
    {
        IFeeder dogFeeder = new DogFeeder();
        IFeeder catFeeder = new CatFeeder();

        IAnimal a = new Dog("karabas");
        a.Accept(dogFeeder);

        a = new Cat("sarman");
        a.Accept(catFeeder);
    }
}
