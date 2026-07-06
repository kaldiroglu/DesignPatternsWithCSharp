namespace dev.kaldiroglu.Adapter.Gof.Demo;

public static class Program
{
    // Runs the drawing-editor demo. The pluggable shape demo now lives in its own runnable
    // project, Gof.Pluggable (`dotnet run --project src/Gof.Pluggable`).
    public static void Main(string[] args)
    {
        dev.kaldiroglu.Adapter.Gof.DrawingEditor.Run();
    }
}
