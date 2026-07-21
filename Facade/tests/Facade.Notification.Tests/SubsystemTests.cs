using dev.kaldiroglu.Facade.Notification.Subsystems;
using Xunit;

namespace dev.kaldiroglu.Facade.Notification.Tests;

/// <summary>The subsystem classes are usable directly, without the facade.</summary>
public class SubsystemTests
{
    [Fact]
    public void EmailServiceCanBeUsedWithoutTheFacade()
    {
        var email = new EmailService("smtp", 587, "u", "p");
        Assert.True(email.Send("a@b.com", "Subject", "Body"));
    }

    [Fact]
    public void SmsServiceCanBeUsedWithoutTheFacade()
    {
        var sms = new SmsService("sid", "tok", "+1");
        Assert.True(sms.Send("+15551234567", "hi"));
    }

    [Fact]
    public void PushServiceMasksTheDeviceToken()
    {
        var push = new PushNotificationService("/fb");
        // No exception even for a short token (masking must not overflow).
        Assert.True(push.Send("abc", "Title", "Body"));
    }
}
