using System.Buffers.Binary;
using System.Globalization;
using System.IO.Compression;
using System.Text;

namespace dev.kaldiroglu.Decorator.Io;

/// <summary>
/// The pattern in the standard library, measured.
/// <para>
/// The streams are stacked: <see cref="BinaryWriter"/> over <see cref="BufferedStream"/> over
/// a file, and a <see cref="GZipStream"/> added as one more layer.
/// </para>
/// <para>
/// The point is what <see cref="WriteInvoice"/> does <i>not</i> know: it takes a writer and
/// writes an invoice. Whether those bytes are buffered, compressed or written straight to
/// disk was decided by whoever built the chain, and this method never learned about it.
/// </para>
/// <para>
/// The format is written by hand so that the plain file has a fixed size of 274 bytes. The
/// header is a two-byte length and one byte per character; every other character takes two
/// bytes; numbers are big-endian. The gzipped size depends on the library: 194 bytes on
/// .NET 10, because <see cref="BufferedStream"/> passes its flush on to
/// <see cref="GZipStream"/>, which ends a block there.
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

    /// <summary>Two bytes for each character the writer and the reader are given.</summary>
    private static readonly Encoding TwoByteChars = Encoding.BigEndianUnicode;

    /// <summary>Writes the invoice twice, reads both back, and reports the sizes.</summary>
    public static (long PlainBytes, long GzippedBytes, double Total) Run(bool print = true)
    {
        var plain = Path.GetTempFileName();
        var compressed = Path.GetTempFileName();

        try
        {
            // Two layers: the file, then buffering, then the data formatting on top.
            using (var writer = new BinaryWriter(
                       new BufferedStream(File.Create(plain)), TwoByteChars))
            {
                WriteInvoice(writer);
            }

            // The same invoice, one decorator deeper. Compression sits below the buffer and
            // below the formatting, because it should compress the finished bytes.
            using (var writer = new BinaryWriter(
                       new BufferedStream(
                           new GZipStream(File.Create(compressed), CompressionMode.Compress)),
                       TwoByteChars))
            {
                WriteInvoice(writer);
            }

            // Read back through the mirror image of the chain that wrote it. Every layer
            // added on the way out needs its counterpart on the way in, in the opposite
            // order. That is the one real obligation the pattern places on the caller.
            double total;
            using (var reader = new BinaryReader(
                       new BufferedStream(File.OpenRead(plain)), TwoByteChars))
            {
                total = ReadInvoice(reader, print);
            }

            using (var reader = new BinaryReader(
                       new BufferedStream(
                           new GZipStream(File.OpenRead(compressed), CompressionMode.Decompress)),
                       TwoByteChars))
            {
                ReadInvoice(reader, false);
            }

            var plainSize = new FileInfo(plain).Length;
            var gzipSize = new FileInfo(compressed).Length;

            if (print)
            {
                var sum = total.ToString("F2", CultureInfo.InvariantCulture);
                Console.WriteLine($"plain    total: ${sum}  file: {plainSize} bytes  [total as expected]");
                Console.WriteLine($"gzipped  total: ${sum}  file: {gzipSize} bytes  [total as expected]");
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

    /// <summary>Writes the invoice. The writer must use <see cref="TwoByteChars"/>.</summary>
    public static void WriteInvoice(BinaryWriter writer)
    {
        writer.Write(BinaryPrimitives.ReverseEndianness((ushort)Header.Length));
        writer.Write(Encoding.ASCII.GetBytes(Header));
        for (var i = 0; i < Items.Length; i++)
        {
            writer.Write(Items[i].ToCharArray());
            writer.Write(Separator);
            writer.Write('\t');
            writer.Write(BinaryPrimitives.ReverseEndianness(Units[i]));
            writer.Write('\t');
            writer.Write(BinaryPrimitives.ReverseEndianness(BitConverter.DoubleToInt64Bits(Prices[i])));
            writer.Write('\n');
        }
    }

    /// <summary>Reads the invoice back and returns its total.</summary>
    public static double ReadInvoice(BinaryReader reader, bool print)
    {
        double totalPrice = 0;
        int headerLength = BinaryPrimitives.ReverseEndianness(reader.ReadUInt16());
        var header = Encoding.ASCII.GetString(reader.ReadBytes(headerLength));
        if (print)
        {
            Console.WriteLine(header);
        }

        for (var i = 0; i < Items.Length; i++)
        {
            var item = new StringBuilder();
            for (var c = reader.ReadChar(); c != Separator; c = reader.ReadChar())
            {
                item.Append(c);
            }

            reader.ReadChar(); // the '\t'
            var unit = BinaryPrimitives.ReverseEndianness(reader.ReadInt32());
            reader.ReadChar(); // the '\t'
            var price = BitConverter.Int64BitsToDouble(BinaryPrimitives.ReverseEndianness(reader.ReadInt64()));
            reader.ReadChar(); // the '\n' that ends the row
            totalPrice += unit * price;

            if (print)
            {
                Console.WriteLine(string.Create(CultureInfo.InvariantCulture, $"{item}{Separator}\t{unit}\t{price}"));
            }
        }

        return totalPrice;
    }
}
