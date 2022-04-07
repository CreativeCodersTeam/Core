namespace CreativeCoders.Config.Base;

public interface ISettingsTransient<out T> : ISettings<T>
    where T : class { }
