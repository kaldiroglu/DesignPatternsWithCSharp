namespace dev.kaldiroglu.Bridge.Gof.Solution;

/// <summary>
/// A <b>ConcreteImplementor</b>: IBM's Presentation Manager (GoF p. 154).
/// <para>
/// GoF's own version is worth remembering: PM has no single call that draws a rectangle, so
/// <c>DeviceRect</c> has to build one out of lines. The abstraction never learns this.
/// </para>
/// </summary>
public sealed class PMWindowImp : IWindowImp
{
    private readonly List<string> _journal = [];

    public string Platform => "PM";

    public void DeviceRect(Canvas canvas, int x, int y, int width, int height)
    {
        // PM draws a rectangle as a polyline of four points — GoF, p. 157.
        _journal.Add("GpiBeginPath()");
        _journal.Add("GpiPolyLine(4 points)");
        _journal.Add("GpiEndPath()");
        canvas.Rectangle(x, y, width, height, '#', '=', '!');
    }

    public void DeviceText(Canvas canvas, int x, int y, string text)
    {
        _journal.Add($"GpiCharStringAt({x},{y},\"{text}\")");
        canvas.Text(x, y, text);
    }

    public void DeviceRaise() => _journal.Add("WinSetWindowPos(HWND_TOP)");

    public void DeviceLower() => _journal.Add("WinSetWindowPos(HWND_BOTTOM)");

    public IReadOnlyList<string> Journal => _journal.AsReadOnly();
}
