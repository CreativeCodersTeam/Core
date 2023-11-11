using System.Threading.Tasks;
using CreativeCoders.Net.JsonRpc;
using CreativeCoders.Net.JsonRpc.ApiBuilder;

namespace CreativeCoders.Net.UnitTests.JsonRpc.TestData;

[JsonRpcApi(IncludeParameterNames = false)]
public interface ITestJsonRpcApi
{
    Task<string> TestMethod(string arg1, int arg2);

    Task<JsonRpcResponse<string>> TestMethodWithJsonRpcResponse(string arg1, int arg2);

    [JsonRpcMethod("test")]
    Task<string> TestMethodWithNames([JsonRpcArgument("argument1")]string arg1, int arg2);

    string InvalidMethod();
}
