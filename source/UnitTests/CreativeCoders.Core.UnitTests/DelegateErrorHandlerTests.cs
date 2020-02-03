using System;
using Xunit;

namespace CreativeCoders.Core.UnitTests
{
    public class DelegateErrorHandlerTests
    {
        [Fact]
        public void Ctor_ActionParameterIsNull_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => new DelegateErrorHandler(null));
        }

        [Fact]
        public void HandleException_()
        {
            Exception exception = null;
            
            var errorHandler = new DelegateErrorHandler(e => exception = e);
            
            var newException = new ApplicationException("Test Exception");
            
            errorHandler.HandleException(newException);
            
            Assert.Same(newException, exception);
        }
    }
}