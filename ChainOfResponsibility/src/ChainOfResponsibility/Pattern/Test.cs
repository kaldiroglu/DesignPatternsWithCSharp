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
        IHandler? handler1 = null;
        IHandler? handler2 = null;
        IHandler? handler3 = null;

        // NOTE: each handler is built before its predecessor exists, so every predecessor
        // passed here is null. The Java has the same behavior.
        handler3 = new ConcreteHandler3(null, handler2);
        handler2 = new ConcreteHandler2(handler3, handler1);
        handler1 = new ConcreteHandler1(handler2, null);

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
