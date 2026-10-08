namespace dev.kaldiroglu.TemplateMethod.Task;

/// <summary>A <b>ConcreteClass</b> that also overrides the <c>Prepare</c> and <c>Clean</c> hooks.</summary>
public class Print(string name, int interval, int repetition) : Task(name, interval, repetition)
{
    public override void Prepare()
    {
        Console.WriteLine("*** Preparing printing! ***");
    }

    public override void Clean()
    {
        Console.WriteLine("*** Cleaning printing environment. ***");
    }

    public override void DoTask()
    {
        Console.WriteLine("Printing task.");
    }
}
