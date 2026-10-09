namespace dev.kaldiroglu.Iterator.Tests.Hw;

using dev.kaldiroglu.Iterator.Hw.Bom;
using dev.kaldiroglu.Iterator.Hw.Bom.Composite;
using dev.kaldiroglu.Iterator.Hw.Calendar;
using dev.kaldiroglu.Iterator.Hw.Paging;
using Xunit;

/// <summary>
/// The worked solutions of the Iterator homework, and the figures its speaker notes quote.
/// Ported from the Java <c>hw.HomeworkTest</c>.
/// </summary>
public class HomeworkTests
{
    // ------------------------------------------------------------ 1 · parts of a bicycle

    /// <summary>The parts of the city bicycle that the tests look at.</summary>
    private sealed record Bicycle(Assembly Whole, Assembly Frame, Assembly Wheel, Assembly Hub, Part Spoke);

    /// <summary>
    /// The city bicycle of the Composite deck. The Java tests take it from
    /// <c>ProductCatalog.cityBicycle()</c> in the Composite package. The C# port copies only
    /// the bill-of-materials types, not the catalog, so the same tree is built here, with the
    /// same part numbers, names and quantities.
    /// <para>
    /// Java builds each wheel with 32 spokes and then calls
    /// <c>wheel.changeQuantity(spoke, 36)</c>. The C# <c>Assembly</c> has no
    /// <c>ChangeQuantity</c>, so the wheel is built with 36 spokes from the start, which is the
    /// state the Java test checks.
    /// </para>
    /// </summary>
    private static Bicycle CityBicycle(int spokesPerWheel)
    {
        var rim = new Part("RIM-700C", "700c Rim", Money.Of(24.00m), 850);
        var spoke = new Part("SPOKE-14G", "14g Spoke", Money.Of(0.40m), 5);
        var axle = new Part("AXLE-QR", "Quick-release Axle", Money.Of(6.50m), 120);
        var bearing = new Part("BEARING-6001", "6001 Sealed Bearing", Money.Of(2.10m), 15);
        var tire = new Part("TIRE-700x25", "700x25 Tire", Money.Of(18.00m), 260);
        var tube = new Part("TUBE-700", "700c Inner Tube", Money.Of(4.50m), 95);
        var saddle = new Part("SADDLE-CR", "Cromoly Saddle", Money.Of(18.00m), 310);
        var tubeset = new Part("TUBESET-CR", "Cromoly Tubeset", Money.Of(95.00m), 1800);
        var fork = new Part("FORK-CR", "Cromoly Fork", Money.Of(42.00m), 700);
        var paint = new Part("PAINT-KIT", "Paint & Decals", Money.Of(6.00m), 40);

        var hub = new Assembly("HUB-ASM", "Wheel Hub")
            .Add(axle)
            .Add(bearing, 2);
        var wheel = new Assembly("WHEEL-ASM", "700c Wheel")
            .Add(rim)
            .Add(spoke, spokesPerWheel)
            .Add(hub)
            .Add(tire)
            .Add(tube);
        var frame = new Assembly("FRAME-ASM", "Frame Assembly")
            .Add(tubeset)
            .Add(fork)
            .Add(paint);
        var bicycle = new Assembly("BIKE-CITY", "City Bicycle")
            .Add(frame)
            .Add(wheel, 2)
            .Add(saddle);
        return new Bicycle(bicycle, frame, wheel, hub, spoke);
    }

    private static Service PowderCoating() => new("SVC-COAT", "Powder Coating", Money.Of(14.00m));

    [Fact(DisplayName = "two wheels with 36 spokes each give one line of 72 spokes")]
    public void SpokesAreMultipliedDownTheTree()
    {
        var bike = CityBicycle(spokesPerWheel: 36);

        var spokes = new List<PartLine>();
        foreach (var line in PartIterator.PartsOf(bike.Whole))
        {
            if (line.Part == bike.Spoke)
            {
                spokes.Add(line);
            }
        }

        // One wheel object used twice gives one spoke line.
        Assert.Single(spokes);
        Assert.Equal(72, spokes[0].Quantity);
        Assert.Equal("72 x " + bike.Spoke.Name, spokes[0].ToString());
    }

