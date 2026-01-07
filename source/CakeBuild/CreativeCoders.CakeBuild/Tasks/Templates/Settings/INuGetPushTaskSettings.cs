namespace CreativeCoders.CakeBuild.Tasks.Templates.Settings;

[CakeTaskSettings]
public interface INuGetPushTaskSettings
{
    string NuGetFeedUrl => string.Empty;

    string NuGetApiKey => string.Empty;

    bool SkipPush => false;
}
