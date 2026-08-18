namespace dev.kaldiroglu.Bridge.Hw.PaymentDesk;

/// <summary>
/// A hold placed on money that has not moved yet.
/// </summary>
/// <param name="Reference">What the provider calls it.</param>
/// <param name="Amount">The amount held.</param>
/// <param name="Settled">True when the money has already moved, so capture has nothing left to do.</param>
public record Authorization(string Reference, decimal Amount, bool Settled);
