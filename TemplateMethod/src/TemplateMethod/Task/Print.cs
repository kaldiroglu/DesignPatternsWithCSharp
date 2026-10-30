namespace dev.kaldiroglu.TemplateMethod.Task;

/// <summary>A <b>ConcreteClass</b> that also overrides the <c>Prepare</c> and <c>Clean</c> hooks.</summary>
public class Print(int interval, int repetition) : Task(interval, repetition)
{
    protected override void Prepare()
    {
        Console.WriteLine("*** Preparing printing! ***");
    }

    protected override void Clean()
    {
        Console.WriteLine("*** Cleaning printing environment. ***");
    }

    protected override void DoTask()
    {
        Console.WriteLine("Printing task.");
    }
}
