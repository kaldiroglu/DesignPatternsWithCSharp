using Xunit;

// The examples print, and several tests capture Console.Out to read what they printed.
// xUnit runs test classes in parallel by default, so a capture opened by one class could
// take the output of another class. The tests therefore run one at a time.
[assembly: CollectionBehavior(DisableTestParallelization = true)]
