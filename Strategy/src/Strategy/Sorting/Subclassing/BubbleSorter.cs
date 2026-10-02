namespace dev.kaldiroglu.Strategy.Sorting.Subclassing;

/// <summary>Quickest on a short array, and hopeless on a long one.</summary>
public sealed class BubbleSorter : Sorter
{
    public override string Name => "BubbleSort";

    public override void Sort(double[] list)
    {
        for (var counter = 0; counter < list.Length - 1; counter++)
        {
            for (var index = 0; index < list.Length - 1 - counter; index++)
            {
                if (list[index] > list[index + 1])
                {
                    var temp = list[index];
                    list[index] = list[index + 1];
                    list[index + 1] = temp;
                }
            }
        }
    }
}
