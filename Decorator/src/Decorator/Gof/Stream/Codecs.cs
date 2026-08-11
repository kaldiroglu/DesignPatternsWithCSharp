namespace dev.kaldiroglu.Decorator.Gof.Stream;

/// <summary>
/// The two transformations the streams apply. Shared by both designs, so the only
/// difference between Problem and Solution is how they are attached.
/// </summary>
public static class Codecs
{
    private static readonly Dictionary<char, string> Foldings = new()
    {
        ['á'] = "a", ['à'] = "a", ['â'] = "a", ['ä'] = "a",
        ['é'] = "e", ['è'] = "e", ['ê'] = "e", ['ë'] = "e",
        ['í'] = "i", ['ï'] = "i", ['î'] = "i",
        ['ó'] = "o", ['ô'] = "o", ['ö'] = "o",
        ['ú'] = "u", ['ü'] = "u", ['û'] = "u",
        ['ç'] = "c", ['ñ'] = "n", ['ß'] = "ss"
    };

    /// <summary>Run-length compression: "aaa" becomes "3a".</summary>
    public static string Compress(string input)
    {
        var outp = new System.Text.StringBuilder();
        var i = 0;
        while (i < input.Length)
        {
            var c = input[i];
            var run = 1;
            while (i + run < input.Length && input[i + run] == c)
            {
                run++;
            }

            if (run >= 2)
            {
                outp.Append(run);
            }

            outp.Append(c);
            i += run;
        }

        return outp.ToString();
    }

    /// <summary>Folds accented characters down to 7-bit ASCII: "café" becomes "cafe".</summary>
    public static string ToAscii7(string input)
    {
        var outp = new System.Text.StringBuilder();
        foreach (var c in input)
        {
            if (c < 128)
            {
                outp.Append(c);
            }
            else if (!Foldings.TryGetValue(char.ToLowerInvariant(c), out var folded))
            {
                outp.Append('?');
            }
            else if (char.IsUpper(c))
            {
                outp.Append(folded.ToUpperInvariant());
            }
            else
            {
                outp.Append(folded);
            }
        }

        return outp.ToString();
    }
}
