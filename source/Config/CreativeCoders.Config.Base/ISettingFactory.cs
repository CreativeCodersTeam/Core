namespace CreativeCoders.Config.Base;

public interface ISettingFactory<out T>
    where T : class
{
    T Create();
}
