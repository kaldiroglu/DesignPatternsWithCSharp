using System.Reflection;
using dev.kaldiroglu.Flyweight.Quote.Problem;
using Xunit;

namespace dev.kaldiroglu.Flyweight.Tests;

/// <summary>
/// The feed example's problem. The tests decode fields the way a real handler does, so the
/// counts are the counts a real handler would see.
/// </summary>
public class QuoteFlyweightTests
{
    private const int Ticks = 10_000;

    [Fact(DisplayName = "String literals hide the problem: the compiler already interned them")]
    public void LiteralsMakeTheProblemInvisible()
    {
        string one = "AAPL";
        string two = "AAPL";

        Assert.Same(one, two);
    }

    [Fact(DisplayName = "Decoded off the wire, every tick allocates its own symbol")]
    public void DecodedFieldsAreDistinctObjects()
    {
        Wire.Reset();

        string first = Wire.Decode("AAPL");
        string second = Wire.Decode("AAPL");

        Assert.Equal(first, second);
        Assert.NotSame(first, second);
        Assert.Equal(2, Wire.DecodeCount());
    }

    [Fact(DisplayName = "The problem retains one metadata object per tick")]
    public void ProblemRetainsMetadataPerTick()
    {
        var feed = new FeedHandler();

        for (int i = 0; i < Ticks; i++)
        {
            feed.OnTick(Wire.Decode("AAPL"), Wire.Decode("NASDAQ"), Wire.Decode("USD"),
                Wire.Decode("US0378331005"), Wire.Decode("Technology"), Wire.Decode("Apple Inc."),
                0.01m, 100, 190.00m, 190.05m, 190.02m, 1);
        }

        var distinct = new HashSet<string>(ReferenceEqualityComparer.Instance);
        foreach (var quote in feed.Quotes())
        {
            distinct.Add(quote.Symbol());
        }

        Assert.Equal(Ticks, feed.Total());
        Assert.Equal(Ticks, distinct.Count);
    }

    [Fact(DisplayName = "The per-event object carries thirteen fields")]
    public void TheEventCarriesThirteenFields()
    {
        int fields = typeof(dev.kaldiroglu.Flyweight.Quote.Problem.Quote).GetFields(BindingFlags.Instance | BindingFlags.NonPublic).Length;

        Assert.Equal(13, fields);
    }
}
