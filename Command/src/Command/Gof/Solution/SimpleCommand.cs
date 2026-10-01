namespace dev.kaldiroglu.Command.Gof.Solution;

/// <summary>
/// GoF's <c>SimpleCommand</c>: one class for every command that only forwards.
/// <para>
/// In the book it is a C++ template holding a receiver and a pointer to one of its member
/// functions, so that a paste needs no <c>PasteCommand</c> class of its own. That is GoF
/// implementation issue 4 (using C++ templates). In C# the member-function pointer is a
/// delegate, written as a lambda, and the whole of <c>PasteCommand</c> becomes
/// <code>new SimpleCommand&lt;Document&gt;(document, d =&gt; d.Paste())</code>
/// </para>
/// <para>
/// The limit is the same in both languages: it suits commands that are not undoable and
/// take no arguments. Anything that has to remember what it did needs a class.
/// </para>
/// </summary>
public sealed class SimpleCommand<TReceiver> : ICommand
{
    private readonly TReceiver _receiver;
    private readonly Action<TReceiver> _action;

    public SimpleCommand(TReceiver receiver, Action<TReceiver> action)
    {
        _receiver = receiver;
        _action = action;
    }

    public void Execute() => _action(_receiver);
}
