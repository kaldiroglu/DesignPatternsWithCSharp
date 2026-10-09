namespace dev.kaldiroglu.Visitor.Animal.Pattern1;

/// <summary>A dog and a cat each accept the feeder.</summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Visitor.Demo -- animal-pattern1</c>.
/// </remarks>
public static class Test
{
    public static void Run()
    {
        Feeder feeder = new Feeder();

        IAnimal a = new Dog();
        a.Accept(feeder);

        a = new Cat();
        a.Accept(feeder);
    }
}
