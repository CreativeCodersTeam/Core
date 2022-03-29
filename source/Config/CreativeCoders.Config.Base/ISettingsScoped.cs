namespace CreativeCoders.Config.Base;

public interface ISettingsScoped<out T> : ISettings<T>
    where T : class
{
        
}