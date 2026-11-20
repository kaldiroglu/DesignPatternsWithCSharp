using System.Reflection;
using System.Text.RegularExpressions;
using Xunit;

namespace dev.kaldiroglu.State.Tests.Order;

using dev.kaldiroglu.State.Order.Problem;
using dev.kaldiroglu.State.Order.Solution;
using OrderContext = global::dev.kaldiroglu.State.Order.Solution.Order;

/// <summary>
/// The order as a finite state machine. Every claim the state-machine slides make about the
/// order is asserted here: the table, the start and end states, the guard, and where each
/// design keeps the table.
/// </summary>
public class StateMachineTests
{
    private static readonly string[] EventNames = ["Pay", "Ship", "FailDelivery", "Deliver", "Cancel"];

    [Fact]
    public void TheTable()
    {
        Assert.Equal(
            [
                "           Pay   Ship     FailDelivery                Deliver    Cancel",
                "placed     paid  -        -                           -          cancelled",
                "paid       -     shipped  -                           -          cancelled",
                "shipped    -     -        shipped; paid on attempt 3  delivered  -",
                "delivered  -     -        -                           -          -",
                "cancelled  -     -        -                           -          -",
            ],
            Printed.By(TransitionTableMain.Run));
    }

    [Fact]
    public void MostOfTheTableIsRefusals()
    {
        var cells = Printed.By(TransitionTableMain.Run).Skip(1)
            .SelectMany(row => Regex.Split(row, @"\s{2,}").Skip(1))
            .ToList();

        Assert.Equal(25, cells.Count);
        Assert.Equal(6, cells.Count(cell => cell != "-"));
    }

    [Fact]
    public void OneStartTwoEnds()
    {
        Assert.Equal(new Placed(), new OrderContext().State);

        foreach (var end in new[] { Delivered(), Cancelled() })
        {
            var before = end.State;
            foreach (var @event in EventsOf(end))
            {
                Assert.Throws<InvalidOperationException>(@event);
            }
            Assert.Equal(before, end.State);
        }
    }

    [Fact]
    public void TheGuard()
    {
        var order = new OrderContext();
        order.Pay();
        order.Ship("TR-1");
        order.FailDelivery();
        Assert.Equal("shipped", order.State.Name);
        order.FailDelivery();
        Assert.Equal("shipped", order.State.Name);
        order.FailDelivery();
        Assert.Equal(new Paid(), order.State);
    }

    [Fact]
    public void AnEventWithNoTransitionIsRefused()
    {
        var order = new OrderContext();
        order.Pay();
        order.Ship("TR-1");
        var refused = Assert.Throws<InvalidOperationException>(order.Cancel);
        Assert.Equal("cannot cancel a shipped order", refused.Message);
    }

    [Fact]
    public void TheActionDependsOnTheStateAndTheEvent()
    {
        var placed = new OrderContext();
        placed.Cancel();
        var paid = new OrderContext();
        paid.Pay();
        paid.Cancel();

        Assert.Equal("cancelled", placed.Events[^1]);
        Assert.Equal("cancelled, refund issued", paid.Events[^1]);
        Assert.Equal(placed.State, paid.State);
    }

    [Fact]
    public void TheSwitchCodesTheSameMachine()
    {
        var expected = new Dictionary<string, string>
        {
            ["placed Pay"] = "paid", ["placed Cancel"] = "cancelled",
            ["paid Ship"] = "shipped", ["paid Cancel"] = "cancelled",
            ["shipped Deliver"] = "delivered",
        };

        foreach (var state in new[] { "placed", "paid", "shipped", "delivered", "cancelled" })
        {
            foreach (var @event in new[] { "Pay", "Ship", "Deliver", "Cancel" })
            {
                var order = SwitchingOrderIn(state);
                string next;
                try
                {
                    Send(order, @event);
                    next = order.Status.ToString().ToLowerInvariant();
                }
                catch (InvalidOperationException)
                {
                    next = "-";
                }
                Assert.Equal(expected.GetValueOrDefault(state + " " + @event, "-"), next);
            }
        }
    }

    [Fact]
    public void EachMethodIsOneColumn()
    {
        var code = Printed.CodeOf("Order/Problem/SwitchingOrder.cs");
        var methods = Regex.Matches(code,
            @"public void (\w+)\([^)]*\)\s*\{\s*status = status switch\s*\{(.*?)\};",
            RegexOptions.Singleline);

        Assert.Equal(4, methods.Count);
        foreach (Match method in methods)
        {
            foreach (var status in Enum.GetNames<Status>())
            {
                Assert.Contains("Status." + status, method.Groups[2].Value);
            }
        }
    }

    [Fact]
    public void EachClassIsOneRow()
    {
        var rows = new Dictionary<Type, HashSet<string>>
        {
            [typeof(Placed)] = ["Pay", "Cancel"],
            [typeof(Paid)] = ["Ship", "Cancel"],
            [typeof(Shipped)] = ["FailDelivery", "Deliver"],
            [typeof(Delivered)] = [],
            [typeof(Cancelled)] = [],
        };

        var events = typeof(IOrderState).GetMethods()
            .Where(m => !m.IsAbstract)
            .Select(m => m.Name)
            .ToHashSet();
        Assert.Equal(new HashSet<string>(EventNames), events);

        foreach (var (state, allowed) in rows)
        {
            var overridden = state.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Select(m => m.Name)
                .Where(events.Contains)
                .ToHashSet();
            Assert.Equal(allowed, overridden);
        }
    }

    private static OrderContext Delivered()
    {
        var order = new OrderContext();
        order.Pay();
        order.Ship("TR-1");
        order.Deliver();
        return order;
    }

    private static OrderContext Cancelled()
    {
        var order = new OrderContext();
        order.Cancel();
        return order;
    }

    private static Action[] EventsOf(OrderContext order) =>
        [order.Pay, () => order.Ship("TR-2"), order.FailDelivery, order.Deliver, order.Cancel];

    private static SwitchingOrder SwitchingOrderIn(string state)
    {
        var order = new SwitchingOrder();
        switch (state)
        {
            case "placed": break;
            case "paid": order.Pay(); break;
            case "shipped": order.Pay(); order.Ship("TR-1"); break;
            case "delivered": order.Pay(); order.Ship("TR-1"); order.Deliver(); break;
            case "cancelled": order.Cancel(); break;
            default: throw new ArgumentException(state);
        }
        return order;
    }

    private static void Send(SwitchingOrder order, string @event)
    {
        switch (@event)
        {
            case "Pay": order.Pay(); break;
            case "Ship": order.Ship("TR-1"); break;
            case "Deliver": order.Deliver(); break;
            case "Cancel": order.Cancel(); break;
            default: throw new ArgumentException(@event);
        }
    }
}
