using System.Reflection;
using Xunit;
using Pm1 = dev.kaldiroglu.Proxy.Pm.Pm1;
using Pm2 = dev.kaldiroglu.Proxy.Pm.Pm2;
using Pm3 = dev.kaldiroglu.Proxy.Pm.Pm3;

namespace dev.kaldiroglu.Proxy.Tests;

/// <summary>
/// Three stages. The tests measure what changes between them: where the screening lives, what
/// type the client holds, and whether the real subject is reached.
/// </summary>
public class PmProxyTests
{
    private const string Real = "the bridge on the coast road is closed";
    private const string Favor = "my cousin needs a job at the ministry";

    [Fact(DisplayName = "Stage 1: the PM screens her own calls and hears every one of them")]
    public void StageOne()
    {
        var pm = new Pm1.PM();
        var citizen = new Pm1.Citizen("Ayse", pm);

        citizen.TellProblem(Real);
        citizen.TellProblem(Favor);

        Assert.Equal(2, pm.ProblemsHeard);
        Assert.Equal(1, pm.ProblemsResolved);
    }

    [Fact(DisplayName = "Stage 2: the screening moved, but the client had to be rewritten")]
    public void StageTwo()
    {
        var pm = new Pm2.PM();
        var proxy = new Pm2.PmProxy(pm);
        var citizen = new Pm2.Citizen("Bora", proxy);

        citizen.TellProblem(Real);
        citizen.TellProblem(Favor);

        Assert.Equal(2, proxy.CallsScreened);
        Assert.Equal(1, proxy.CallsPassedOn);
        Assert.Equal(1, pm.ProblemsHeard);

        Assert.False(typeof(Pm2.PM).IsAssignableFrom(typeof(Pm2.PmProxy)));
        Assert.Empty(typeof(Pm2.PmProxy).GetInterfaces());
        var field = typeof(Pm2.Citizen).GetField("_proxy", BindingFlags.NonPublic | BindingFlags.Instance)!;
        Assert.Equal(typeof(Pm2.PmProxy), field.FieldType);
    }

    [Fact(DisplayName = "Stage 3: one interface, and the client holds the interface")]
    public void StageThree()
    {
        var secretary = new Pm3.PMSecretary();
        var citizen = new Pm3.Citizen("Ayse", secretary);

        citizen.TellProblem(Real);
        citizen.TellProblem(Favor);
        citizen.AskForJob();

        var proxy = (Pm3.ProxyPM)secretary.GetPM();
        Assert.Equal(2, proxy.CallsScreened);
        Assert.Equal(1, proxy.CallsPassedOn);
        Assert.Equal(1, proxy.CallsRefused);

        var field = typeof(Pm3.Citizen).GetField("_pm", BindingFlags.NonPublic | BindingFlags.Instance)!;
        Assert.Equal(typeof(Pm3.IPM), field.FieldType);
    }

    [Fact(DisplayName = "The proxy can be substituted, and the secretary hands out the proxy")]
    public void Substitutable()
    {
        Assert.True(typeof(Pm3.IPM).IsAssignableFrom(typeof(Pm3.ProxyPM)));
        Assert.True(typeof(Pm3.IPM).IsAssignableFrom(typeof(Pm3.RealPM)));
        Assert.IsType<Pm3.ProxyPM>(new Pm3.PMSecretary().GetPM());
    }

    [Fact(DisplayName = "The secretary creates the Prime Minister on the first GetPM, and only once")]
    public void CreatedOnce()
    {
        var secretary = new Pm3.PMSecretary();

        Assert.False(secretary.HasBeenAskedForThePM);
        secretary.GetPM();
        Assert.True(secretary.HasBeenAskedForThePM);
        Assert.Same(secretary.GetPM(), secretary.GetPM());
    }

    [Fact(DisplayName = "The first citizen creates the Prime Minister, before any call")]
    public void TheFirstCitizenCreatesThePM()
    {
        var secretary = new Pm3.PMSecretary();

        _ = new Pm3.Citizen("Ayse", secretary);

        Assert.True(secretary.HasBeenAskedForThePM);
    }

    [Fact(DisplayName = "A proxy may answer by itself: FindJob never reaches the PM")]
    public void AnsweredWithoutForwarding()
    {
        var real = new Pm3.RealPM();
        var proxy = new Pm3.ProxyPM(real);

        proxy.FindJob("Bora");

        Assert.Equal(0, real.ProblemsHeard);
        Assert.Equal(0, proxy.CallsPassedOn);
    }

    [Fact(DisplayName = "A proxy must stand in front of something")]
    public void NullSubjectIsRejected()
    {
        Assert.Throws<ArgumentNullException>(() => new Pm3.ProxyPM(null!));
    }

    [Fact(DisplayName = "The screening demo prints the counters and does not claim a virtual proxy")]
    public void ScreeningDemoPrintsTheFigures()
    {
        string printed = CaptureConsole(Pm3.Pm3Demo.Screening);

        Assert.Contains("calls screened : 2", printed);
        Assert.Contains("passed on      : 1", printed);
        Assert.Contains("refused        : 1", printed);
        Assert.Contains("before any call", printed);
        Assert.DoesNotContain("did not exist", printed);
    }

    [Fact(DisplayName = "The secretary demo prints False, then True")]
    public void SecretaryDemoPrintsTheFlag()
    {
        string printed = CaptureConsole(Pm3.Pm3Demo.Secretary);

        Assert.Contains("Prime Minister created yet? False", printed);
        Assert.Contains("Prime Minister created yet? True", printed);
    }

    private static string CaptureConsole(Action action)
    {
        TextWriter original = Console.Out;
        var buffer = new StringWriter();
        Console.SetOut(buffer);
        try
        {
            action();
        }
        finally
        {
            Console.SetOut(original);
        }

        return buffer.ToString();
    }
}
