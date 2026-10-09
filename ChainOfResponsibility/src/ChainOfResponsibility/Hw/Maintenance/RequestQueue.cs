namespace dev.kaldiroglu.ChainOfResponsibility.Hw.Maintenance;

/// <summary>
/// The queue from the homework. It knows only the first developer in the chain, and never
/// which developer takes which kind of request.
/// </summary>
public sealed class RequestQueue
{
    private readonly Queue<Request> waiting = new();
    private readonly Developer first;

    public RequestQueue(Developer first)
    {
        this.first = first;
    }

    public void Add(Request request)
    {
        waiting.Enqueue(request);
    }

    public IReadOnlyList<string> ProcessAll()
    {
        List<string> assignments = [];
        while (waiting.Count > 0)
        {
            assignments.Add(first.Take(waiting.Dequeue()));
        }
        return assignments;
    }
}
