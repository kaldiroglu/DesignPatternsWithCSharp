namespace dev.kaldiroglu.Bridge.Gof.Problem;

/// <summary>
/// A window, in the design GoF start from: one hierarchy, and the platform is a subclass of it
/// (Design Patterns, p. 151).
/// <para>
/// The shared, platform-independent work lives here. The platform-specific drawing is left
/// abstract, so each platform provides it — which is a perfectly reasonable first design, and
/// works, right up until a second reason to subclass arrives.
/// </para>
/// </summary>
public abstract class Window
{
    protected Window(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public int Width { get; }

    public int Height { get; }

    // --- platform-specific: every platform must supply these ------------------------

    public abstract void DrawRect(Canvas canvas, int x, int y, int w, int h);

    public abstract void DrawText(Canvas canvas, int x, int y, string text);

    /// <summary>The name of the windowing system this window is tied to.</summary>
    public abstract string Platform { get; }

    // --- platform-independent: written once, in terms of the operations above -------

    public void DrawBorder(Canvas canvas) => DrawRect(canvas, 0, 0, Width, Height);

    /// <summary>What this <i>kind</i> of window shows. Overridden by the window kinds below.</summary>
    public virtual void DrawContents(Canvas canvas) => DrawBorder(canvas);

    public static string Render(Window window)
    {
        var canvas = new Canvas(window.Width, window.Height);
        window.DrawContents(canvas);
        return canvas.ToString();
    }
}
