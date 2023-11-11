using CreativeCoders.Net.JsonRpc.ApiBuilder;

namespace CreativeCoders.Net.UnitTests.JsonRpc.TestData;

[JsonRpcApi(IncludeParameterNames = true)]
public interface ITestJsonRpcApiWithParamNames : ITestJsonRpcApi
{

}
