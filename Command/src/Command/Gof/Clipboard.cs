namespace dev.kaldiroglu.Command.Gof;

/// <summary>What the user last copied. One per application, shared by every document in it.</summary>
public sealed class Clipboard
{
    public string Contents { get; private set; } = "";

    public void Put(string text) => Contents = text;
}
