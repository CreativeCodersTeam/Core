using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CreativeCoders.Core.IO
{
    public static class StreamExtensions
    {
        public static string ReadAsString(this Stream stream)
        {
            using var streamReader = new StreamReader(stream);
            return streamReader.ReadToEnd();
        }

        public static string ReadAsString(this Stream stream, Encoding encoding)
        {
            using var streamReader = new StreamReader(stream, encoding);
            return streamReader.ReadToEnd();
        }

        public static string ReadAsString(this Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks)
        {
            using var streamReader = new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks);
            return streamReader.ReadToEnd();
        }

        public static Task<string> ReadAsStringAsync(this Stream stream)
        {
            using var streamReader = new StreamReader(stream);
            return streamReader.ReadToEndAsync();
        }

        public static Task<string> ReadAsStringAsync(this Stream stream, Encoding encoding)
        {
            using var streamReader = new StreamReader(stream, encoding);
            return streamReader.ReadToEndAsync();
        }

        public static Task<string> ReadAsStringAsync(this Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks)
        {
            using var streamReader = new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks);
            return streamReader.ReadToEndAsync();
        }
    }
}