using System.Diagnostics;
using System.Reflection;
using Xunit;

namespace dev.kaldiroglu.TemplateMethod.Tests.Task;

using TaskBase = global::dev.kaldiroglu.TemplateMethod.Task.Task;
using Fax = global::dev.kaldiroglu.TemplateMethod.Task.Fax;
using Print = global::dev.kaldiroglu.TemplateMethod.Task.Print;
using Scan = global::dev.kaldiroglu.TemplateMethod.Task.Scan;
using Client = global::dev.kaldiroglu.TemplateMethod.Task.Test;

/// <summary>
/// The repeated task. The Part 3 notes say the client runs a print task ten times, one
/// second apart, and that the wait happens only between repetitions.
/// </summary>
public class TaskTests
{
    private static int Count(List<string> lines, string line) => lines.Count(l => l == line);

    /// <summary>
    /// The client runs a print task ten times, one second apart: nine waits. This is the only
    /// slow test in the project; it takes about nine seconds.
    /// </summary>
    [Fact]
    public void TheClientPrintsTenTimesWithNineWaits()
    {
        var clock = Stopwatch.StartNew();
        List<string> lines = Printed.By(Client.Run);
        long millis = clock.ElapsedMilliseconds;

        Assert.Equal(10, Count(lines, "Printing task."));
        Assert.Equal(10, Count(lines, "- in before() -"));
        Assert.Equal(10, Count(lines, "- in after() -"));
        Assert.Equal("*** Preparing printing! ***", lines[0]);
        Assert.Equal("*** Cleaning printing environment. ***", lines[^1]);
        Assert.True(millis >= 9_000, "nine waits of one second, took " + millis + " ms");
        Assert.True(millis < 10_000, "no wait after the last repetition, took " + millis + " ms");
    }

    /// <summary>The steps run in the order the template method fixes: prepare, then before, task, after, then clean.</summary>
    [Fact]
    public void TheOrderOfTheSteps()
    {
        List<string> lines = Printed.By(() => new Scan("Scanning", 0, 2).Run())
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .ToList();

        Assert.Equal([
            "*** in prepare() ***",
            "- in before() -", "I'm scanning!", "- in after() -",
            "- in before() -", "I'm scanning!", "- in after() -",
            "*** in clean() ***"], lines);
    }

    /// <summary>
    /// An interrupt during the wait stops repeating, and still cleans up.
    /// <para>
    /// The Java test also checks that the thread keeps its interrupt flag. .NET has no
    /// interrupt flag: the pending interrupt is used up when <c>Thread.Sleep</c> throws
    /// <c>ThreadInterruptedException</c>. So the C# test checks the opposite fact: after the
    /// run, the thread can sleep again without an exception. The test runs on its own thread,
    /// so the interrupt cannot reach the test runner's thread.
    /// </para>
    /// </summary>
    [Fact]
    public void AnInterruptStopsTheLoopAndCleansUp()
    {
        List<string> lines = [];
        bool stillInterrupted = true;
        var worker = new Thread(() =>
        {
            Thread.CurrentThread.Interrupt();
            lines = Printed.By(() => new Fax("Faxing", 1, 3).Run());
            try
            {
                Thread.Sleep(0);
                stillInterrupted = false;
            }
            catch (ThreadInterruptedException)
            {
                stillInterrupted = true;
            }
        });
        worker.Start();
        worker.Join();

        Assert.False(stillInterrupted);
        Assert.Equal(1, Count(lines, "I'm faxing!"));
        Assert.Equal("*** in clean() ***", lines[^1]);
    }

    /// <summary>As the exercise says: if DoTask throws an exception, Clean is never called.</summary>
    [Fact]
    public void AFailingTaskSkipsClean()
    {
        TaskBase failing = new FailingTask();

        List<string> lines = Printed.By(() => Assert.Throws<InvalidOperationException>(failing.Run));

        Assert.Contains("*** in prepare() ***", lines);
        Assert.DoesNotContain("*** in clean() ***", lines);
    }

    /// <summary>The Java test writes this as an anonymous subclass.</summary>
    private sealed class FailingTask() : TaskBase("Failing", 0, 3)
    {
        public override void DoTask() => throw new InvalidOperationException("the device is off");
    }

    /// <summary>
    /// Run is final, DoTask is the only abstract step, and the four hooks are public. Java's
    /// <c>final</c> is "not <c>virtual</c>" in C#.
    /// </summary>
    [Fact]
    public void TheKindsOfMethod()
    {
        Assert.False(typeof(TaskBase).GetMethod("Run", Type.EmptyTypes)!.IsVirtual);

        List<string> abstractSteps = typeof(TaskBase)
            .GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static
                        | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(m => m.IsAbstract)
            .Select(m => m.Name)
            .ToList();
        Assert.Equal(["DoTask"], abstractSteps);

        foreach (string hook in new[] { "Prepare", "Before", "After", "Clean" })
        {
            var method = typeof(TaskBase).GetMethod(hook, Type.EmptyTypes)!;
            Assert.True(method.IsPublic, hook);
            Assert.False(method.IsAbstract, hook);
        }
    }

    /// <summary>Print overrides Prepare and Clean; Fax and Scan write only DoTask.</summary>
    [Fact]
    public void WhichHooksEachTaskOverrides()
    {
        Assert.Equal(new HashSet<string> { "Prepare", "Clean", "DoTask" }, Code.DeclaredMethodsOf(typeof(Print)));
        Assert.Equal(new HashSet<string> { "DoTask" }, Code.DeclaredMethodsOf(typeof(Fax)));
        Assert.Equal(new HashSet<string> { "DoTask" }, Code.DeclaredMethodsOf(typeof(Scan)));
    }
}
