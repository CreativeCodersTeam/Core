using JetBrains.Annotations;

namespace CreativeCoders.Core.UnitTests.Reflection;

[PublicAPI]
[DummyTest(Value = 12345)]
public class GenericClass<T>
{
    public GenericClass() : this(default)
    {
            
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public GenericClass(T data)
    {
        Data = data;
    }

    public T Data { get; set; }
}