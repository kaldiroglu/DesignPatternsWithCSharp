namespace dev.kaldiroglu.TemplateMethod.Hw.CallCenter;

/// <summary>What a call center says about one call: its id and how long it lasted.</summary>
public sealed record CallRecord(string Id, int Seconds);
