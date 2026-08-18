namespace dev.kaldiroglu.Bridge.Gof.Solution;

/// <summary>
/// The <b>Abstraction</b> (GoF p. 154): what a window IS, to the rest of the program.
/// <para>
/// The one field is the whole solution. This class does not extend a platform — it <i>holds</i>
/// one, and it can be handed a different one at any time. GoF calls the reference <c>imp</c>,
/// and this code keeps the name.
/// </para>
/// <para>
/// Every operation here is written in terms of the implementor's primitives, never in terms of
/// any particular platform. That is what lets the two hierarchies below and beside it grow
/// without ever meeting.
/// </para>
/// <para>
/// GoF implementation issue 1 asks who chooses the implementor, and this class answers "the
/// caller": the reference arrives in the constructor. Note also that it is <c>private</c> here
/// and reached through a <c>protected</c> property — refined abstractions draw through it, and
/// nothing outside the hierarchy can reach past the window to the device.
/// </para>
/// </summary>
public class Window
{
    private IWindowImp _imp;

    public Window(int width, int height, IWindowImp imp)
    {
        Width = width;
        Height = height;
        _imp = imp ?? throw new ArgumentNullException(nameof(imp), "a window needs an implementation");
    }

    /// <summary>The implementation currently in use. Subclasses draw through it.</summary>
    protected IWindowImp Imp => _imp;

    public int Width { get; }

    public int Height { get; }

    public string Platform => _imp.Platform;

    /// <summary>
    /// Swap the implementation on a window that already exists.
    /// <para>
    /// Nothing in the <c>Problem</c> namespace can do this at any price: there, the platform is
    /// the object's class.
    /// </para>
    /// </summary>
    public void SetImp(IWindowImp newImp) =>
        _imp = newImp ?? throw new ArgumentNullException(nameof(newImp));

    // --- higher-level operations, defined in terms of the primitives ----------------

    public void DrawRect(Canvas canvas, int x, int y, int w, int h) =>
        _imp.DeviceRect(canvas, x, y, w, h);

    public void DrawText(Canvas canvas, int x, int y, string text) =>
        _imp.DeviceText(canvas, x, y, text);

    public void DrawBorder(Canvas canvas) => DrawRect(canvas, 0, 0, Width, Height);

    public void RaiseWindow() => _imp.DeviceRaise();

    public void LowerWindow() => _imp.DeviceLower();

    /// <summary>What this kind of window shows. Refined abstractions override it.</summary>
    public virtual void DrawContents(Canvas canvas) => DrawBorder(canvas);

    public static string Render(Window window)
    {
        var canvas = new Canvas(window.Width, window.Height);
        window.DrawContents(canvas);
        return canvas.ToString();
    }
}
