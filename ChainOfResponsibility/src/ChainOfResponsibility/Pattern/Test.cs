namespace dev.kaldiroglu.ChainOfResponsibility.Pattern;

/// <summary>Asks the chain for more specific, specific and generic help, and shows each answer.</summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/ChainOfResponsibility.Demo -- pattern</c>.
/// </remarks>
public class Test
{
    public static void Run()
    {
        IHandler handler3 = new ConcreteHandler3(null);
        IHandler handler2 = new ConcreteHandler2(handler3);
        IHandler handler1 = new ConcreteHandler1(handler2);

        IHelp help = handler1.HandleRequest(Context.MORE_SPECIFIC);
        help.Show();

        Console.WriteLine();

        help = handler1.HandleRequest(Context.SPECIFIC);
        help.Show();

        Console.WriteLine();

        help = handler1.HandleRequest(Context.GENERIC);
        help.Show();
    }
}
