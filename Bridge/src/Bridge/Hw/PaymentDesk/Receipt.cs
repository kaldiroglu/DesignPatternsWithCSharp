namespace dev.kaldiroglu.Bridge.Hw.PaymentDesk;

/// <summary>Proof that money moved.</summary>
public record Receipt(string Reference, decimal Amount, string Provider);
