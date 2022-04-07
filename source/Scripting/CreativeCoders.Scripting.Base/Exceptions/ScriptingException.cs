using System;

namespace CreativeCoders.Scripting.Base.Exceptions;

public abstract class ScriptingException : Exception
{
    protected ScriptingException(string message) : base(message) { }
}
