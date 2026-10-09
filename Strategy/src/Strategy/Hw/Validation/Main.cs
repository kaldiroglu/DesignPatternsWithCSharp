namespace dev.kaldiroglu.Strategy.Hw.Validation;

/// <summary>Checks the same passphrases on a relaxed form with one rule and a strict form with three.</summary>
/// <remarks>
/// The Java original's <c>main</c>. Run it with
/// <c>dotnet run --project src/Strategy.Demo -- hw-validation</c>.
/// </remarks>
public static class Main
{
    public static void Run()
    {
        var relaxed = new SignUpForm(new MinimumLength(8));
        var strict = new SignUpForm(new MinimumLength(12), new MixedCharacters(),
            NoCommonWords.TheUsualSuspects());
        Console.WriteLine("Relaxed form: " + relaxed.RuleCount + " rule. Strict form: "
            + strict.RuleCount + " rules.");

        foreach (var passphrase in new[] { "hunter2024", "password", "kedi-42-balkon!" })
        {
            Console.WriteLine("'" + passphrase + "'");
            Console.WriteLine("  relaxed accepts: " + Show(relaxed.Accepts(passphrase)));
            Console.WriteLine("  strict accepts: " + Show(strict.Accepts(passphrase))
                + " [" + string.Join(", ", strict.Complaints(passphrase)) + "]");
        }
    }

    /// <summary>Prints a boolean the way Java does: <c>true</c>, not <c>True</c>.</summary>
    private static string Show(bool value) => value ? "true" : "false";
}
