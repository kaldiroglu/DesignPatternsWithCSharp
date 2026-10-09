namespace dev.kaldiroglu.ChainOfResponsibility.Hw.Maintenance;

/// <summary>
/// Homework 1: the <b>Handler</b>. A developer takes the requests it is suited for and
/// passes the rest to the next developer in line.
/// </summary>
public abstract class Developer
{
    private readonly string name;
    private Developer? next;

    protected Developer(string name)
    {
        this.name = name;
    }

    public Developer Then(Developer next)
    {
        this.next = next;
        return next;
    }

    public string Take(Request request)
    {
        if (Suits(request))
        {
            return request.Title + " -> " + name;
        }
        if (next == null)
        {
            return request.Title + " -> nobody: back to the team lead";
        }
        return next.Take(request);
    }

    protected abstract bool Suits(Request request);
}
