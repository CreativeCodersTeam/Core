namespace CreativeCoders.Config.Base;

public interface ISettingTransient<out T> : ISetting<T>
    where T : class { }
