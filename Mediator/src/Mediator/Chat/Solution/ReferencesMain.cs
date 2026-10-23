using System.Globalization;

namespace dev.kaldiroglu.Mediator.Chat.Solution;

/// <summary>
/// Counts the references of stage one: four members who all hold each other.
/// <para>
/// In the chat room, a member holds one reference, to the room.
/// </para>
/// </summary>
/// <remarks>
/// Run it with
/// <c>dotnet run --project src/Mediator.Demo -- chat-references</c>.
/// </remarks>
public static class ReferencesMain
{
    public static void Run()
    {
        // Inside this namespace the plain name Member means the room's member, so stage one's
        // member is written Problem.Direct.Member.
        Problem.Direct.Member elif = new("Elif"), burak = new("Burak"), mert = new("Mert"),
                selin = new("Selin");
        List<Problem.Direct.Member> team = [elif, burak, mert, selin];
        for (int i = 0; i < team.Count; i++)
        {
            for (int j = i + 1; j < team.Count; j++)
            {
                team[i].Meet(team[j]);
            }
        }
        int references = team.Sum(m => m.References);
        Console.WriteLine("Stage one: " + Number(team.Count) + " members hold " + Number(references)
                + " references to each other.");
    }

    private static string Number(int value) => value.ToString(CultureInfo.InvariantCulture);
}
