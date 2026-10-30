namespace dev.kaldiroglu.Observer.Price.Problem;

/// <summary>
/// Stage three: <b>the readers poll more often, and act only when the price changed.</b>
/// <para>
/// The best of the three. The feed still knows nobody. This class runs a thread that wakes
/// up ten times a second. The readers poll more often, so they
/// are less late, and they remember the last price, so an unchanged price costs one read
/// and no work: the chart no longer draws the same point again and again.
/// </para>
/// <para>
/// What it cannot fix: a reader sees the price only at the moment it asks. If the price
/// goes from 100 to 106 and back to 100 between two polls, no reader ever sees 106, and the
/// alert at 105 never fires. Polling more often makes this rarer and wastes more reads; it
/// never makes it impossible.
/// </para>
/// </summary>
public sealed class ChangeOnlyReaders
{
    private const int OneTenthOfASecond = 100;

    private readonly PriceFeed feed;
    private readonly Chart chart;
    private readonly Ticker ticker;
    private readonly PriceAlert alert;
    private int lastSeen;
    private Thread? thread;

    public ChangeOnlyReaders(PriceFeed feed, Chart chart, Ticker ticker, PriceAlert alert)
    {
        this.feed = feed;
        this.chart = chart;
        this.ticker = ticker;
        this.alert = alert;
        lastSeen = feed.Price();
    }

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

    /// <summary>Polls ten times a second, until the thread is interrupted.</summary>
    private void Run()
    {
        try
        {
            while (true)
            {
                Poll();
                Thread.Sleep(OneTenthOfASecond);
            }
        }
        catch (ThreadInterruptedException)
        {
            // the thread was asked to stop
        }
    }

    public void Poll()
    {
        int price = feed.Price();
        if (price == lastSeen)
        {
            return;                     // nothing new: one read, no work
        }
        lastSeen = price;
        chart.Add(price);
        ticker.Show(price);
        alert.Check(price);
    }
}
