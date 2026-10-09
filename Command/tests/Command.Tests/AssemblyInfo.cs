using Xunit;

// The lender and air conditioner examples print, and their tests capture Console.Out to read
// what was printed. xUnit runs test classes in parallel by default, and a capture opened by
// one class would then take in another class's output. So the tests run one at a time.
[assembly: CollectionBehavior(DisableTestParallelization = true)]
