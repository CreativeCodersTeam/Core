namespace CreativeCoders.CakeBuild.Tasks.Templates.Settings;

[CakeTaskSettings]
public interface ITestTaskSettings
{
    IEnumerable<string> TestProjects { get; }
}
