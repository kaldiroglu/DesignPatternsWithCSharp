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

    /// <summary>
    /// The current date and time in the shape of Java's <c>Date.toString()</c>, for example
    /// <c>Thu Oct 08 21:52:22 GMT+03:00 2026</c>. Java prints a time zone name such as
    /// <c>CET</c> where it knows one; this prints the offset.
    /// </summary>
    private static string Today() =>
        DateTime.Now.ToString("ddd MMM dd HH:mm:ss 'GMT'zzz yyyy", CultureInfo.InvariantCulture);
}
