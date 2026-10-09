namespace dev.kaldiroglu.Memento.Pattern1;

/// <summary>
/// The client: one thread changes the originator's state every second, and the caretaker's
/// thread saves every two seconds and undoes every fifth time. The output depends on the
/// timing of the two threads.
/// </summary>
public static class Test
{
    private static Originator? originator;
    private static Memento? memento;

    /// <summary>
    /// Starts the two threads and waits for them. The Java <c>main</c> returns at once and the
    /// JVM waits for the threads; waiting here gives the same output, and lets the runner start
    /// the next example only after this one has finished.
    /// </summary>
    public static void Run()
    {
        originator = new Originator("state-0");
        memento = originator.Memento;

        OriginatorTrigger trigger = new OriginatorTrigger();
        trigger.Start();

        Caretaker caretaker = new Caretaker(memento);
        caretaker.Start();

        trigger.Join();
        caretaker.Join();
    }

    /// <summary>
    /// Changes the originator's state every second, on its own thread. In the Java it extends
    /// <c>Thread</c>; here it holds one, as <see cref="Caretaker"/> does. The Java class is
    /// package-private, so this one is <c>internal</c>.
    /// </summary>
    internal class OriginatorTrigger
    {
        private readonly Thread thread;

        public OriginatorTrigger()
        {
            thread = new Thread(Run);
        }

        public void Start()
        {
            thread.Start();
        }

        public void Join()
        {
            thread.Join();
        }

        public void Run()
        {
            for (int i = 1; i < 20; i++)
            {
                string state = "state-" + i.ToString(System.Globalization.CultureInfo.InvariantCulture);
                originator!.SetState(state);
                try
                {
                    Thread.Sleep(1000);
                }
                catch (ThreadInterruptedException e)
                {
                    Console.Error.WriteLine(e);
                }
            }
        }
    }
}
