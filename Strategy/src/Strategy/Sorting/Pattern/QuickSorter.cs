namespace dev.kaldiroglu.Strategy.Sorting.Pattern;

/// <summary>The middle case: too big to bubble, small enough that the library's setup is not free.</summary>
public sealed class QuickSorter : ISorter
{
    public string Name => "QuickSort";

    public void Sort(double[] list)
    {
        Quicksort(list, 0, list.Length - 1);
    }

    private void Quicksort(double[] a, int left, int right)
    {
        if (right <= left)
        {
            return;
        }
        var i = Partition(a, left, right);
        Quicksort(a, left, i - 1);
        Quicksort(a, i + 1, right);
    }

    private int Partition(double[] a, int left, int right)
    {
        var i = left;
        var j = right;
        while (true)
        {
            while (a[i] < a[right])
            {
                i++;
            }
            while (a[right] < a[--j])
            {
                if (j == left)
                {
                    break;
                }
            }
            if (i >= j)
            {
                break;
            }
            Exchange(a, i, j);
        }
        Exchange(a, i, right);
        return i;
    }

    private void Exchange(double[] a, int i, int j)
    {
        var swap = a[i];
        a[i] = a[j];
        a[j] = swap;
    }
}
