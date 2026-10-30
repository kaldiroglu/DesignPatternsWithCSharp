using dev.kaldiroglu.Decorator.Middleware.Domain;
using dev.kaldiroglu.Decorator.Middleware.Problem;
using Xunit;

namespace dev.kaldiroglu.Decorator.Tests.Middleware;

public class SubclassChainTests
{
    private const string Sku = "SKU-200";

    [Fact(DisplayName = "subclasses: the combination class works")]
    public void TheCombinationWorks()
    {
        var feed = new CachingRetryingLoggingPriceFeed(IClock.Manual(), 3, new CallLog(), TimeSpan.FromSeconds(60));

        feed.FailNext(1);
        feed.QuoteFor(Sku);
        feed.QuoteFor(Sku);

        Assert.Equal(2, feed.CallCount);
    }

    [Fact(DisplayName = "subclasses: the supplier is the top of the hierarchy, so a sealed vendor class cannot take its place")]
    public void TheSupplierIsTheTopOfTheHierarchy()
    {
        Assert.True(typeof(VendorPriceFeed).IsSealed);
        Assert.Equal(typeof(SimulatedRemotePriceFeed), typeof(BasicPriceFeed).BaseType);
    }

    [Fact(DisplayName = "copy-paste and flags hold a field, so both work on the sealed vendor class")]
    public void TheFirstTwoAttemptsWorkOnASealedClass()
    {
        var clock = IClock.Manual();
        var vendor = new VendorPriceFeed();

        new CopyPasteOrderService(vendor, clock, new CallLog()).PriceForOrder(Sku);
        FlaggedPriceFeed.FullyEnabled(vendor, clock, new CallLog(), new Metrics()).QuoteFor(Sku);

        Assert.Equal(2, vendor.CallCount);
    }
}
