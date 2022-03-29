namespace CreativeCoders.Config.Base;

public interface ISetting<out T>
    where T : class
{
    T Value { get; }
}