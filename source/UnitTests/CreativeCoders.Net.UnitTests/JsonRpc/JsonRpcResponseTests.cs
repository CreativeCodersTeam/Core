using CreativeCoders.Net.JsonRpc;
using FluentAssertions;
using Xunit;

namespace CreativeCoders.Net.UnitTests.JsonRpc;

public class JsonRpcResponseTests
{
    [Fact]
    public void EnsureSuccess_WithNullError_NoExceptionThrown()
    {
        // Arrange
        var response = new JsonRpcResponse<string> { Error = null };

        // Act & Assert
        response
            .Invoking(r => r.EnsureSuccess("TestMethod"))
            .Should()
            .NotThrow();
    }

    [Fact]
    public void EnsureSuccess_WithError_ExceptionThrown()
    {
        const int errorCode = 500;
        const string errorMessage = "Internal Server Error";
        const string methodName = "TestMethod";

        // Arrange
        var response = new JsonRpcResponse<string> { Error = new JsonRpcError { Code = errorCode, Message = errorMessage } };

        // Act & Assert
        response
            .Invoking(r => r.EnsureSuccess(methodName))
            .Should().Throw<JsonRpcCallException>()
            .Where(ex =>
                ex.ErrorCode == errorCode &&
                ex.RpcMethodName == methodName &&
                ex.ErrorMessage == errorMessage);
    }
}
