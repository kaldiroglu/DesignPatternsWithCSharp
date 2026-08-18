using dev.kaldiroglu.Bridge.Gof;
using dev.kaldiroglu.Bridge.Gof.Solution;
using Xunit;

namespace dev.kaldiroglu.Bridge.Tests.Gof;

/// <summary>
/// GoF's own Bridge (p. 152): the platform is an object the window holds.
/// </summary>
public class WindowSolutionTests
{
    private readonly IWindowImp _x = new XWindowImp();
    private readonly IWindowImp _pm = new PMWindowImp();

    [Fact(DisplayName = "one window kind, every platform")]
    public void OneKindManyPlatforms()
    {
        Assert.Equal("+------+\n|      |\n+------+", Window.Render(new Window(8, 3, _x)));
        Assert.Equal("#======#\n!      !\n#======#", Window.Render(new Window(8, 3, _pm)));
    }

    [Fact(DisplayName = "one platform, every window kind")]
    public void OnePlatformManyKinds()
    {
        var icon = Window.Render(new IconWindow(14, 5, "a.txt", _x));
        var transient = Window.Render(new TransientWindow(14, 5, "Save", _x));

        Assert.Contains("a.txt", icon);
        Assert.Contains("Save", transient);
        Assert.NotEqual(icon, transient);
    }

    [Fact(DisplayName = "the implementation can be swapped on a window that already exists")]
    public void ImplementationIsChosenAtRunTime()
    {
        var window = new IconWindow(14, 5, "a.txt", _x);
        var onX = Window.Render(window);
        Assert.Equal("X", window.Platform);

        window.SetImp(_pm);                      // the same object, a different platform

        Assert.Equal("PM", window.Platform);
        Assert.NotEqual(onX, Window.Render(window));
    }

    [Fact(DisplayName = "the abstraction speaks only in primitives, and never learns how they are met")]
    public void AbstractionUsesPrimitivesOnly()
    {
        IWindowImp traced = new PMWindowImp();
        new IconWindow(14, 5, "a.txt", traced).DrawContents(new Canvas(14, 5));

        // The window asked twice for a rectangle and once for text. Presentation Manager has no
        // rectangle primitive, so it built each one from a polyline — a detail the window
        // neither knows nor could act on.
        Assert.Equal(7, traced.Journal.Count);
        Assert.StartsWith("GpiBeginPath", traced.Journal[0]);
        Assert.StartsWith("GpiPolyLine", traced.Journal[1]);
        Assert.StartsWith("GpiCharStringAt", traced.Journal[^1]);

        IWindowImp onX = new XWindowImp();
        new IconWindow(14, 5, "a.txt", onX).DrawContents(new Canvas(14, 5));
        Assert.Equal(3, onX.Journal.Count);       // X has a rectangle call, so it uses it
    }

    [Fact(DisplayName = "a new platform costs one class and works with every window kind at once")]
    public void AddingAPlatformCostsOneClass()
    {
        // A hypothetical third platform, written here in a dozen lines, needs no change anywhere
        // else — and every existing window kind can use it immediately.
        IWindowImp web = new WebWindowImp();

        Assert.Contains("o....", Window.Render(new IconWindow(14, 5, "a.txt", web)));
        Assert.Contains("Save", Window.Render(new TransientWindow(14, 5, "Save", web)));
    }

    [Fact(DisplayName = "a window needs an implementation")]
    public void NullImpIsRejected() =>
        Assert.Throws<ArgumentNullException>(() => new Window(8, 3, null!));

    private sealed class WebWindowImp : IWindowImp
    {
        private readonly List<string> _journal = [];

        public string Platform => "Web";

        public void DeviceRect(Canvas c, int x, int y, int width, int height)
        {
            _journal.Add("canvas.strokeRect");
            c.Rectangle(x, y, width, height, 'o', '.', ':');
        }

        public void DeviceText(Canvas c, int x, int y, string text)
        {
            _journal.Add("canvas.fillText");
            c.Text(x, y, text);
        }

        public void DeviceRaise() => _journal.Add("z-index++");

        public void DeviceLower() => _journal.Add("z-index--");

        public IReadOnlyList<string> Journal => _journal.AsReadOnly();
    }
}
