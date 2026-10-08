namespace dev.kaldiroglu.TemplateMethod.Task;

/// <summary>A <b>ConcreteClass</b>: writes <c>DoTask()</c> only.</summary>
public class Scan(string name, int interval, int repetition) : Task(name, interval, repetition)
{
    public override void DoTask()
    {
        Console.WriteLine("I'm scanning!");
    }
}
