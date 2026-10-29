using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using dev.kaldiroglu.Introduction.Problems.ComplexIfs;
using Xunit;

namespace dev.kaldiroglu.Introduction.Tests.Problems.ComplexIfs;

/// <summary>
/// The figures the Introduction deck quotes for <c>ProcessUser</c>: its parameters, how deep
/// its <c>if</c> statements are nested, and how many different answers it can give.
/// <para>
/// The source is read with its comments removed, so a comment that names a result or holds
/// a brace cannot change a count.
/// </para>
/// </summary>
public class UserProcessorTests
{
    private static string Code()
    {
        var source = File.ReadAllText(SourceFile());
        source = Regex.Replace(source, @"/\*.*?\*/", "", RegexOptions.Singleline);
        return Regex.Replace(source, @"//[^\n]*", "");
    }

    [Fact(DisplayName = "ProcessUser takes 7 parameters")]
    public void SevenParameters()
    {
        var method = typeof(UserProcessor).GetMethod(nameof(UserProcessor.ProcessUser))!;
        Assert.Equal(7, method.GetParameters().Length);
    }

    [Fact(DisplayName = "its if statements are nested 15 levels deep")]
    public void FifteenLevels()
    {
        int depth = 0, deepest = 0;
        foreach (var c in Code())
        {
            if (c == '{') deepest = Math.Max(deepest, ++depth);
            if (c == '}') depth--;
        }

        // One level for the class and one for the method; the rest are if and else blocks.
        Assert.Equal(15, deepest - 2);
    }

    [Fact(DisplayName = "it can give 43 different results")]
    public void FortyThreeResults()
    {
        var returns = Regex.Matches(Code(), "return \"(\\w+)\"");
        var results = returns.Select(m => m.Groups[1].Value).ToHashSet();

        Assert.Equal(43, results.Count);
        Assert.Equal(43, returns.Count);     // every return gives a different result
    }

    [Fact(DisplayName = "the three users in Main get the answers Main prints")]
    public void MainsAnswers()
    {
        var processor = new UserProcessor();

        Assert.Equal("NULL_USER_ACCESS_DENIED",
            processor.ProcessUser(null, false, "TRIAL", 1, false, "IT", true));
        Assert.Equal("ACCESS_DENIED_TOO_YOUNG",
            processor.ProcessUser(new User(), false, "TRIAL", 1, false, "IT", true));
        Assert.Equal("TRIAL_EXPIRED_UPGRADE_REQUIRED",
            processor.ProcessUser(new Main.AdultUser(), false, "TRIAL", 1, false, "IT", true));
    }

    /// <summary>
    /// <c>CallerFilePath</c> is filled in at the call site, so it is captured here, in this
    /// file, and the library's source is found from it.
    /// </summary>
    private static string SourceFile() =>
        Path.GetFullPath(Path.Combine(Path.GetDirectoryName(ThisFile())!,
            "..", "..", "..", "..", "src", "Introduction", "Problems", "ComplexIfs",
            "UserProcessor.cs"));

    private static string ThisFile([CallerFilePath] string path = "") => path;
}
