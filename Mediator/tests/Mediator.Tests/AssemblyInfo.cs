using Xunit;

// The examples print, and several tests capture Console.Out to read what they print.
// xUnit runs test classes in parallel by default, and then a capture opened by one class
// also takes the output of another class. So the tests in this project run one at a time.
[assembly: CollectionBehavior(DisableTestParallelization = true)]
