namespace dev.kaldiroglu.TemplateMethod.Task;

/// <summary>A <b>ConcreteClass</b>: writes <c>DoTask()</c> only.</summary>
public class Scan(int interval, int repetition) : Task(interval, repetition)
{
    protected override void DoTask()
    {
        Console.WriteLine("I'm scanning!");
    }
}
