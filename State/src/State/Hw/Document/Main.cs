namespace dev.kaldiroglu.State.Hw.Document;

/// <summary>
/// Takes a document through review with a central transition table: an action the table
/// does not allow is refused, then the document is rejected once, approved and archived.
/// </summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/State.Demo -- hw-document</c>. Inside this namespace,
/// <c>Action</c> is the homework's enum, not <c>System.Action</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        var document = new Document(new Workflow());
        try
        {
            document.Apply(Action.APPROVE);
        }
        catch (InvalidOperationException refused)
        {
            Console.WriteLine("APPROVE -> refused: " + refused.Message);
        }
        foreach (Action action in new[]
                 {
                     Action.SUBMIT, Action.REJECT, Action.SUBMIT, Action.APPROVE, Action.ARCHIVE
                 })
        {
            document.Apply(action);
            Console.WriteLine(action + " -> " + document.Status);
        }
    }
}
