namespace dev.kaldiroglu.Introduction.Proxy.Pm.Pm3;

/// <summary>
/// Stage 3 — the solution. The citizen holds <see cref="IPM"/>, an interface, and every call
/// is screened.
/// </summary>
/// <remarks>
/// Run it with <c>dotnet run --project src/Introduction.Demo -- pm3</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        var secretary = new PMSecretary();
        Console.WriteLine("Prime Minister created yet? " + Lower(secretary.HasBeenAskedForThePM()));

        var ayse = new Citizen("Ayse", secretary);
        Console.WriteLine("Prime Minister created yet? " + Lower(secretary.HasBeenAskedForThePM()));
        Console.WriteLine();

        var bora = new Citizen("Bora", secretary);

        ayse.TellProblem("the bridge on the coast road is closed");
        Console.WriteLine();
        bora.TellProblem("my cousin needs a job at the ministry");
        Console.WriteLine();
        bora.AskForJob();

        var proxy = (ProxyPM)secretary.GetPM();
        Console.WriteLine();
        Console.WriteLine($"calls screened : {proxy.CallsScreened()}");
        Console.WriteLine($"passed on      : {proxy.CallsPassedOn()}");
        Console.WriteLine($"refused        : {proxy.CallsRefused()}");
        Console.WriteLine("""

            Citizen holds IPM, an interface. In stage 1 it held PM, a class.
            Citizen does not know that its calls are screened, and RealPM
            screens nothing.

            Two proxy kinds are at work in this one object:
              protection — one call was refused, and FindJob never reached her
              virtual    — the Prime Minister did not exist until asked for
            """);
    }

    private static string Lower(bool value) => value ? "true" : "false";
}
