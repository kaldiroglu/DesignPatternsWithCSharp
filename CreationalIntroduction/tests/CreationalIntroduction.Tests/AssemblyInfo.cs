using Xunit;

// The examples print, and SlideRunsTests captures Console.Out. Under xUnit's default a
// capture opened by one class would swallow what another class prints.
[assembly: CollectionBehavior(DisableTestParallelization = true)]
