namespace CreativeCoders.Core.UnitTests.Reflection;

public interface IReflectionTestObject
{
    void Execute();

    void ExecuteEx();

    void ExecuteEx(int value);

    void ExecuteEx(int value, string text);

    void ExecuteEx(int value, string text, bool condition);

    int Calculate();

    int Calculate(int value);

    int Calculate(int value, string text);
}
