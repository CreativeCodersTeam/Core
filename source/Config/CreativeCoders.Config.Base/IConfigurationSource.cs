namespace CreativeCoders.Config.Base;

public interface IConfigurationSource
{
    object GetSettingObject();

    object GetDefaultSettingObject();
}
