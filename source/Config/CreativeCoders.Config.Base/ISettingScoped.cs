namespace CreativeCoders.Config.Base
{
    public interface ISettingScoped<out T> : ISetting<T>
        where T : class
    {
        
    }
}