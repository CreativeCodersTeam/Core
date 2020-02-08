using System.Collections.Generic;
using CreativeCoders.Core;
using CreativeCoders.Net.XmlRpc.Model;

namespace CreativeCoders.Net.XmlRpc
{
    public class XmlRpcResponse
    {        
        private readonly IList<XmlRpcMethodResult> _results;

        public XmlRpcResponse(IEnumerable<XmlRpcMethodResult> results, bool isMultiCall)
        {            
            Ensure.IsNotNull(results, nameof(results));

            _results = new List<XmlRpcMethodResult>(results);
            IsMultiCall = isMultiCall;
        }

        public IEnumerable<XmlRpcMethodResult> Results => _results;

        public bool IsMultiCall { get; }
    }
}