using Xunit;

// The examples print. The drawers in Shape.Solution record every device call to the console, the
// windows in Gof do the same, and ViolationTests has to capture Console.Out to prove that
// ASubType prints nothing at all — which is the only way that violation is visible.
//
// Those two facts do not coexist under xUnit's default, which runs test classes in parallel: a
// capture opened by one class swallows another class's output, and the violation test then sees
// text it never produced. The suite is 96 tests and runs in milliseconds, so there is nothing to
// win by running them at once.
[assembly: CollectionBehavior(DisableTestParallelization = true)]
