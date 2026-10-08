namespace dev.kaldiroglu.State.Person;

/// <summary>A person says hello and goodbye, happy and then sad. The Java original's <c>main</c>.</summary>
public static class Test
{
    public static void Run()
    {
        Person person = new Person(new HappyState());
        Console.WriteLine("Hello in happy state: " + person.SayHello());
        Console.WriteLine("Goodbye in happy state: " + person.SayGoodbye());

        Console.WriteLine();

        person.SetEmotionalState(new SadState());
        Console.WriteLine("Hello in sad state: " + person.SayHello());
        Console.WriteLine("Goodbye in sad state: " + person.SayGoodbye());
    }
}
