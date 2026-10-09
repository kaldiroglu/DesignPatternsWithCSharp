using Xunit;

// The examples print. The order's Main, GoF's Main and the door demos write to the console,
// and the tests capture Console.Out to read what they printed. xUnit runs test classes in
// parallel by default, and then a capture opened by one class would also catch the output of
// another class. So the tests run one at a time.
[assembly: CollectionBehavior(DisableTestParallelization = true)]
