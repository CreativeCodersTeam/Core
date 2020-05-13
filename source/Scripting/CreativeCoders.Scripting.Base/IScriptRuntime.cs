namespace CreativeCoders.Scripting.Base
{
    public interface IScriptRuntime
    {
        IScriptRuntimeSpace CreateSpace(string nameSpace);
    }
}