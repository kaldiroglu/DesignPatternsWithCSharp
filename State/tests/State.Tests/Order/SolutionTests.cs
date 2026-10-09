using System.Reflection;
using Xunit;

namespace dev.kaldiroglu.State.Tests.Order;

using dev.kaldiroglu.State.Order.Solution;
using OrderContext = global::dev.kaldiroglu.State.Order.Solution.Order;
using OrderMain = global::dev.kaldiroglu.State.Order.Solution.Main;

/// <summary>
/// The order with State objects. Every figure the Part 3 slides quote about the order is
/// asserted here. Ported from the Java <c>SolutionTest</c>.
/// </summary>
public class SolutionTests
{
    private const string Source = "Order/Solution/";

    private static readonly string[] Operations = ["Pay", "Ship", "FailDelivery", "Deliver", "Cancel"];

    private static OrderContext SecondShipmentFailedThreeTimes()
    {
        var order = new OrderContext();
        order.Pay();
        order.Ship("TR-1");
        order.FailDelivery();
        order.FailDelivery();
        order.FailDelivery();
        order.Ship("TR-2");
        order.FailDelivery();
        order.FailDelivery();
        order.FailDelivery();
        return order;
    }

    [Fact]
    public void TheSecondShipmentHasThreeAttempts()
    {
        var order = SecondShipmentFailedThreeTimes();

        var events = order.Events.ToList();
        var secondShipment = events.Skip(events.IndexOf("shipped TR-2") + 1).ToList();
        Assert.Equal(["delivery attempt 1 failed", "delivery attempt 2 failed",
            "delivery attempt 3 failed", "back to the warehouse"], secondShipment);
        Assert.Equal<IOrderState>(new Paid(), order.State);
    }

    [Fact]
    public void ANewShipmentStartsAtZero()
    {
        var order = new OrderContext();
        order.Pay();
        order.Ship("TR-1");
        Assert.Equal<IOrderState>(new Shipped("TR-1", 0), order.State);

        order.FailDelivery();
        Assert.Equal<IOrderState>(new Shipped("TR-1", 1), order.State);
        Assert.Equal(3, Shipped.MaxAttempts);
    }

    [Fact]
    public void TheHappyPath()
    {
        var order = new OrderContext();
        Assert.Equal<IOrderState>(new Placed(), order.State);
        order.Pay();
        order.Ship("TR-1");
        order.Deliver();

        Assert.Equal<IOrderState>(new Delivered(), order.State);
        Assert.Equal(["paid", "shipped TR-1", "delivered"], order.Events);
    }

    [Fact]
    public void Cancelling()
    {
        var placed = new OrderContext();
        placed.Cancel();
        Assert.Equal(["cancelled"], placed.Events);
        Assert.Equal<IOrderState>(new Cancelled(), placed.State);

        var paid = new OrderContext();
        paid.Pay();
        paid.Cancel();
        Assert.Equal(["paid", "cancelled, refund issued"], paid.Events);
    }

    [Fact]
    public void RefusedByDefault()
    {
        var order = new OrderContext();
        order.Pay();
        order.Ship("TR-1");

        var refused = Assert.Throws<InvalidOperationException>(order.Cancel);
        Assert.Equal("cannot cancel a shipped order", refused.Message);
        Assert.Equal<IOrderState>(new Shipped("TR-1", 0), order.State);

        order.Deliver();
        Action[] requests =
            [order.Pay, () => order.Ship("TR-2"), order.FailDelivery, order.Deliver, order.Cancel];
        foreach (var request in requests)
        {
            Assert.Throws<InvalidOperationException>(request);
        }
    }

    [Fact]
    public void TheEndStatesOverrideNothing()
    {
        foreach (var end in new[] { typeof(Delivered), typeof(Cancelled) })
        {
            // An explicit interface implementation is named "...IOrderState.Pay", so the
            // check reads the part after the last dot.
            var declared = end.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance
                                          | BindingFlags.Public | BindingFlags.NonPublic)
                .Select(m => m.Name[(m.Name.LastIndexOf('.') + 1)..])
                .Where(name => Operations.Contains(name))
                .ToList();
            Assert.Empty(declared);
        }
    }

    /// <summary>
    /// Java checks a sealed interface that permits exactly five records. C# has no sealed
    /// interface, so this checks the closest form the port uses: exactly five types in the
    /// library implement <see cref="IOrderState"/>, each a sealed record.
    /// </summary>
    [Fact]
    public void FiveSealedStates()
    {
        Assert.True(typeof(IOrderState).IsInterface);
        var states = typeof(IOrderState).Assembly.GetTypes()
            .Where(t => typeof(IOrderState).IsAssignableFrom(t) && t != typeof(IOrderState))
            .ToHashSet();

        Assert.Equal(
            new HashSet<Type> { typeof(Placed), typeof(Paid), typeof(Shipped), typeof(Delivered), typeof(Cancelled) },
            states);
        Assert.All(states, t => Assert.True(t.IsSealed, t.Name));
        // A record is the only class the compiler gives a "<Clone>$" method.
        Assert.All(states, t => Assert.NotNull(t.GetMethod("<Clone>$")));

        // Only Shipped has data: its positional parameters.
        Assert.Equal(["TrackingNumber", "FailedAttempts"], PositionalParameters(typeof(Shipped)));
        Assert.All(states.Where(t => t != typeof(Shipped)),
            t => Assert.Empty(PositionalParameters(t)));
    }

    /// <summary>The parameters of a record's public primary constructor.</summary>
    private static List<string> PositionalParameters(Type record) =>
        record.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
            .Single()
            .GetParameters()
            .Select(p => p.Name!)
            .ToList();

    [Fact]
    public void TheContextOnlyForwards()
    {
        var code = Printed.CodeOf(Source + "Order.cs");

        Assert.DoesNotContain("switch", code);
        Assert.DoesNotContain("bool", code);
        Assert.DoesNotContain("trackingNumber;", code);
        Assert.DoesNotContain("failedAttempts", code);
        Assert.DoesNotContain("FailedAttempts", code);
        Assert.Contains("state = state.Ship(this, trackingNumber);", code);
    }

    [Fact]
    public void MainOutput()
    {
        var lines = Printed.By(OrderMain.Run);

        Assert.Equal(5, lines.Count);
        Assert.StartsWith("Enum constants: ", lines[0]);
        Assert.EndsWith("shipped TR-2, delivery attempt 4 failed]", lines[0]);
        Assert.Equal("  status now: SHIPPED", lines[1]);
        Assert.EndsWith("shipped TR-2, delivery attempt 1 failed]", lines[2]);
        Assert.Equal("  state now: Shipped[trackingNumber=TR-2, failedAttempts=1]", lines[3]);
        Assert.Equal("Cancel a shipped order: cannot cancel a shipped order", lines[4]);
    }
}
