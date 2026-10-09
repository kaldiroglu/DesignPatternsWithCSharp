using Xunit;

// Several examples print to the console, and the tests capture Console.Out to read what they
// printed. Under xUnit's default, test classes run in parallel, and a capture opened by one
// class would also collect the output of another. So the tests in this project run one at a time.
[assembly: CollectionBehavior(DisableTestParallelization = true)]
