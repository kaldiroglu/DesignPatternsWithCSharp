namespace dev.kaldiroglu.Strategy.Sorting.Problem;

/// <summary>
/// Stage one: three sorting algorithms in one class, chosen by a branch on the array's size.
/// <para>
/// A small array is quickest to bubble; a large one is best left to the library. So the
/// choice is genuine engineering rather than indecision — which is what makes this design
/// worth taking seriously before it is taken apart.
/// </para>
/// <para>
/// What it costs is that the class does two different jobs at once: it <b>decides</b> which
/// algorithm suits the input, and it <b>implements</b> all three. Neither can be changed,
/// read or tested without the other.
/// </para>
/// </summary>
public sealed class Sorter
{
    /// <summary>How the last call was sorted, so a test can see the decision rather than infer it.</summary>
    public string LastUsed { get; private set; } = "none";

    public void Sort(double[] list)
    {
        var size = list.Length;

        if (size < 100)
        {
            LastUsed = "BubbleSort";
            for (var counter = 0; counter < size - 1; counter++)
            {
                for (var index = 0; index < size - 1 - counter; index++)
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
        else if (size < 1_000_000)
        {
            LastUsed = "QuickSort";
            Quicksort(list, 0, size - 1);
        }
        else
        {
            LastUsed = "JavaSort";
            Array.Sort(list);
        }
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
