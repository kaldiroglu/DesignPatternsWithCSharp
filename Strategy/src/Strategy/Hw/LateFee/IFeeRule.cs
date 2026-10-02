namespace dev.kaldiroglu.Strategy.Hw.LateFee;

/// <summary>
/// The Strategy: what a member class is charged for keeping something too long.
/// <para>
/// Homework 2. The rules below are the ones a library actually runs; the exercise adds a
/// fourth and then asks whether it belongs here at all.
/// </para>
/// </summary>
public interface IFeeRule
{
    string Name { get; }

    /// <summary>The charge in minor units. Never negative.</summary>
    int Charge(Loan loan);
}
