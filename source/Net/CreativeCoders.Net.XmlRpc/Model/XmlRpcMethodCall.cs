using System.Collections.Generic;
using CreativeCoders.Core;

namespace CreativeCoders.Net.XmlRpc.Model
{
    public class XmlRpcMethodCall
    {
        private readonly IList<XmlRpcValue> _parameters;

        public XmlRpcMethodCall(string name, params XmlRpcValue[] parameters)
        {
            Ensure.IsNotNullOrWhitespace(name, nameof(name));

            Name = name;
            
            _parameters = new List<XmlRpcValue>(parameters);
        }

        public void AddParameter(XmlRpcValue parameter)
        {
            _parameters.Add(parameter);
        }

        public string Name { get; }

        public IEnumerable<XmlRpcValue> Parameters => _parameters;
    }
}