namespace dev.kaldiroglu.Mediator.Tests.Chat.Solution;

// These directives are inside the namespace, so that Member and Main mean the room's
// classes and not anything found further out.
using dev.kaldiroglu.Mediator.Chat.Solution;
using Xunit;
using static dev.kaldiroglu.Mediator.Tests.Fields;
using static dev.kaldiroglu.Mediator.Tests.Printed;

/// <summary>The chat room as the mediator. Every figure on the Part 3 slides is asserted here.</summary>
public class SolutionTest
{
    private static readonly Type[] Participants = [typeof(IParticipant), typeof(Member), typeof(Guest)];

    /// <summary>The room delivers Elif's private message to Mert only, and the guest shows nothing.</summary>
    [Fact]
    public void TheRoomDeliversToMertOnly()
    {
        ChatRoom room = new ChatRoom();
        Member elif = new Member("Elif", room);
        _ = new Member("Burak", room);
        Member mert = new Member("Mert", room);
        Guest can = new Guest("Can", room);
        elif.Whisper("Mert", "Your review is late.");
        Assert.Equal(new[] { "Mert" }, room.Deliveries);
        Assert.Equal(new[] { "Elif (private): Your review is late." }, mert.Shown);
        Assert.Equal(Array.Empty<string>(), can.Shown);
    }

    /// <summary>Mert blocks Burak: Burak's message to everyone goes to Elif and Can, not to Mert.</summary>
    [Fact]
    public void ABlockIsKeptByTheRoom()
    {
        ChatRoom room = new ChatRoom();
        Member elif = new Member("Elif", room);
        Member burak = new Member("Burak", room);
        Member mert = new Member("Mert", room);
        Guest can = new Guest("Can", room);
        mert.Block("Burak");
        burak.Say("Lunch at one?");
        burak.Whisper("Mert", "Are you there?");
        Assert.Equal(new[] { "Elif", "Can" }, room.Deliveries);
        Assert.Equal(new[] { "Burak: Lunch at one?" }, elif.Shown);
        Assert.Equal(new[] { "Burak: Lunch at one?" }, can.Shown);
        Assert.Equal(Array.Empty<string>(), mert.Shown);
        // The sender does not receive its own message.
        Assert.Equal(Array.Empty<string>(), burak.Shown);
    }

    /// <summary>No participant holds a reference to another participant; a member holds one, to the room.</summary>
    [Fact]
    public void EveryoneHoldsTheRoom()
    {
        Assert.False(HoldsAny(typeof(Member), Participants));
        Assert.False(HoldsAny(typeof(Guest), Participants));
        Assert.Equal(1, HeldBy(typeof(Member)).Count(t => t == typeof(ChatRoom)));
    }

    /// <summary>Main runs stage one, stage three and the room, and prints the figures on the slides.</summary>
    [Fact]
    public void MainOutput()
    {
        Assert.Equal(new[]
        {
            "Stage one: 4 members hold 12 references to each other.",
            "Stage three: Elif sends Mert a private message.",
            "  delivered to: [Burak, Mert, Can]",
            "  Mert shows:   [Elif (private): Your review is late.]",
            "  Burak shows:  []",
            "  Can shows:    [Elif: Your review is late.]",
            "Chat room: Elif sends Mert a private message.",
            "  delivered to: [Mert]",
            "  Mert shows:   [Elif (private): Your review is late.]",
            "  Can shows:    []",
            "Mert blocks Burak; Burak says something to everyone.",
            "  Elif shows:   [Burak: Lunch at one?]",
            "  Mert shows:   [Elif (private): Your review is late.]",
            "  Can shows:    [Burak: Lunch at one?]"
        }, By(Main.Run));
    }
}
