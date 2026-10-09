using System.Reflection;
using Xunit;
using F = global::dev.kaldiroglu.Visitor.File.Pattern1;

namespace dev.kaldiroglu.Visitor.Tests.File;

/// <summary>
/// Text files and XML files: type tests in FileOperator, then a visitor. The checks are
/// random, so these tests use a visitor that always answers the same way.
/// </summary>
public class FileTests
{
    private const string Source = "File/";

    /// <summary>A visitor with a fixed answer for each kind of file.</summary>
    private sealed record Fixed(bool Text, bool Xml) : F.IVisitor
    {
        public bool Visit(F.TextFile file) => Text;

        public bool Visit(F.XMLFile file) => Xml;
    }

    private static IEnumerable<string> DeclaredMethodNames(Type type) =>
        type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
                        BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Select(m => m.Name);

    [Fact]
    public void AcceptReturnsTheVisitorsAnswer()
    {
        F.File text = new F.TextFile("a.txt");
        F.File xml = new F.XMLFile("b.xml");

        Assert.True(text.Accept(new Fixed(true, false)));
        Assert.False(xml.Accept(new Fixed(true, false)));
        Assert.False(text.Accept(new Fixed(false, true)));
        Assert.True(xml.Accept(new Fixed(false, true)));
    }

    [Fact]
    public void ReadOnlyWhenAllowed()
    {
        F.File text = new F.TextFile("a.txt");
        F.IVisitor refuses = new Fixed(false, false);

        var lines = Printed.By(() =>
        {
            text.Open();
            if (text.Accept(refuses))
            {
                text.Read();
            }

            text.Close();
        });

        Assert.Equal(["", "Opening the file: a.txt", "Closing the file: a.txt"], lines);
    }

    [Fact]
    public void TheChecksMovedToTheVisitor()
    {
        foreach (var file in new[] { typeof(F.TextFile), typeof(F.XMLFile) })
        {
            var names = DeclaredMethodNames(file).ToList();
            Assert.False(names.Contains("CheckFormat"), file.Name);
            Assert.False(names.Contains("Validate"), file.Name);
        }

        Assert.Equal(2, typeof(F.IVisitor).GetMethods().Length);
        Assert.Equal(2, DeclaredMethodNames(typeof(F.FileVisitor)).Count());
    }

    [Fact]
    public void TypeTestsAndCasts()
    {
        // Java: "instanceof XMLFile" and "(TextFile) aFile". C#: "is XMLFile" and "(TextFile)aFile".
        var operatorCode = Printed.CodeOf(Source + "Problem2/FileOperator.cs");
        Assert.Contains("aFile is XMLFile", operatorCode);
        Assert.Contains("(TextFile)aFile", operatorCode);

        // The visitor prints sentences such as "It is a valid XML file", so strings are removed first.
        var visitor = Printed.WithoutStrings(Printed.CodeOf(Source + "Pattern1/FileVisitor.cs"));
        Assert.Equal(0, Printed.CountOf(visitor, @"\bis\b"));
    }

    [Fact]
    public void FourTimesInFive()
    {
        var visitor = Printed.CodeOf(Source + "Pattern1/FileVisitor.cs");
        Assert.Equal(2, Printed.CountOf(visitor, @"random < 0\.80"));
    }
}
