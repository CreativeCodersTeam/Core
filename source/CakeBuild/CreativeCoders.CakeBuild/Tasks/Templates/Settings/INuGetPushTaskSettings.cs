namespace CreativeCoders.CakeBuild.Tasks.Templates.Settings;

[CakeTaskSettings]
public interface INuGetPushTaskSettings : IBuildContextAccessor
{
    string NuGetFeedUrl => "nuget.org";

    string NuGetApiKey => Context.Environment.GetEnvironmentVariable("NUGET_TOKEN");

    bool SkipPush => false;
}
