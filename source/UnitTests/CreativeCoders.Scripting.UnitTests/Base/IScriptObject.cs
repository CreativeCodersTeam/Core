namespace CreativeCoders.Scripting.UnitTests.Base;

public interface IScriptObject
{
    void Add();

    void Add(int arg);

    void Add(int arg1, string arg2);

    void Add(int arg1, string arg2, bool arg3);
        
    void Add(int arg1, string arg2, bool arg3, object arg4);
        
    int Process();

    int Process(int arg);

    int Process(int arg1, string arg2);

    int Process(int arg1, string arg2, bool arg3);
        
    int Process(int arg1, string arg2, bool arg3, object arg4);
        
    int IntValue { get; set; }
}