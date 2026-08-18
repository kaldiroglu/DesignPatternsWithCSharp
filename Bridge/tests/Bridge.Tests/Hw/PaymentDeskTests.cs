using dev.kaldiroglu.Bridge.Hw.PaymentDesk;
using Xunit;

namespace dev.kaldiroglu.Bridge.Tests.Hw;

/// <summary>
/// Homework 2 — the payment desk. The cash drawer is the whole exercise: it has no hold, and the
/// abstraction must not find out.
/// </summary>
public class PaymentDeskTests
{
    private const string PaymentDeskNs = "dev.kaldiroglu.Bridge.Hw.PaymentDesk";
    private const decimal Thousand = 1000.00m;

    private static List<IPaymentProvider> Providers() =>
        [new BankGateway(), new Wallet(), new CashDrawer()];

    [Fact(DisplayName = "one rounding rule, the same three installments on every provider")]
    public void OneRuleEveryProvider()
    {
        foreach (var provider in Providers())
        {
            var receipts = new InstallmentPlan(provider, 3).Collect(Thousand);

            Assert.Equal(3, receipts.Count);
            Assert.Equal([333.34m, 333.33m, 333.33m], receipts.Select(r => r.Amount));
            Assert.Equal(Thousand, receipts.Sum(r => r.Amount));
            Assert.All(receipts, r => Assert.Equal(provider.Name, r.Provider));
        }
    }

    [Fact(DisplayName = "cash collapses the two phases, and the abstraction cannot tell")]
    public void CashHasNoHold()
    {
        var bank = new BankGateway().Authorize(Thousand);
        var cash = new CashDrawer().Authorize(Thousand);

        Assert.False(bank.Settled);   // the money has not moved yet
        Assert.True(cash.Settled);    // it is in the drawer

        // Both still answer Capture, so OneOffPayment is written once.
        Assert.Equal(Thousand, new OneOffPayment(new BankGateway()).Collect(Thousand)[0].Amount);
        Assert.Equal(Thousand, new OneOffPayment(new CashDrawer()).Collect(Thousand)[0].Amount);
    }

    [Fact(DisplayName = "no primitive asks which provider it is talking to")]
    public void NoCapabilityBooleans()
    {
        foreach (var method in typeof(IPaymentProvider).GetMethods())
        {
            Assert.False(method.ReturnType == typeof(bool),
                $"{method.Name} returns a boolean the abstraction would branch on, which is "
                + "'which implementation are you?' in disguise");
        }
    }

    [Fact(DisplayName = "refunds run through the same bridge")]
    public void RefundsToo()
    {
        var receipts = new Refund(new Wallet(), "WLT-1").Collect(250.00m);

        Assert.Single(receipts);
        Assert.Equal(-250.00m, receipts[0].Amount);
        Assert.Equal("WLT-1-R", receipts[0].Reference);
    }

    [Fact(DisplayName = "three payment kinds and three providers are six classes, not nine")]
    public void MPlusNNotMTimesN()
    {
        var kinds = TypeCensus.ConcreteImplementationsOf(PaymentDeskNs, typeof(Payment));
        var providers = TypeCensus.ConcreteImplementationsOf(PaymentDeskNs, typeof(IPaymentProvider));

        Assert.Equal(3, kinds);
        Assert.Equal(3, providers);
        Assert.Equal(6, kinds + providers);
        Assert.Equal(9, kinds * providers);

        Assert.Equal(typeof(IPaymentProvider),
            TypeCensus.Field(typeof(Payment), "Provider").FieldType);
    }

    [Fact(DisplayName = "an installment plan of one is a mistake, and says so")]
    public void TwoInstallmentsMinimum() =>
        Assert.Throws<ArgumentException>(() => new InstallmentPlan(new Wallet(), 1));
}
