namespace dev.kaldiroglu.Command.Lender.Pattern;

public class Lender
{
    public void Lend(ICommand command, int money)
    {
        command.Execute(money);
    }
}
