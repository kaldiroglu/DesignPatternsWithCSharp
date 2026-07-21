using dev.kaldiroglu.Facade.Gof;

// Client (GoF p. 185): compiles source using ONLY the Compiler facade.
const string source =
    """
    x = 3
    y = 4
    z = x + y - 1
    return z
    """;

Console.WriteLine("== Source ==");
Console.WriteLine(source);

var compiler = new Compiler();

Console.WriteLine("\n== Bytecode (stack machine — the facade's default) ==");
Console.WriteLine(compiler.Compile(source));

Console.WriteLine("\n== Same source, RISC target (subsystem still reachable directly) ==");
var risc = new BytecodeStream();
compiler.Compile(source, new RiscCodeGenerator(risc));
Console.WriteLine(risc);
