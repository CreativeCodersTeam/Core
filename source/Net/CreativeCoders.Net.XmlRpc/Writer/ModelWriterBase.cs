using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using CreativeCoders.Core;

namespace CreativeCoders.Net.XmlRpc.Writer
{
    public abstract class ModelWriterBase<T>
    {
        protected ModelWriterBase(IValueWriters writers)
        {
            Ensure.IsNotNull(writers, nameof(writers));

            Writers = writers;
        }

        public async Task WriteAsync(Stream outputStream, T data, Encoding encoding)
        {
            Ensure.IsNotNull(outputStream, nameof(outputStream));
            Ensure.IsNotNull(data, nameof(data));
            Ensure.IsNotNull(encoding, nameof(encoding));

            var xmlDoc = CreateXml(data, encoding);

            var writer = XmlWriter.Create(outputStream, new XmlWriterSettings { Async = true, Encoding = encoding });

            xmlDoc.WriteTo(writer);

            await writer.FlushAsync().ConfigureAwait(false);
        }

        public Task WriteAsync(Stream outputStream, T data)
        {
            return WriteAsync(outputStream, data, Encoding.GetEncoding("UTF-8"));
        }

        protected abstract XDocument CreateXml(T xmlRpcRequest, Encoding encoding);

        protected IValueWriters Writers { get; }
    }
}