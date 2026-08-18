namespace dev.kaldiroglu.Bridge.Notifications.Domain;

/// <summary>What actually happened when we tried to reach somebody.</summary>
public record DeliveryResult(bool Delivered, string Channel, string Address, string BodySent,
    int Attempts)
{
    public bool Truncated(string originalBody) => BodySent.Length < originalBody.Length;
}
