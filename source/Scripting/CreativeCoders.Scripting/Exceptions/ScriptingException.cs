using System;
using System.Runtime.Serialization;

namespace CreativeCoders.Scripting.Exceptions
{
    public abstract class ScriptingException : Exception
    {
        protected ScriptingException() { }

        protected ScriptingException(string message) : base(message) { }

        protected ScriptingException(string message, Exception innerException) : base(message, innerException) { }

        protected ScriptingException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}