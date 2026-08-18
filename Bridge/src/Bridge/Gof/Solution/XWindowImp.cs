namespace dev.kaldiroglu.Bridge.Gof.Solution;

/// <summary>
/// A <b>ConcreteImplementor</b>: the X Window System (GoF p. 154).
/// <para>
/// It knows about pixels and rectangles. It has never heard of icons, dialogs or titles, and it
/// will never need to change when a new kind of window is invented.
/// </para>
/// </summary>
public sealed class XWindowImp : IWindowImp
{
    private readonly List<string> _journal = [];

    public string Platform => "X";

    public void DeviceRect(Canvas canvas, int x, int y, int width, int height)
    {
        _journal.Add($"XDrawRectangle({x},{y},{width},{height})");
        canvas.Rectangle(x, y, width, height, '+', '-', '|');
    }

    public void DeviceText(Canvas canvas, int x, int y, string text)
    {
        _journal.Add($"XDrawString({x},{y},\"{text}\")");
        canvas.Text(x, y, text);
    }

    public void DeviceRaise() => _journal.Add("XRaiseWindow()");

    public void DeviceLower() => _journal.Add("XLowerWindow()");

    public IReadOnlyList<string> Journal => _journal.AsReadOnly();
}
