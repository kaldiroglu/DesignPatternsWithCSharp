namespace dev.kaldiroglu.CreationalIntroduction.Bank;

/// <summary>
/// A constructor body runs after the base constructor, and no statement can come before
/// <c>base(...)</c>. The argument of <c>base(...)</c> is evaluated first, so the check sits
/// there, as a <c>throw</c> expression, and <see cref="Term"/> never runs for a bad value.
/// </summary>
public class ShortTerm : Term
{
    public ShortTerm(int months)
        : base(months > 12 ? throw new ArgumentException("A short term is at most 12 months") : months)
    {
    }
}
