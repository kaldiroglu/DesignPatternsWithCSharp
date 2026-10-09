namespace dev.kaldiroglu.Visitor.Animal.Problem;

/// <summary>A dog and a cat eat, and then the feeder feeds each of them.</summary>
/// <remarks>
/// The Java original's <c>main</c>; its commented-out lines are not ported. Run it with
/// <c>dotnet run --project src/Visitor.Demo -- animal-problem</c>.
/// </remarks>
public static class Test
{
    public static void Run()
    {
        Feeder feeder = new Feeder();

        IAnimal a = new Dog();
        // NOTE: the comment below is wrong. This line prints "Woof". The Java has the same
        // comment and the same output.
        a.Eat(); // Prints "Gnaws bones"

        feeder.Feed(a);

        Console.WriteLine();

        a = new Cat();
        // NOTE: the comment below is wrong. This line prints "Meeoow". The Java has the same
        // comment and the same output.
        a.Eat(); // Prints "Gnaws bones"

        feeder.Feed(a);
    }
}
