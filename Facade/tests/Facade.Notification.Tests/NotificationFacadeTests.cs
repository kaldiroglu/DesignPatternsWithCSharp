using dev.kaldiroglu.Facade.Notification.Solution;
using Xunit;

namespace dev.kaldiroglu.Facade.Notification.Tests;

/// <summary>Verifies the NotificationFacade over the notification subsystem.</summary>
public class NotificationFacadeTests
{
    private static NotificationFacade NewFacade() => new(new NotificationConfig(
        "smtp", 587, "u", "p", "sid", "tok", "+1", "hook", "/fb", "/log"));

    [Fact]
    public void FullyConfiguredUserReachesAllFourChannels()
    {
        var facade = NewFacade();
        var user = new User("u1")
        {
            Email = "a@b.com", Phone = "+1555", SlackChannel = "#c", DeviceToken = "tok123456",
        };

        NotificationResult result = facade.Notify(user, "Title", "Body");

        Assert.True(result.AllSucceeded);
        Assert.Equal(new[] { "email", "sms", "slack", "push" }.OrderBy(x => x),
            result.Results.Keys.OrderBy(x => x));
    }

    [Fact]
    public void ChannelsWithNoUserFieldAreSkipped()
    {
        var facade = NewFacade();
        var user = new User("u1") { Email = "a@b.com" }; // only email

        NotificationResult result = facade.Notify(user, "Title", "Body");

        Assert.Equal(new[] { "email" }, result.Results.Keys);
    }

    [Fact]
    public void SmsIsSkippedWhenBodyExceeds160Chars()
    {
        var facade = NewFacade();
        var user = new User("u1") { Phone = "+1555" };
        string longBody = new string('x', 161);

        NotificationResult result = facade.Notify(user, "Title", longBody);

        Assert.DoesNotContain("sms", result.Results.Keys);
    }

    [Fact]
    public void SlackIsSkippedWhenDoNotDisturb()
    {
        var facade = NewFacade();
        var user = new User("u1") { SlackChannel = "#c", DoNotDisturb = true };

        NotificationResult result = facade.Notify(user, "Title", "Body");

        Assert.DoesNotContain("slack", result.Results.Keys);
    }

    [Fact]
    public void AllSucceededIsTrueWhenNoChannelsAttempted()
    {
        var facade = NewFacade();
        var user = new User("u1"); // no channels configured

        NotificationResult result = facade.Notify(user, "Title", "Body");

        Assert.Empty(result.Results);
        Assert.True(result.AllSucceeded);
    }
}
