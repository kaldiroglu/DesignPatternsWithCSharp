using System.Reflection;
using Xunit;
using Pm1 = dev.kaldiroglu.Introduction.Proxy.Pm.Pm1;
using Pm2 = dev.kaldiroglu.Introduction.Proxy.Pm.Pm2;
using Pm3 = dev.kaldiroglu.Introduction.Proxy.Pm.Pm3;

namespace dev.kaldiroglu.Introduction.Tests.Proxy.Pm;

/// <summary>
/// Three stages, and the tests measure what changes between them: where the screening lives,
/// what type the client holds, and whether the real subject is reached.
/// </summary>
public class PrimeMinisterTests
{
    private const string Real = "the bridge on the coast road is closed";
    private const string Favor = "my cousin needs a job at the ministry";

    private static FieldInfo Field(Type type, string name) =>
        type.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic)
        ?? throw new InvalidOperationException($"{type.Name} has no field {name}");

    [Fact(DisplayName = "stage 1 — the PM screens his own calls, and hears every one of them")]
    public void StageOne()
    {
        var pm = new Pm1.PM();
        var citizen = new Pm1.Citizen("Ayse", pm);

        citizen.TellProblem(Real);
        citizen.TellProblem(Favor);

        Assert.Equal(2, pm.ProblemsHeard());      // both calls reached him
        Assert.Equal(1, pm.ProblemsResolved());   // and he screened them himself
    }

    [Fact(DisplayName = "stage 2 — the screening moved, but the client had to be rewritten")]
    public void StageTwo()
    {
        var pm = new Pm2.PM();
        var proxy = new Pm2.Proxy(pm);
        var citizen = new Pm2.Citizen("Bora", proxy);

        citizen.TellProblem(Real);
        citizen.TellProblem(Favor);

        Assert.Equal(2, proxy.CallsScreened());
        Assert.Equal(1, proxy.CallsPassedOn());
        Assert.Equal(1, pm.ProblemsHeard());      // one call never reached him

        // The defect of this stage, in the type system: the two classes are unrelated, so
        // Citizen's field had to change from PM to Proxy.
        Assert.False(typeof(Pm2.PM).IsAssignableFrom(typeof(Pm2.Proxy)));   // no shared base type
        Assert.Empty(typeof(Pm2.Proxy).GetInterfaces());
        Assert.Same(typeof(Pm2.Proxy), Field(typeof(Pm2.Citizen), "proxy").FieldType);
    }

    [Fact(DisplayName = "stage 3 — one interface, and the client is back to holding a PM")]
    public void StageThree()
    {
        var secretary = new Pm3.PMSecretary();
        var citizen = new Pm3.Citizen("Ayse", secretary);

        citizen.TellProblem(Real);
        citizen.TellProblem(Favor);
        citizen.AskForJob();

        var proxy = (Pm3.ProxyPM)secretary.GetPM();
        Assert.Equal(2, proxy.CallsScreened());
        Assert.Equal(1, proxy.CallsPassedOn());
        Assert.Equal(1, proxy.CallsRefused());

        // The fix, in the type system: the client's field is the Subject type again.
        Assert.Same(typeof(Pm3.IPM), Field(typeof(Pm3.Citizen), "pm").FieldType);
    }

    [Fact(DisplayName = "the proxy is substitutable and the real subject is unreachable")]
    public void Substitutable()
    {
        var secretary = new Pm3.PMSecretary();

        Assert.True(typeof(Pm3.IPM).IsAssignableFrom(typeof(Pm3.ProxyPM)));
        Assert.True(typeof(Pm3.IPM).IsAssignableFrom(typeof(Pm3.RealPM)));

        // The secretary hands out the proxy, never the real subject.
        Assert.IsType<Pm3.ProxyPM>(secretary.GetPM());
    }

    [Fact(DisplayName = "the Prime Minister is not created until somebody asks — a virtual proxy too")]
    public void CreatedLazily()
    {
        var secretary = new Pm3.PMSecretary();

        Assert.False(secretary.HasBeenAskedForThePM());
        secretary.GetPM();
        Assert.True(secretary.HasBeenAskedForThePM());
        Assert.Same(secretary.GetPM(), secretary.GetPM());   // and only once
    }

    [Fact(DisplayName = "a proxy may answer entirely by itself — FindJob never reaches the PM")]
    public void AnsweredWithoutForwarding()
    {
        var real = new Pm3.RealPM();
        var proxy = new Pm3.ProxyPM(real);

        proxy.FindJob("Bora");

        // No decorator would do this: the subject was not consulted at all.
        Assert.Equal(0, real.ProblemsHeard());
        Assert.Equal(0, proxy.CallsPassedOn());
    }

    [Fact(DisplayName = "a proxy must stand in front of something")]
    public void NullSubjectIsRejected()
    {
        Assert.Throws<ArgumentNullException>(() => new Pm3.ProxyPM(null!));
    }
}
