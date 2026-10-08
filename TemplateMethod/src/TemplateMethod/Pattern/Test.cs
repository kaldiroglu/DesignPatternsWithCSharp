namespace dev.kaldiroglu.TemplateMethod.Pattern;

/// <summary>The client.</summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/TemplateMethod.Demo -- pattern</c>.
/// </remarks>
public static class Test
{
    public static void Run()
    {
        Application myApplication = new MyApplication();
        myApplication.OpenDocument("mydoc");
    }
}
