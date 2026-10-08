namespace dev.kaldiroglu.TemplateMethod.Task;

/// <summary>
/// The <b>AbstractClass</b>: a general algorithm for a task that repeats. Prepare, then
/// before / the task / after as many times as asked, waiting <see cref="Interval"/> seconds
/// after each repetition, then clean up.
/// <para>
/// <see cref="Run"/> is the template method. In Java it is <c>final</c>; in C# it is a public
/// method without <c>virtual</c>, so no subclass can override it. <see cref="DoTask"/> is
/// abstract. <see cref="Prepare"/>, <see cref="Before"/>, <see cref="After"/> and
/// <see cref="Clean"/> are hooks with a default: <c>virtual</c>, and public, as in the Java.
/// </para>
/// </summary>
public abstract class Task
{
    protected string Name { get; set; }

    /// <summary>The wait after each repetition, in seconds.</summary>
    protected int Interval { get; set; }

    /// <summary>How many times the task runs. The Java field is spelled <c>repetation</c>.</summary>
    protected int Repetition { get; set; }

    protected Task(string name, int interval, int repetition)
    {
        Name = name;
        Interval = interval;
        Repetition = repetition;
    }

    public virtual void Prepare()
    {
        Console.WriteLine("*** in prepare() ***");
    }

    public virtual void Clean()
    {
        Console.WriteLine("*** in clean() ***");
    }

    public virtual void Before()
    {
        Console.WriteLine("\n- in before() -");
    }

    public virtual void After()
    {
        Console.WriteLine("- in after() -\n");
    }

    public abstract void DoTask();

    /// <summary>The template method. Not <c>virtual</c>, so no subclass can override it.</summary>
    public void Run()
    {
        Prepare();
        int repetitionCount = 0;
        while (repetitionCount < Repetition)
        {
            Before();
            DoTask();
            After();
            repetitionCount++;
            // Wait only between repetitions, not after the last one.
            if (repetitionCount < Repetition)
            {
                try
                {
                    Thread.Sleep(Interval * 1000);
                }
                catch (ThreadInterruptedException)
                {
                    // Someone asked this thread to stop: stop repeating, but still clean up.
                    break;
                }
            }
        }
        Clean();
    }
}
