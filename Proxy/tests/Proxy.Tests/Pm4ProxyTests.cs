using dev.kaldiroglu.Proxy.Pm.Pm4;
using Xunit;

namespace dev.kaldiroglu.Proxy.Tests;

public class Pm4ProxyTests
{
    [Fact]
    public void CreatedOnTheFirstForwardedCall()
    {
        var proxy = new ProxyPM();
        var citizen = new Citizen("Bora", proxy);
        Assert.False(proxy.RealPMCreated);

        citizen.TellProblem("my cousin needs a job at the ministry");
        citizen.AskForJob();
        Assert.False(proxy.RealPMCreated);

        citizen.TellProblem("the bridge on the coast road is closed");
        Assert.True(proxy.RealPMCreated);
        Assert.Equal(2, proxy.CallsScreened);
        Assert.Equal(1, proxy.CallsPassedOn);
        Assert.Equal(1, proxy.CallsRefused);
    }

    [Fact]
    public void TheClientKnowsOnlyTheInterface()
    {
        var constructor = Assert.Single(typeof(Citizen).GetConstructors());
        Assert.Equal(new[] { typeof(string), typeof(IPM) }, constructor.GetParameters().Select(p => p.ParameterType));
    }

    [Fact]
    public void StageThreeLimits()
    {
        var citizen = typeof(dev.kaldiroglu.Proxy.Pm.Pm3.Citizen);
        var constructor = Assert.Single(citizen.GetConstructors());
        Assert.Equal(typeof(dev.kaldiroglu.Proxy.Pm.Pm3.PMSecretary), constructor.GetParameters()[1].ParameterType);
        Assert.True(typeof(dev.kaldiroglu.Proxy.Pm.Pm3.RealPM).IsPublic);
    }

    [Fact]
    public void TheRealSubjectIsNotPublic()
    {
        var real = typeof(ProxyPM).Assembly.GetType("dev.kaldiroglu.Proxy.Pm.Pm4.RealPM")!;
        Assert.False(real.IsPublic);
    }
}
