namespace dev.kaldiroglu.Decorator.Gof.Visual.Solution;

/// <summary>
/// The <b>Component</b> of the pattern: the interface for objects that can have
/// responsibilities added to them dynamically (GoF p. 178).
/// <para>
/// GoF's implementation issue 3, "keeping Component classes lightweight" (p. 180), is
/// followed here: this type defines an interface and stores no data at all. Every
/// decorator pays for whatever a Component carries, so a fat Component makes decorators
/// too expensive to use in quantity.
/// </para>
/// </summary>
public interface IVisualComponent
{
    /// <summary>The width of the component, including anything wrapped around it.</summary>
    int Width();

    /// <summary>The height of the component, including anything wrapped around it.</summary>
    int Height();

    /// <summary>Draws the component with its top-left corner at (x, y).</summary>
    void Draw(Canvas canvas, int x, int y);

    /// <summary>Renders any component on a canvas of exactly its own size.</summary>
    static string Render(IVisualComponent component)
    {
        var canvas = new Canvas(component.Width(), component.Height());
        component.Draw(canvas, 0, 0);
        return canvas.ToString();
    }
}
