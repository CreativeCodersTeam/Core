using Cake.Core;
using Cake.Core.IO;
using CreativeCoders.CakeBuild.Tasks.Defaults;
using CreativeCoders.CakeBuild.Tasks.Templates.Settings;

namespace CakeBuildSample;

public class BuildSetup(ICakeContext context) : IDefaultBuildSetup
{
    public ICakeContext Context { get; } = context;
}
