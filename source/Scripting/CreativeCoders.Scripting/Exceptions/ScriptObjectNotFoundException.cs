using System;

namespace CreativeCoders.Scripting.Exceptions
{
    public class ScriptObjectNotFoundException : ScriptingException
    {
        // ReSharper disable once SuggestBaseTypeForParameter
        public ScriptObjectNotFoundException(Type scriptObjectType) : base(
            $"Script object was not found: '{scriptObjectType?.Name}'") { }
    }
}