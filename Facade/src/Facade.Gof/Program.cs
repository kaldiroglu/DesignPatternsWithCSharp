using dev.kaldiroglu.Facade.Gof;

// Client (GoF p. 185): compiles source using ONLY the Compiler facade.
const string source =
    """
    x = 3
    y = 4
    z = x + y - 1
    return z
    """;

var compiler = new Compiler();

void Compile()
{
    Console.WriteLine("== Source ==");
    Console.WriteLine(source);

    Console.WriteLine("\n== Bytecode (stack machine — the facade's default) ==");
    Console.WriteLine(compiler.Compile(source));
}

void CompileRisc()
{
    Console.WriteLine("== Same source, RISC target (subsystem still reachable directly) ==");
    var risc = new BytecodeStream();
    compiler.Compile(source, new RiscCodeGenerator(risc));
    Console.WriteLine(risc);
}

// One slide, one run: `dotnet run -- compile` or `-- compile-risc`. With no argument, both.
switch (args.FirstOrDefault())
{
    case "compile":
        Compile();
        break;
    case "compile-risc":
        CompileRisc();
        break;
    case null:
        Compile();
        Console.WriteLine();
        CompileRisc();
        break;
    default:
        Console.WriteLine($"unknown example '{args[0]}'. One of: compile, compile-risc");
        break;
}
