namespace dev.kaldiroglu.Observer.Price.Problem;

/// <summary>
/// Stage two: <b>the readers poll the feed.</b>
/// <para>
/// A real improvement on stage one: the feed knows nobody, and a new reader needs no change
/// to the feed. This class runs a thread: it wakes up every second, and each reader asks for
/// the price. A new reader is still an edit to this class.
/// </para>
/// <para>
/// What it costs: a change is seen only at the next poll, so readers are late; most polls
/// find the same price; and a change that is undone before the next poll is never seen.
/// </para>
/// </summary>
public sealed class PollingReaders(PriceFeed feed, Chart chart, Ticker ticker, PriceAlert alert)
{
    private const int OneSecond = 1_000;

    private Thread? thread;

    /// <summary>Starts the thread.</summary>
    public void Start()
    {
        thread = new Thread(Run);
        thread.Start();
    }

    /// <summary>Stops the thread and waits for it to end.</summary>
    public void Stop()
    {
        thread?.Interrupt();
        thread?.Join();
    }

    /// <summary>Polls once a second, until the thread is interrupted.</summary>
    private void Run()
    {
        try
        {
            while (true)
            {
                Poll();
                Thread.Sleep(OneSecond);
            }
        }
        catch (ThreadInterruptedException)
        {
            // the thread was asked to stop
        }
    }

    public void Poll()
    {
        chart.Add(feed.Price());
        ticker.Show(feed.Price());
        alert.Check(feed.Price());
    }
}
