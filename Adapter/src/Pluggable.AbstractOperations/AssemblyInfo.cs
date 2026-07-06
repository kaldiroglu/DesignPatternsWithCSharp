using System.Runtime.CompilerServices;

// PowerAdapter is internal (Java package-private); let the test project subclass it to verify the
// Template Method behaviour of technique (a).
[assembly: InternalsVisibleTo("Adapter.Tests")]
