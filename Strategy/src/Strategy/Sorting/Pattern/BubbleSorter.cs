namespace dev.kaldiroglu.Strategy.Sorting.Pattern;

/// <summary>Quickest on a short array, and hopeless on a long one.</summary>
public sealed class BubbleSorter : ISorter
{
    public string Name => "BubbleSort";

    public void Sort(double[] list)
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
