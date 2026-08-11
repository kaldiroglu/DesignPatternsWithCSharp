using dev.kaldiroglu.Decorator.Hw.FeeEngine;
using Xunit;

namespace dev.kaldiroglu.Decorator.Tests.Hw;

/// <summary>
/// Homework 2. There is a right answer and it comes from tax law, not taste: the discount
/// reduces the taxable base, so VAT goes outside the voucher.
/// </summary>
public class FeeEngineTests
{
    private static ICharge Subtotal() =>
        new TransactionFee(new PlatformFee(new BasketTotal("100.00"), "8"), "2.50");

    [Fact(DisplayName = "the subtotal is the basket plus both fees")]
    public void Subtotals()
    {
        Assert.Equal(100.00m, new BasketTotal("100.00").Amount());
        Assert.Equal(110.50m, Subtotal().Amount()); // 100 + 8% + 2.50
    }

    [Fact(DisplayName = "a voucher is a subtraction, so VAT's position changes the tax")]
    public void AVoucherDoesNotCommuteWithVat()
    {
        var lawful = new ValueAddedTax(new Voucher(Subtotal(), "10.00"), "20");
        var unlawful = new Voucher(new ValueAddedTax(Subtotal(), "20"), "10.00");

        Assert.Equal(120.60m, lawful.Amount());   // VAT on 100.50
        Assert.Equal(122.60m, unlawful.Amount()); // VAT on 110.50, then 10 off
        Assert.Equal(2.00m, unlawful.Amount() - lawful.Amount()); // the VAT on the voucher
    }

    [Fact(DisplayName = "a percentage discount is a multiplication, so its position changes nothing")]
    public void APercentageDiscountCommutes()
    {
        var outside = new ValueAddedTax(new PromotionalDiscount(Subtotal(), "10"), "20");
        var inside = new PromotionalDiscount(new ValueAddedTax(Subtotal(), "20"), "10");

        Assert.Equal(outside.Amount(), inside.Amount());
    }

    [Fact(DisplayName = "a voucher never takes a charge below zero")]
    public void AVoucherIsClamped() =>
        Assert.Equal(0m, new Voucher(new BasketTotal("5.00"), "10.00").Amount());
}
