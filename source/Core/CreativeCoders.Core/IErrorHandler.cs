using System;

namespace CreativeCoders.Core
{
    public interface IErrorHandler
    {
        void HandleException(Exception exception);
    }
}