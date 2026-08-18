namespace dev.kaldiroglu.Bridge.Gof.Solution;

/// <summary>
/// The <b>Implementor</b> (GoF p. 154): the interface for window implementations.
/// <para>
/// Read the method names. They are <i>device primitives</i> — the smallest operations a
/// windowing system can be asked for — not window operations. GoF is explicit about this
/// (p. 153): "WindowImp declares an interface that provides access to the low-level primitives
/// that the underlying window system supplies. The Implementor interface provides only
/// primitive operations, and Abstraction defines higher-level operations based on these
/// primitives."
/// </para>
/// <para>
/// That split is the solution. If this interface grew a <c>DrawIcon</c> method it would stop
/// being an implementor and start being a second copy of the abstraction.
/// </para>
/// </summary>
public interface IWindowImp
{
    /// <summary>The name of the windowing system behind this implementation.</summary>
    string Platform { get; }

    void DeviceRect(Canvas canvas, int x, int y, int width, int height);

    void DeviceText(Canvas canvas, int x, int y, string text);

    /// <summary>Bring the window to the front. Recorded rather than drawn, so tests can see it.</summary>
    void DeviceRaise();

    void DeviceLower();

    /// <summary>What the platform has been asked to do, in order.</summary>
    IReadOnlyList<string> Journal { get; }
}
