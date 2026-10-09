namespace dev.kaldiroglu.Mediator.Tests.Hw.AirTraffic;

using System.Reflection;
using System.Text.RegularExpressions;
using dev.kaldiroglu.Mediator.Hw.AirTraffic;
using Xunit;
using static dev.kaldiroglu.Mediator.Tests.Printed;

/// <summary>Homework 2: the control tower gives the runway to one aircraft at a time.</summary>
public class ControlTowerTest
{
    /// <summary>One aircraft on the runway at a time; the next one waiting gets it when it is clear.</summary>
    [Fact]
    public void OneAtATime()
    {
        ControlTower tower = new ControlTower();
        Aircraft tk1 = new Aircraft("TK1", "land", tower);
        Aircraft pc2 = new Aircraft("PC2", "take off", tower);
        Aircraft lh3 = new Aircraft("LH3", "land", tower);
        tk1.Request();
        pc2.Request();
        lh3.Request();
        tk1.Clear();
        pc2.Clear();
        Assert.Equal(new[]
        {
            "TK1 may land",
            "PC2 waits",
            "LH3 waits",
            "TK1 clears the runway",
            "PC2 may take off",
            "PC2 clears the runway",
            "LH3 may land"
        }, tower.Log);
        // TK1 is not on the runway.
        Assert.Throws<InvalidOperationException>(tk1.Clear);
    }

    /// <summary>
    /// Every public method of the tower takes the lock. The Java checks the synchronized
    /// modifier. The C# tower uses a lock statement on a private object, which leaves no mark
    /// on the method, so this test reads the source: the body of each public member has a lock.
    /// </summary>
    [Fact]
    public void EveryMethodIsSynchronized()
    {
        List<MethodInfo> methods = typeof(ControlTower)
            .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .ToList();
        Assert.Equal(3, methods.Count);   // Request, RunwayClear and the getter of Log

        // Each part of the source that starts with "public " runs to the next one.
        string code = CodeOf("Hw/AirTraffic/ControlTower.cs");
        string[] members = code.Split("public ");
        foreach (MethodInfo method in methods)
        {
            string name = method.Name.StartsWith("get_", StringComparison.Ordinal) ? method.Name[4..] : method.Name;
            string body = Assert.Single(members, m => Regex.IsMatch(m, @"^\S+\s+" + name + @"\b"));
            Assert.True(body.Contains("lock (", StringComparison.Ordinal), name);
        }
    }
}
