namespace CreativeCoders.Scripting.Exceptions
{
    public class ScriptEntryPointNotFoundException : ScriptingException
    {
        public ScriptEntryPointNotFoundException(string entryPointName) : base(
            $"Script execution entry point not found: '{entryPointName}'") { }
    }
}