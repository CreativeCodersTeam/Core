using CreativeCoders.CakeBuild.Tasks.Templates.Settings;

namespace CreativeCoders.CakeBuild.Tasks.Defaults;

public interface IDefaultTaskSettings :
    ICleanTaskSettings,
    ITestTaskSettings,
    ICodeCoverageTaskSettings,
    IPackTaskSettings,
    INuGetPushTaskSettings,
    IPublishTaskSettings;
