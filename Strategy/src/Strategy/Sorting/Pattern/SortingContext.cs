namespace dev.kaldiroglu.Strategy.Sorting.Pattern;

/// <summary>
/// The <b>Context</b>, and the slide this example exists for.
/// <para>
/// Read the branch below, then read the one in <c>Problem.Sorter</c>. They test the same two
/// thresholds and they are not the same thing at all. That one <b>implemented</b> three
/// algorithms; this one <b>selects</b> between three objects and implements none.
/// </para>
/// <para>
/// That is the honest version of "Strategy removes your if statements". It does not. It
/// separates deciding from doing, and leaves the deciding somewhere small — which is worth
/// saying out loud, because a deck that promises the branch disappears is teaching something
/// the code does not do.
/// </para>
/// <para>
/// The cost is stated in GoF's consequences and is real: as algorithms are added, this method
/// grows. A registry keyed on the input, as <c>Pricing.Solution.CampaignBook</c> uses, is the
/// usual next step.
/// </para>
/// </summary>
public sealed class SortingContext
{
    private readonly ISorter _bubbleSorter = new BubbleSorter();
    private readonly ISorter _quickSorter = new QuickSorter();
    private readonly ISorter _javaSorter = new NetSorter();

    /// <summary>Below this many elements, bubbling beats setting anything else up.</summary>
    public const int BubbleLimit = 100;

    /// <summary>Above this many, hand it to the library.</summary>
    public const int QuickLimit = 1_000_000;

    public string LastUsed { get; private set; } = "none";

    /// <summary>Choose an algorithm for this array, then let it do the work.</summary>
    public void Sort(double[] list)
    {
        var sorter = SorterFor(list.Length);
        LastUsed = sorter.Name;
        sorter.Sort(list);
    }

    /// <summary>The decision, on its own and testable without sorting anything.</summary>
    public ISorter SorterFor(int size)
    {
        if (size < BubbleLimit)
        {
            return _bubbleSorter;
        }
        if (size < QuickLimit)
        {
            return _quickSorter;
        }
        return _javaSorter;
    }
}
