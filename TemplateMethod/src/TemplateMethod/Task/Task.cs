namespace dev.kaldiroglu.TemplateMethod.Task;

/// <summary>
/// The <b>AbstractClass</b>: a general algorithm for a task that repeats. Prepare, then
/// before / the task / after as many times as asked, waiting <see cref="Interval"/> seconds
/// after each repetition, then clean up.
/// <para>
/// <see cref="Run"/> is the template method. It is a public method without <c>virtual</c>,
/// so no subclass can override it. <see cref="DoTask"/> is abstract. <see cref="Prepare"/>,
/// <see cref="Before"/>, <see cref="After"/> and
/// <see cref="Clean"/> are hooks with a default: <c>virtual</c>, and protected.
/// </para>
/// </summary>
public abstract class Task
{
    /// <summary>The wait after each repetition, in seconds.</summary>
    protected int Interval { get; set; }

    /// <summary>How many times the task runs.</summary>
    protected int Repetition { get; set; }

    protected Task(int interval, int repetition)
    {
        Interval = interval;
        Repetition = repetition;
    }

    protected virtual void Prepare()
    {
        Console.WriteLine("*** in prepare() ***");
    }

    protected virtual void Clean()
    {
        Console.WriteLine("*** in clean() ***");
    }

    protected virtual void Before()
    {
        Console.WriteLine("\n- in before() -");
    }

    protected virtual void After()
    {
        Console.WriteLine("- in after() -\n");
    }

    protected abstract void DoTask();

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
