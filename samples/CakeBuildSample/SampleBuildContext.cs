using Cake.Core;
using Cake.Core.IO;
using CreativeCoders.CakeBuild;
using CreativeCoders.CakeBuild.Tasks.Defaults;
using CreativeCoders.CakeBuild.Tasks.Templates.Settings;
using CreativeCoders.Core;
using CreativeCoders.Core.Collections;

namespace CakeBuildSample;

public class SampleBuildContext : BuildContext, IDefaultTaskSettings
{
    public SampleBuildContext(ICakeContext context) : base(context) { }

    IList<DirectoryPath> ICleanTaskSettings.DirectoriesToClean =>
        this.CastAs<ICleanTaskSettings>().DefaultDirectoriesToClean.AddRange(RootDir.Combine(".tests"));
}
