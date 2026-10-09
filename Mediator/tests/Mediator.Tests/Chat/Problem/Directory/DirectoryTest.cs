namespace dev.kaldiroglu.Mediator.Tests.Chat.Problem.Directory;

// These directives are inside the namespace, so that Directory means the example's class
// and not System.IO.Directory, which the implicit usings bring in.
using dev.kaldiroglu.Mediator.Chat.Problem.Directory;
using Xunit;
using static dev.kaldiroglu.Mediator.Tests.Printed;

/// <summary>Stage two: the members share one directory, and every sender applies the rules.</summary>
public class DirectoryTest
{
    /// <summary>A new member is added once, to the directory, and everyone can reach them.</summary>
    [Fact]
    public void ANewMemberIsAddedOnce()
    {
        Directory directory = new Directory();
        Member elif = new Member("Elif", directory);
        Member burak = new Member("Burak", directory);
        Member can = new Member("Can", directory);
        burak.Say("Lunch at one?");
        Assert.Equal(3, directory.Members.Count);
        Assert.Equal(new[] { "Burak: Lunch at one?" }, elif.Inbox);
        Assert.Equal(new[] { "Burak: Lunch at one?" }, can.Inbox);
        Assert.Empty(burak.Inbox);   // the sender skips itself
    }

    /// <summary>The sender skips anyone who blocked it, for messages to everyone and for private ones.</summary>
    [Fact]
    public void TheSenderAppliesTheBlock()
    {
        Directory directory = new Directory();
        Member elif = new Member("Elif", directory);
        Member burak = new Member("Burak", directory);
        Member mert = new Member("Mert", directory);
        mert.Block("Burak");
        burak.Say("Lunch at one?");
        burak.Whisper("Mert", "Are you there?");
        elif.Whisper("Mert", "Your review is late.");
        Assert.Equal(new[] { "Burak: Lunch at one?" }, elif.Inbox);
        Assert.Equal(new[] { "Elif (private): Your review is late." }, mert.Inbox);
    }

    /// <summary>The block rule is written in the sending code, once for Say and once for Whisper.</summary>
    [Fact]
    public void TheRulesAreInTheSender()
    {
        string member = CodeOf("Chat/Problem/Directory/Member.cs");
        Assert.Equal(2, CountOf(member, "blocked.Contains(Name)"));
        Assert.Equal(0, CountOf(CodeOf("Chat/Problem/Directory/Directory.cs"), "blocked"));
    }
}
