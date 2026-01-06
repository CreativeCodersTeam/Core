using CreativeCoders.Core.SysEnvironment;

namespace CreativeCoders.CakeBuild.Tasks.Templates.Settings;

[CakeTaskSettings]
public interface ICodeCoverageTaskSettings
{
    string Framework => $"net{Env.Version.Major}.0";
}