    [Fact(DisplayName = "the part iterator returns parts only: assemblies and services are skipped")]
    public void OnlyPartsComeOut()
    {
        var bike = CityBicycle(spokesPerWheel: 32);
        var coating = PowderCoating();
        bike.Frame.Add(coating);

        var names = new List<string>();
        foreach (var line in PartIterator.PartsOf(bike.Whole))
        {
            names.Add(line.Part.Name);
        }

        Assert.DoesNotContain(bike.Wheel.Name, names);
        Assert.DoesNotContain(bike.Hub.Name, names);
        Assert.DoesNotContain(coating.Name, names);
        Assert.Contains(bike.Spoke.Name, names);
    }

    [Fact(DisplayName = "the part iterator throws NoSuchElementException after the last part")]
    public void PartIteratorEnds()
    {
        var parts = new PartIterator(CityBicycle(spokesPerWheel: 32).Hub);
        while (parts.MoveNext())
        {
        }

        // Java's next() after the end throws NoSuchElementException. In C# MoveNext()
        // answers false, and Current then throws InvalidOperationException.
        Assert.False(parts.MoveNext());
        Assert.Throws<InvalidOperationException>(() => parts.Current);
    }

    // ------------------------------------------------------------ 2 · business days

    [Fact(DisplayName = "business days skip weekends and holidays, and no list of dates is stored")]
    public void BusinessDaysSkipWeekendsAndHolidays()
    {
        // Friday 2026-10-23 to Friday 2026-10-30; 2026-10-29 is a holiday.
        var holiday = new DateOnly(2026, 10, 29);
        var days = new BusinessDays(new DateOnly(2026, 10, 23), new DateOnly(2026, 10, 30), [holiday]);

        var walked = new List<DateOnly>();
        foreach (var day in days)
        {
            walked.Add(day);
        }

        Assert.Equal([
            new DateOnly(2026, 10, 23),
            new DateOnly(2026, 10, 26),
            new DateOnly(2026, 10, 27),
            new DateOnly(2026, 10, 28),
            new DateOnly(2026, 10, 30)], walked);
    }

    // ------------------------------------------------------------ 3 · paged results

    [Fact(DisplayName = "a caller that stops early fetches only the pages up to the one where it stopped")]
    public void PagesAfterTheStopAreNeverFetched()
    {
        var asked = new List<int>();
        using var results = new PagedIterator<string>(number =>
        {
            asked.Add(number);
            return number < 5 ? ["p" + number + "a", "p" + number + "b"] : [];
        });

        string? found = null;
        while (results.MoveNext())
        {
            var item = results.Current;
            if (item == "p1a")
            {
                found = item;
                break;
            }
        }

        Assert.Equal("p1a", found);
        Assert.Equal(2, results.PagesFetched);
        Assert.Equal([0, 1], asked);
    }

    [Fact(DisplayName = "a caller that walks to the end gets every item, and the empty page ends the walk")]
    public void WalkingToTheEndFetchesEveryPage()
    {
        using var results = new PagedIterator<string>(
            number => number < 3 ? ["p" + number] : []);

        // Java's forEachRemaining walks to the end; C# walks with MoveNext() and Current.
        var all = new List<string>();
        while (results.MoveNext())
        {
            all.Add(results.Current);
        }

        Assert.Equal(["p0", "p1", "p2"], all);
        // Three pages and the empty one.
        Assert.Equal(4, results.PagesFetched);
        // Java's next() after the end throws NoSuchElementException. In C# MoveNext()
        // answers false, and Current then throws InvalidOperationException.
        Assert.False(results.MoveNext());
        Assert.Throws<InvalidOperationException>(() => results.Current);
    }
}
