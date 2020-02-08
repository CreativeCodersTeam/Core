using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using CreativeCoders.Net.XmlRpc.Model.Values;
using CreativeCoders.Net.XmlRpc.Reader;
using FakeItEasy;
using Xunit;

namespace CreativeCoders.Net.UnitTests.XmlRpc.Reader
{
    public class XmlRpcResponseModelReaderTests
    {
        private const string XmlRpcResponseData =
            "<?xml version=\"1.0\" encoding=\"utf-8\"?><methodResponse><params><param><value><string>TestResult Value</string></value></param></params></methodResponse>";

        [Fact]
        public async void Read_ValidResponse_ReturnsCorrectModel()
        {
            var stringValue = new StringValue("Test1234");
            var readers = A.Fake<IValueReaders>();
            A.CallTo(() => readers.ReadValue(A<XElement>.Ignored)).Returns(stringValue);

            var reader = new ResponseModelReader(readers);

            var model = await reader.ReadAsync(new MemoryStream(Encoding.UTF8.GetBytes(XmlRpcResponseData)), false);

            Assert.Single(model.Results);
            var result = model.Results.First();
            Assert.Single(result.Values);
            Assert.Same(stringValue, result.Values.First());
        }
    }
}