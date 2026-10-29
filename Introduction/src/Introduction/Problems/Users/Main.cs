namespace dev.kaldiroglu.Introduction.Problems.Users;

/// <summary>
/// Three users through <see cref="UserProcessor.ProcessUser"/>: each answer needs all seven
/// arguments, even when the decision uses only one of them.
/// </summary>
/// <remarks>
/// Run it with <c>dotnet run --project src/Introduction.Demo -- user</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        var processor = new UserProcessor();

        // A user with no data at all: age 0.
        var child = new User();

        // An adult on a trial whose days have run out.
        var trialUser = new AdultUser();

        Console.WriteLine("No user          : "
            + processor.ProcessUser(null, false, "TRIAL", 1, false, "IT", true));
        Console.WriteLine("A child          : "
            + processor.ProcessUser(child, false, "TRIAL", 1, false, "IT", true));
        Console.WriteLine("An expired trial : "
            + processor.ProcessUser(trialUser, false, "TRIAL", 1, false, "IT", true));
    }

    /// <summary>A user aged 30, with every other value left at its default.</summary>
    public class AdultUser : User
    {
        public override int Age => 30;
    }
}
