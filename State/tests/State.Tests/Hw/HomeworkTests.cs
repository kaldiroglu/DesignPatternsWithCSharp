using System.Reflection;
using Xunit;
using A = global::dev.kaldiroglu.State.Hw.AirConditioner;
using D = global::dev.kaldiroglu.State.Hw.Document;
using V = global::dev.kaldiroglu.State.Hw.Vending;

namespace dev.kaldiroglu.State.Tests.Hw;

/// <summary>Homework 1: the air conditioner. The "which state now?" rule is written once.</summary>
public class AirConditionerTests
{
    [Fact]
    public void TheRoomAndTheTargetChooseTheState()
    {
        var ac = new A.AirConditioner(26);
        ac.PowerOn();
        Assert.Equal("cooling", ac.State);
        ac.RoomIs(22);
        Assert.Equal("idle", ac.State);
        ac.SetTarget(24);
        Assert.Equal("heating", ac.State);
    }

    [Fact]
    public void OffStaysOff()
    {
        var ac = new A.AirConditioner(26);
        ac.SetTarget(20);
        ac.RoomIs(30);
        Assert.Equal("off", ac.State);
        Assert.Empty(ac.Log);
    }

    /// <summary>Only the base class AcState declares Running; no running state has its own copy.</summary>
    [Fact]
    public void TheRuleIsWrittenOnce()
    {
        Type acState = typeof(A.AirConditioner).Assembly.GetType("dev.kaldiroglu.State.Hw.AirConditioner.AcState")!;
        const BindingFlags All = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static
                                 | BindingFlags.Instance | BindingFlags.DeclaredOnly;
        Assert.Contains(acState.GetMethods(All), m => m.Name == "Running");
        foreach (Type state in acState.GetNestedTypes(BindingFlags.NonPublic | BindingFlags.Public))
        {
            Assert.DoesNotContain(state.GetMethods(All), m => m.Name == "Running");
        }
    }

    [Fact]
    public void MainOutput()
    {
        Assert.Equal(new[]
        {
            "On in a 26 degree room, target 22: cooling",
            "The room is now 22:                idle",
            "The target is now 24:              heating",
            "Off, then the target is set to 20: off",
            "Log: [cooling (room 26, target 22), idle (room 22, target 22), heating (room 22, target 24), off]"
        }, Printed.By(A.Main.Run));
    }
}

/// <summary>Homework 2: the document workflow, with the transitions in one central table.</summary>
public class DocumentTests
{
    [Fact]
    public void ADraftCannotBeApproved()
    {
        var document = new D.Document(new D.Workflow());
        var refused = Assert.Throws<InvalidOperationException>(() => document.Apply(D.Action.APPROVE));
        Assert.Equal("cannot approve a draft document", refused.Message);
        Assert.Equal(D.Status.DRAFT, document.Status);
    }

    [Fact]
    public void TheTableHasFourLines()
    {
        var workflow = new D.Workflow();
        int allowed = Enum.GetValues<D.Status>()
            .Sum(from => Enum.GetValues<D.Action>().Count(action => workflow.After(from, action) is not null));
        Assert.Equal(4, allowed);
        Assert.Equal(D.Status.DRAFT, workflow.After(D.Status.IN_REVIEW, D.Action.REJECT));
    }

    [Fact]
    public void MainOutput()
    {
        Assert.Equal(new[]
        {
            "APPROVE -> refused: cannot approve a draft document",
            "SUBMIT -> IN_REVIEW",
            "REJECT -> DRAFT",
            "SUBMIT -> IN_REVIEW",
            "APPROVE -> PUBLISHED",
            "ARCHIVE -> ARCHIVED"
        }, Printed.By(D.Main.Run));
    }
}

/// <summary>Homework 3: the vending machine. A sold-out machine returns the coin at once.</summary>
public class VendingMachineTests
{
    [Fact]
    public void SoldOutReturnsTheCoin()
    {
        var machine = new V.VendingMachine(1);
        machine.InsertCoin();
        machine.PressButton();
        Assert.Equal("SoldOut", machine.State);
        machine.InsertCoin();
        Assert.Equal("SoldOut", machine.State);
        Assert.Equal("coin returned: sold out", machine.Log[^1]);
    }

    [Fact]
    public void OneCoinAtATime()
    {
        var machine = new V.VendingMachine(3);
        machine.PressButton();
        machine.InsertCoin();
        machine.InsertCoin();
        Assert.Equal(new[] { "insert a coin first", "coin accepted", "coin returned: one coin is enough" }, machine.Log);
        Assert.Equal("HasCoin", machine.State);
    }

    [Fact]
    public void ARefill()
    {
        var machine = new V.VendingMachine(0);
        Assert.Equal("SoldOut", machine.State);
        machine.Refill(5);
        Assert.Equal("WaitingForCoin", machine.State);
    }

    [Fact]
    public void MainOutput()
    {
        Assert.Equal(new[]
        {
            "After one sale: SoldOut",
            "After a refill: WaitingForCoin",
            "Log: [insert a coin first, coin accepted, coin returned: one coin is enough, drink given, coin returned: sold out]"
        }, Printed.By(V.Main.Run));
    }
}
