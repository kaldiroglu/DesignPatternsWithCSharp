namespace dev.kaldiroglu.Bridge.Notifications.Domain;

/// <summary>What we want to say. Nothing here knows how it will be delivered.</summary>
public record Message(string Subject, string Body);
