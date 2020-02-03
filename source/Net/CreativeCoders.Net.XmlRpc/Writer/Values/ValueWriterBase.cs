using System;
using System.Xml.Linq;
using CreativeCoders.Net.XmlRpc.Model;

namespace CreativeCoders.Net.XmlRpc.Writer.Values
{
    public abstract class ValueWriterBase : IValueWriter
    {
        public abstract void WriteTo(XElement paramNode, XmlRpcValue xmlRpcValue);

        public abstract bool HandlesType(Type valueType);
    }

    public abstract class ValueWriterBase<T> : ValueWriterBase
    {
        private readonly string _valueElementName;

        private readonly Func<XmlRpcValue<T>, string> _getValue;

        private readonly Action<XmlRpcValue<T>, XElement> _writeToValueElement;

        protected ValueWriterBase(string valueElementName, Func<XmlRpcValue<T>, string> getValue)
        {
            _valueElementName = valueElementName;
            _getValue = getValue;
        }

        protected ValueWriterBase(string valueElementName, Action<XmlRpcValue<T>, XElement> writeToValueElement)
        {
            _valueElementName = valueElementName;
            _writeToValueElement = writeToValueElement;
        }

        public override void WriteTo(XElement paramNode, XmlRpcValue xmlRpcValue)
        {
            var valueElement = new XElement(XmlRpcTags.Value);
            paramNode.Add(valueElement);

            var valueDataElement = new XElement(_valueElementName);

            if (_getValue != null)
            {
                valueDataElement.Add(_getValue((XmlRpcValue<T>)xmlRpcValue));
            }
            else
            {
                _writeToValueElement?.Invoke((XmlRpcValue<T>) xmlRpcValue, valueDataElement);
            }

            valueElement.Add(valueDataElement);
        }

        public override bool HandlesType(Type valueType)
        {
            return typeof(XmlRpcValue<T>).IsAssignableFrom(valueType);
        }
    }
}