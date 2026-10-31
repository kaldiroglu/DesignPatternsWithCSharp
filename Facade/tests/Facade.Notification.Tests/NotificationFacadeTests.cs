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
    public void AnSmsOver160CharsIsReportedAsAFailure()
    {
        var facade = NewFacade();
        var user = new User("u1") { Phone = "+1555" };
        string longBody = new string('x', 161);

        NotificationResult result = facade.Notify(user, "Title", longBody);

        Assert.False(result.Results["sms"]);
        Assert.False(result.AllSucceeded);
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

    private static Solution2.NotificationFacade NewChannelFacade() => new(new NotificationConfig(
        "smtp", 587, "u", "p", "sid", "tok", "+1", "hook", "/fb", "/log"));

    private static User Reachable() => new("u-1")
    {
        Email = "a@example.com", Phone = "+905550000", SlackChannel = "#c", DeviceToken = "tok123456",
    };

    [Fact(DisplayName = "Solution 2 also reports an SMS it could not send as a failure")]
    public void SolutionTwoReportsATooLongSms()
    {
        var texter = new User("u-3") { Phone = "+905550000" };

        var tooLong = NewChannelFacade().Notify(texter, "t", new string('x', 161));

        Assert.False(tooLong.Results["sms"]);
        Assert.False(tooLong.AllSucceeded);
    }

    [Fact(DisplayName = "Solution 2 lets the caller choose a subset of channels")]
    public void ChannelSubset()
    {
        var only = NewChannelFacade().Notify(Reachable(),
            new HashSet<Solution2.Channel> { Solution2.Channel.Email, Solution2.Channel.Slack }, "t", "b");

        Assert.Equal(2, only.Results.Count);
        Assert.True(only.Results.ContainsKey("email"));
        Assert.True(only.Results.ContainsKey("slack"));
    }

    [Fact(DisplayName = "The convenience overload still means all channels")]
    public void ConvenienceOverloadIsAllChannels()
    {
        Assert.Equal(4, NewChannelFacade().Notify(Reachable(), "t", "b").Results.Count);
    }

    [Fact(DisplayName = "Choosing a channel the user cannot receive on is not an error")]
    public void AnImpossibleChannelIsSkipped()
    {
        var emailOnly = new User("u-5") { Email = "a@example.com" };

        var result = NewChannelFacade().Notify(emailOnly,
            new HashSet<Solution2.Channel> { Solution2.Channel.Email, Solution2.Channel.Sms }, "t", "b");

        Assert.Single(result.Results);
    }
}
