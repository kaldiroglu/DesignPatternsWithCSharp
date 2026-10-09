namespace dev.kaldiroglu.Command.Lender.Lambda;

/// <summary>
/// The lender of <c>Lender.Pattern</c>, with the command as a function.
/// <para>
/// <c>Pattern.ICommand</c> has one method, <c>Execute(int)</c>, so it is a function from an
/// amount to nothing. .NET already has that type: <see cref="Action{T}"/> of <c>int</c>. The
/// lender takes one, and a lambda or a method group can be the borrower or the tax office. No
/// command interface and no command classes are needed. The Java version takes an
/// <c>IntConsumer</c>.
/// </para>
/// </summary>
public class Lender
{
    public void Lend(Action<int> command, int money)
    {
        command(money);
    }
}
