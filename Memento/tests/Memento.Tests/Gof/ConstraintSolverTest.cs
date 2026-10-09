namespace dev.kaldiroglu.Memento.Tests.Gof;

using System.Reflection;
using dev.kaldiroglu.Memento.Gof.Solution;
using Xunit;
using static dev.kaldiroglu.Memento.Tests.Printed;
using ProblemGraphic = dev.kaldiroglu.Memento.Gof.Problem.Graphic;
using ProblemSolver = dev.kaldiroglu.Memento.Gof.Problem.ConstraintSolver;
using ProblemMove = dev.kaldiroglu.Memento.Gof.Problem.MoveCommand;

/// <summary>GoF's constraint solver: A at 0, B at 100, the bend at 50; B moves 60 to the left.</summary>
public class ConstraintSolverTest
{
    /// <summary>Before the pattern: the bend goes 50, 20, and stays at 20 after undo.</summary>
    [Fact]
    public void UndoByMovingBack()
    {
        var a = new ProblemGraphic("A", 0);
        var b = new ProblemGraphic("B", 100);
        var solver = new ProblemSolver(a, b);
        var move = new ProblemMove(solver, b, -60);
        Assert.Equal("A 0 -> bend 50 -> B 100", solver.Line());
        move.Execute();
        Assert.Equal("A 0 -> bend 20 -> B 40", solver.Line());
        move.Unexecute();
        // B is back, the bend is not.
        Assert.Equal("A 0 -> bend 20 -> B 100", solver.Line());
    }

    /// <summary>With a memento: the bend goes 50, 20, and is at 50 again after undo.</summary>
    [Fact]
    public void UndoWithAMemento()
    {
        Graphic a = new Graphic("A", 0);
        Graphic b = new Graphic("B", 100);
        ConstraintSolver solver = new ConstraintSolver(a, b);
        MoveCommand move = new MoveCommand(solver, b, -60);
        Assert.Equal("A 0 -> bend 50 -> B 100", solver.Line());
        move.Execute();
        Assert.Equal("A 0 -> bend 20 -> B 40", solver.Line());
        move.Unexecute();
        Assert.Equal("A 0 -> bend 50 -> B 100", solver.Line());
    }

    /// <summary>Two moves and two undos: given back in reverse order the bend is right, in another order it is wrong.</summary>
    [Fact]
    public void TwoMovesTwoUndos()
    {
        Assert.Equal("A 0 -> bend 50 -> B 100", AfterTwoMovesAndUndos(true));
        Assert.Equal("A 0 -> bend 20 -> B 100", AfterTwoMovesAndUndos(false));
    }

    private static string AfterTwoMovesAndUndos(bool reverseOrder)
    {
        Graphic a = new Graphic("A", 0);
        Graphic b = new Graphic("B", 100);
        ConstraintSolver solver = new ConstraintSolver(a, b);
        MoveCommand moveB = new MoveCommand(solver, b, -60);
        MoveCommand moveA = new MoveCommand(solver, a, 30);
        moveB.Execute();
        moveA.Execute();
        if (reverseOrder)
        {
            moveA.Unexecute();
            moveB.Unexecute();
        }
        else
        {
            moveB.Unexecute();
            moveA.Unexecute();
        }
        return solver.Line();
    }

    /// <summary>
    /// The memento is closed to the caretaker. The Java checks that the memento's field and
    /// constructor are private and that it has no methods. In C# an outer class cannot read a
    /// nested class's private members, so the port hides the state in a different way: the
    /// class that holds the state is private, and the caretaker sees only an empty public
    /// interface. This test checks that idea.
    /// </summary>
    [Fact]
    public void TheMementoIsClosed()
    {
        // What the caretaker sees has no members at all: nothing to read, nothing to call.
        Type seen = typeof(ConstraintSolver.IMemento);
        Assert.True(seen.IsInterface);
        Assert.Empty(seen.GetMembers());

        // The class that holds the bend is private to the solver.
        Type? memento = typeof(ConstraintSolver).GetNestedType("Memento", BindingFlags.NonPublic);
        Assert.NotNull(memento);
        Assert.True(memento!.IsNestedPrivate);
        Assert.True(seen.IsAssignableFrom(memento));

        // The caretaker, MoveCommand, keeps the memento only as the empty interface.
        FieldInfo[] fields = typeof(MoveCommand).GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.Contains(fields, f => f.FieldType == typeof(ConstraintSolver.IMemento));
        Assert.DoesNotContain(fields, f => f.FieldType == memento);
    }

    /// <summary>Main prints the bend at each step in both designs.</summary>
    [Fact]
    public void MainOutput()
    {
        Assert.Equal(new[]
        {
            "Before the pattern",
            "  at the start: A 0 -> bend 50 -> B 100",
            "  after move:   A 0 -> bend 20 -> B 40",
            "  after undo:   A 0 -> bend 20 -> B 100",
            "With a memento",
            "  at the start: A 0 -> bend 50 -> B 100",
            "  after move:   A 0 -> bend 20 -> B 40",
            "  after undo:   A 0 -> bend 50 -> B 100"
        }, By(global::dev.kaldiroglu.Memento.Gof.Main.Run));
    }
}
