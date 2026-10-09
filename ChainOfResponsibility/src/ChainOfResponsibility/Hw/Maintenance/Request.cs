namespace dev.kaldiroglu.ChainOfResponsibility.Hw.Maintenance;

/// <summary>A maintenance request: its kind, a title, and the estimated effort in days.</summary>
public sealed record Request(RequestKind Kind, string Title, int Days);
