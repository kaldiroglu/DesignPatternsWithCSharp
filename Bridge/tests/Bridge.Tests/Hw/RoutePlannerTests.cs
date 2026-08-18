using dev.kaldiroglu.Bridge.Hw.RoutePlanner;
using Xunit;

namespace dev.kaldiroglu.Bridge.Tests.Hw;

/// <summary>
/// Homework 3 — the map switch. The deliverable is a diff: after the swap, not one file on the
/// abstraction side was touched.
/// </summary>
public class RoutePlannerTests
{
    private static readonly string[] Hubs = ["Uskudar", "Sisli"];
    private const string From = "Kadikoy";
    private const string To = "Levent";

    [Fact(DisplayName = "the same routing code gives different answers on different maps")]
    public void TheSameRulesOverTwoProviders()
    {
        var inHouse = new FastestRoute(new InHouseMaps()).Plan(From, To, Hubs);
        var vendor = new FastestRoute(new VendorMaps()).Plan(From, To, Hubs);

        Assert.Equal(["Kadikoy", "Uskudar", "Levent"], inHouse.Stops);
        Assert.Equal(["Kadikoy", "Uskudar", "Levent"], vendor.Stops);
        Assert.Equal(2100, inHouse.Seconds);
        Assert.Equal(1620, vendor.Seconds);
        Assert.NotEqual(inHouse.Seconds, vendor.Seconds);
    }

    [Fact(DisplayName = "each route kind prefers something different on the same map")]
    public void ThreeKindsThreePreferences()
    {
        IMapProvider maps = new InHouseMaps();

        Assert.Equal(2100, new FastestRoute(maps).Plan(From, To, Hubs).Seconds);
        Assert.Equal(700, new CheapestRoute(maps).Plan(From, To, Hubs).TollMinor);
        Assert.True(new StepFreeRoute(maps).Plan(From, To, Hubs).StepFree);
    }

    [Fact(DisplayName = "better survey data changes which route is step-free, and the planner follows")]
    public void TheVendorKnowsAboutTheSteps()
    {
        // In-house believes Uskudar > Levent is step-free; the vendor surveyed it and it is not.
        Assert.True(new InHouseMaps().StepFree("Uskudar", "Levent"));
        Assert.False(new VendorMaps().StepFree("Uskudar", "Levent"));

        var inHouse = new StepFreeRoute(new InHouseMaps()).Plan(From, To, Hubs);
        var vendor = new StepFreeRoute(new VendorMaps()).Plan(From, To, Hubs);

        Assert.Equal(["Kadikoy", "Uskudar", "Levent"], inHouse.Stops);
        Assert.Equal(["Kadikoy", "Sisli", "Levent"], vendor.Stops);
        Assert.True(inHouse.StepFree);
        Assert.True(vendor.StepFree);
    }

    [Fact(DisplayName = "swapping the provider is one line, and it is not in the abstraction")]
    public void NoProviderTypeReachesTheAbstraction()
    {
        Type[] abstraction =
        [
            typeof(RoutePlanner), typeof(FastestRoute), typeof(CheapestRoute), typeof(StepFreeRoute)
        ];
        Type[] concreteProviders = [typeof(InHouseMaps), typeof(VendorMaps)];

        foreach (var type in abstraction)
        {
            foreach (var field in type.GetFields(System.Reflection.BindingFlags.Instance
                                                 | System.Reflection.BindingFlags.NonPublic
                                                 | System.Reflection.BindingFlags.Public))
            {
                Assert.DoesNotContain(field.FieldType, concreteProviders);
            }

            foreach (var constructor in type.GetConstructors())
            {
                foreach (var parameter in constructor.GetParameters())
                {
                    Assert.DoesNotContain(parameter.ParameterType, concreteProviders);
                }
            }
        }

        // The only thing the abstraction holds is the interface.
        Assert.Equal(typeof(IMapProvider), TypeCensus.Field(typeof(RoutePlanner), "Maps").FieldType);
    }

    [Fact(DisplayName = "a leg the map has never heard of fails loudly rather than guessing")]
    public void UnknownLegsAreRejected() =>
        Assert.Throws<ArgumentException>(() =>
            new FastestRoute(new InHouseMaps()).Plan("Kadikoy", "Ankara", []));
}
