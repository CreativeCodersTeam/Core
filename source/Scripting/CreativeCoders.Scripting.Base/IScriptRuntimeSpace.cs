namespace CreativeCoders.Scripting.Base;

public interface IScriptRuntimeSpace
{
    IScript Build(ScriptPackage scriptPackage);
}
