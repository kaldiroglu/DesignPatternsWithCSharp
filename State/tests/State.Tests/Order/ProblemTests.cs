using System.Reflection;
using Xunit;

namespace dev.kaldiroglu.State.Tests.Order;

using dev.kaldiroglu.State.Order.Problem;

/// <summary>
/// The three designs of Part 1: flags, a switch, and status objects. Every figure the Part 1
/// slides quote about them is asserted here. Ported from the Java <c>ProblemTest</c>.
/// </summary>
public class ProblemTests
{
    private const string Source = "Order/Problem/";

    private const BindingFlags Declared =
        BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic;

    [Fact]
    public void FourFlagsSixteenCombinations()
    {
        var flags = typeof(FlagOrder).GetFields(Declared | BindingFlags.Instance)
            .Where(f => f.FieldType == typeof(bool))
            .ToList();

        Assert.Equal(4, flags.Count);
        Assert.Equal(16, 1 << flags.Count);
        // Only five of the combinations are real statuses.
        Assert.Equal(5, Enum.GetValues<Status>().Length);
    }

    [Fact]
    public void FlagsWork()
    {
        var order = new FlagOrder();
        order.Pay();
        order.Ship("TR-1");
        order.Deliver();

        Assert.Equal(["paid", "shipped TR-1", "delivered"], order.Events);
        Assert.Throws<InvalidOperationException>(order.Cancel);
    }

    [Fact]
    public void FlagsRefund()
    {
        var order = new FlagOrder();
        order.Pay();
        order.Cancel();

        Assert.Equal(["paid", "cancelled, refund issued"], order.Events);
        Assert.Throws<InvalidOperationException>(() => order.Ship("TR-1"));
    }

    [Fact]
    public void SwitchingOrder()
    {
        var order = new SwitchingOrder();
        Assert.Equal(Status.PLACED, order.Status);

        order.Pay();
        order.Ship("TR-1");
        Assert.Equal(Status.SHIPPED, order.Status);

        var refused = Assert.Throws<InvalidOperationException>(order.Cancel);
        Assert.Equal("cannot cancel a shipped order", refused.Message);

        order.Deliver();
        Assert.Equal(Status.DELIVERED, order.Status);
        Assert.Equal(["paid", "shipped TR-1", "delivered"], order.Events);
    }

    [Fact]
    public void SwitchesHaveNoDefault()
    {
        var code = Printed.CodeOf(Source + "SwitchingOrder.cs");

        // Pay, Ship, Deliver and Cancel each switch on the status. Java writes
        // "switch (status)"; the C# port uses switch expressions, "status switch".
        Assert.Equal(4, Printed.CountOf(code, "status switch"));
        Assert.DoesNotContain("default", code);
    }

    [Fact]
    public void EnumFirstShipment()
    {
        var order = new EnumOrder();
        order.Pay();
        order.Ship("TR-1");
        order.FailDelivery();
        order.FailDelivery();
        Assert.Equal(OrderStatus.SHIPPED, order.Status);

        order.FailDelivery();

        Assert.Equal(OrderStatus.PAID, order.Status);
        Assert.Equal("back to the warehouse", order.Events[^1]);
    }

    [Fact]
    public void EnumSecondShipmentHasNoLimit()
    {
        var order = new EnumOrder();
        order.Pay();
        order.Ship("TR-1");
        order.FailDelivery();
        order.FailDelivery();
        order.FailDelivery();
        order.Ship("TR-2");
        order.FailDelivery();
        order.FailDelivery();
        order.FailDelivery();

        var events = order.Events.ToList();
        var secondShipment = events.Skip(events.IndexOf("shipped TR-2") + 1).ToList();
        Assert.Equal(["delivery attempt 4 failed", "delivery attempt 5 failed",
            "delivery attempt 6 failed"], secondShipment);
        // Still shipped: there is no limit.
        Assert.Equal(OrderStatus.SHIPPED, order.Status);

        // And it goes on: the check is == 3, so no later failure reaches it either.
        order.FailDelivery();
        Assert.Equal(OrderStatus.SHIPPED, order.Status);
    }

    [Fact]
    public void TheDataLivesInTheOrder()
    {
        // Java declares two fields. The C# port declares two internal properties.
        var orderMembers = typeof(EnumOrder).GetProperties(Declared | BindingFlags.Instance)
            .Select(p => p.Name)
            .ToList();
        Assert.Contains("TrackingNumber", orderMembers);
        Assert.Contains("FailedAttempts", orderMembers);

        // A C# enum cannot have methods, so OrderStatus is a class with five shared instances.
        var constants = typeof(OrderStatus).GetFields(Declared | BindingFlags.Static)
            .Where(f => f.FieldType == typeof(OrderStatus) && f.IsInitOnly)
            .ToList();
        Assert.Equal(5, constants.Count);

        // The status holds no data of its own. Its only instance field is the one behind Name,
        // which Java's enum keeps in java.lang.Enum. Each constant's class declares none.
        var ownFields = typeof(OrderStatus).GetFields(Declared | BindingFlags.Instance)
            .Select(f => f.Name)
            .ToList();
        Assert.Equal(["<Name>k__BackingField"], ownFields);
        foreach (var constant in constants)
        {
            var type = constant.GetValue(null)!.GetType();
            Assert.Empty(type.GetFields(Declared | BindingFlags.Instance));
        }
    }
}
