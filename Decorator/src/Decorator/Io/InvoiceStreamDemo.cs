using System.IO.Compression;
using System.Text;

namespace dev.kaldiroglu.Decorator.Io;

/// <summary>
/// The pattern in the standard library, measured.
/// <para>
/// Java's version stacks <c>DataOutputStream</c> over <c>BufferedOutputStream</c> over
/// <c>FileStream</c>, and again with a <c>GZIPOutputStream</c> in the middle. .NET's
/// shapes are the same: <see cref="BinaryWriter"/> over <see cref="BufferedStream"/> over
/// a file, and a <see cref="GZipStream"/> added as one more layer.
/// </para>
/// <para>
/// The point is what <see cref="WriteInvoice"/> does <i>not</i> know: it takes a writer and
/// writes an invoice. Whether those bytes are buffered, compressed or written straight to
/// disk was decided by whoever built the chain, and this method never learned about it.
/// </para>
/// </summary>
public static class InvoiceStreamDemo
{
    private static readonly string[] Items =
        ["Thinking in Java", "JSF Applied", "Java Tutorial", "Java Security", "Swing Programming"];

    private static readonly double[] Prices = [34.99, 29.99, 35.99, 32.99, 40.99];
    private static readonly int[] Units = [2, 3, 6, 2, 5];

    private const char Separator = ':';
    private const string Header = "   - - - I N V O I C E - - -    ";

    public const double ExpectedTotal = 646.82;

    /// <summary>Writes the invoice twice, reads both back, and reports the sizes.</summary>
    public static (long PlainBytes, long GzippedBytes, double Total) Run(bool print = true)
    {
        var plain = Path.GetTempFileName();
        var compressed = Path.GetTempFileName();

        try
        {
            // Two layers: the file, then buffering, then the data formatting on top.
            using (var writer = new BinaryWriter(
                       new BufferedStream(File.Create(plain)), Encoding.UTF8))
            {
                WriteInvoice(writer);
            }

            // The same invoice, one decorator deeper. Compression sits below the buffer and
            // below the formatting, because it should compress the finished bytes.
            using (var writer = new BinaryWriter(
                       new BufferedStream(
                           new GZipStream(File.Create(compressed), CompressionMode.Compress)),
                       Encoding.UTF8))
            {
                WriteInvoice(writer);
            }

            // Read back through the mirror image of the chain that wrote it. Every layer
            // added on the way out needs its counterpart on the way in, in the opposite
            // order. That is the one real obligation the pattern places on the caller.
            double total;
            using (var reader = new BinaryReader(
                       new BufferedStream(File.OpenRead(plain)), Encoding.UTF8))
            {
                total = ReadInvoice(reader, print);
            }

            using (var reader = new BinaryReader(
                       new BufferedStream(
                           new GZipStream(File.OpenRead(compressed), CompressionMode.Decompress)),
                       Encoding.UTF8))
            {
                ReadInvoice(reader, false);
            }

            var plainSize = new FileInfo(plain).Length;
            var gzipSize = new FileInfo(compressed).Length;

            if (print)
            {
                Console.WriteLine($"plain    total: ${total:F2}  file: {plainSize} bytes");
                Console.WriteLine($"gzipped  total: ${total:F2}  file: {gzipSize} bytes");
                Console.WriteLine($"Compression saved {plainSize - gzipSize} bytes, "
                                  + "and WriteInvoice() never learned about it.");
            }

            return (plainSize, gzipSize, total);
        }
        finally
        {
            File.Delete(plain);
            File.Delete(compressed);
        }
    }

    public static void WriteInvoice(BinaryWriter writer)
    {
        writer.Write(Header);
        for (var i = 0; i < Items.Length; i++)
        {
            writer.Write(Items[i]);
            writer.Write(Separator);
            writer.Write('\t');
            writer.Write(Units[i]);
            writer.Write('\t');
            writer.Write(Prices[i]);
            writer.Write('\n');
        }
    }

    public static double ReadInvoice(BinaryReader reader, bool print)
    {
        double totalPrice = 0;
        var header = reader.ReadString();
        if (print)
        {
            Console.WriteLine(header);
        }

        for (var i = 0; i < Items.Length; i++)
        {
            var item = reader.ReadString();
            reader.ReadChar(); // the ':'
            reader.ReadChar(); // the '\t'
            var unit = reader.ReadInt32();
            reader.ReadChar(); // the '\t'
            var price = reader.ReadDouble();
            reader.ReadChar(); // the '\n' that ends the row
            totalPrice += unit * price;

            if (print)
            {
                Console.WriteLine($"{item}{Separator}\t{unit}\t{price}");
            }
        }

        return totalPrice;
    }
}
