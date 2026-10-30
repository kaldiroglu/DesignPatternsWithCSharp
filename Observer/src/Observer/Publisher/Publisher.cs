using System.Globalization;

namespace dev.kaldiroglu.Observer.Publisher;

public class Publisher
{
    private readonly IPublication newsweek;
    private readonly IPublication fourFourTwo;

    public Publisher()
    {
        newsweek = new Newsweek("Newsweek");
        fourFourTwo = new FourFourTwo("FourFourTwo");
    }

    public void PublishNewsweek()
    {
        Console.WriteLine("\n New Newsweek On The Way");
        newsweek.Publish(Today());
    }

    public void PublishFourFourTwo()
    {
        Console.WriteLine("\n New FourFourTwo On The Way");
        fourFourTwo.Publish(Today());
    }

    public IPublication Newsweek => newsweek;

    public IPublication FourFourTwo => fourFourTwo;

    /// <summary>Today as year, month and day, for example <c>2026-10-31</c>.</summary>
    private static string Today() =>
        DateOnly.FromDateTime(DateTime.Now).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
}
