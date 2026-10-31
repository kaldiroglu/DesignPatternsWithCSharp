namespace dev.kaldiroglu.Proxy.Pm.Pm3;

/// <summary>Stage 3: the citizen holds an interface, and every call is screened.</summary>
public static class Pm3Demo
{
    /// <summary>The secretary creates the Prime Minister when the first citizen is built, before any call.</summary>
    public static void Secretary()
    {
        var secretary = new PMSecretary();
        Console.WriteLine("Prime Minister created yet? " + secretary.HasBeenAskedForThePM);

        _ = new Citizen("Ayse", secretary);
        Console.WriteLine("Prime Minister created yet? " + secretary.HasBeenAskedForThePM);
    }

    /// <summary>Two problems and a request for a job, through the proxy.</summary>
    public static void Screening()
    {
        var secretary = new PMSecretary();
        var ayse = new Citizen("Ayse", secretary);
        var bora = new Citizen("Bora", secretary);

        ayse.TellProblem("the bridge on the coast road is closed");
        Console.WriteLine();
        bora.TellProblem("my cousin needs a job at the ministry");
        Console.WriteLine();
        bora.AskForJob();

        var proxy = (ProxyPM)secretary.GetPM();
        Console.WriteLine();
        Console.WriteLine($"calls screened : {proxy.CallsScreened}");
        Console.WriteLine($"passed on      : {proxy.CallsPassedOn}");
        Console.WriteLine($"refused        : {proxy.CallsRefused}");
        Console.WriteLine("""

            Citizen holds IPM, which is an interface. Citizen does not know that
            its calls are screened, and RealPM screens nothing.

            This is a protection proxy: one call was refused, and FindJob never
            reached her.

            It is not yet a virtual proxy. The Prime Minister was created when
            the first citizen was built, before any call.
            """);
    }
}
