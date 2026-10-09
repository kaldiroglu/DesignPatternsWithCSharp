namespace dev.kaldiroglu.Mediator.Tests.Chat.Problem.Direct;

using dev.kaldiroglu.Mediator.Chat.Problem.Direct;
using Xunit;

/// <summary>Stage one: every member holds every other member.</summary>
public class DirectTest
{
    private static List<Member> FourWhoHaveMet()
    {
        List<Member> team = [new Member("Elif"), new Member("Burak"), new Member("Mert"), new Member("Selin")];
        for (int i = 0; i < team.Count; i++)
        {
            for (int j = i + 1; j < team.Count; j++)
            {
                team[i].Meet(team[j]);
            }
        }
        return team;
    }

    /// <summary>Four members hold twelve references to each other: six lines, held from both ends.</summary>
    [Fact]
    public void TwelveReferences()
    {
        List<Member> team = FourWhoHaveMet();
        Assert.Equal(4, team.Count);
        Assert.Equal(12, team.Sum(m => m.References));
        Assert.All(team, m => Assert.Equal(3, m.References));
    }

    /// <summary>It works: a private message goes straight to its receiver and to nobody else.</summary>
    [Fact]
    public void APrivateMessageReachesOnlyItsReceiver()
    {
        List<Member> team = FourWhoHaveMet();
        Member elif = team[0], burak = team[1], mert = team[2], selin = team[3];
        elif.Whisper(mert, "Your review is late.");
        Assert.Equal(new[] { "Elif (private): Your review is late." }, mert.Inbox);
        Assert.Empty(burak.Inbox);
        Assert.Empty(selin.Inbox);
    }

    /// <summary>A fifth member must be added to four lists; a member who was not told never sends to them.</summary>
    [Fact]
    public void AFifthMember()
    {
        List<Member> team = FourWhoHaveMet();
        Member can = new Member("Can");
        team[0].Meet(can);                  // only Elif is told
        team[1].Say("Lunch at one?");       // Burak does not know Can
        Assert.Empty(can.Inbox);
        team[0].Say("Hello");
        Assert.Equal(new[] { "Elif: Hello" }, can.Inbox);
        // One new line, held from both ends; three more lines are still missing.
        Assert.Equal(14, team.Sum(m => m.References) + can.References);
    }
}
