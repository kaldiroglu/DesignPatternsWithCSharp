namespace dev.kaldiroglu.Mediator.Tests.Chat.Problem.Bus;

using dev.kaldiroglu.Mediator.Chat.Problem.Bus;
using Xunit;
using static dev.kaldiroglu.Mediator.Tests.Fields;

/// <summary>Stage three: a message bus, and clients that decide what to show.</summary>
public class BusTest
{
    /// <summary>The bus delivers Elif's private message to Burak, Mert and Can, and the guest client shows it.</summary>
    [Fact]
    public void TheGuestShowsThePrivateMessage()
    {
        MessageBus bus = new MessageBus();
        ChatClient elif = new ChatClient("Elif"), burak = new ChatClient("Burak"), mert = new ChatClient("Mert");
        GuestClient can = new GuestClient("Can");
        foreach (ChatClient client in new[] { elif, burak, mert })
        {
            bus.Subscribe(client);
        }
        bus.Subscribe(can);
        bus.Publish(new Message("Elif", "Mert", "Your review is late."));
        Assert.Equal(new[] { "Burak", "Mert", "Can" }, bus.Deliveries);
        Assert.Equal(3, bus.Deliveries.Count);
        Assert.Equal(new[] { "Elif (private): Your review is late." }, mert.Shown);
        Assert.Equal(Array.Empty<string>(), burak.Shown);                     // a team client filters
        Assert.Equal(new[] { "Elif: Your review is late." }, can.Shown);      // the guest client does not
        Assert.Empty(elif.Shown);                                             // the sender is not delivered to
    }

    /// <summary>A message to everyone is shown by every client except the sender.</summary>
    [Fact]
    public void AMessageToEveryone()
    {
        MessageBus bus = new MessageBus();
        ChatClient elif = new ChatClient("Elif"), burak = new ChatClient("Burak");
        bus.Subscribe(elif);
        bus.Subscribe(burak);
        bus.Publish(new Message("Burak", null, "Lunch at one?"));
        Assert.Equal(new[] { "Burak: Lunch at one?" }, elif.Shown);
        Assert.Empty(burak.Shown);
    }

    /// <summary>No client holds a reference to another client.</summary>
    [Fact]
    public void NoClientKnowsAnother()
    {
        Type[] clients = [typeof(IClient), typeof(ChatClient), typeof(GuestClient)];
        Assert.False(HoldsAny(typeof(ChatClient), clients));
        Assert.False(HoldsAny(typeof(GuestClient), clients));
    }
}
