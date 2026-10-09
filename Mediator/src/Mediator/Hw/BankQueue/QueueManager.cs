using System.Globalization;

namespace dev.kaldiroglu.Mediator.Hw.BankQueue;

/// <summary>
/// Homework 1: the <b>Mediator</b> of a bank branch.
/// <para>
/// Customers and tellers never look for each other. A customer who arrives takes a number
/// here; a teller who becomes free tells the manager. The manager keeps the waiting
/// customers and the idle tellers, and pairs them in arrival order.
/// </para>
/// </summary>
public sealed class QueueManager
{
    private readonly Queue<Customer> waiting = new();
    private readonly Queue<Teller> idle = new();
    private readonly List<string> log = [];
    private int nextNumber = 1;

    public void Arrive(Customer customer)
    {
        customer.Number = nextNumber++;
        log.Add(customer.Name + " takes number " + Number(customer));
        if (idle.Count == 0)
        {
            waiting.Enqueue(customer);
        }
        else
        {
            Pair(idle.Dequeue(), customer);
        }
    }

    public void TellerFree(Teller teller)
    {
        if (waiting.Count == 0)
        {
            idle.Enqueue(teller);
            log.Add(teller.Name + " waits for a customer");
        }
        else
        {
            Pair(teller, waiting.Dequeue());
        }
    }

    private void Pair(Teller teller, Customer customer)
    {
        log.Add(teller.Name + " calls number " + Number(customer) + " (" + customer.Name + ")");
    }

    private static string Number(Customer customer) => customer.Number.ToString(CultureInfo.InvariantCulture);

    /// <summary>A copy of the log, as Java's <c>List.copyOf</c> gives.</summary>
    public IReadOnlyList<string> Log => log.ToList();
}
