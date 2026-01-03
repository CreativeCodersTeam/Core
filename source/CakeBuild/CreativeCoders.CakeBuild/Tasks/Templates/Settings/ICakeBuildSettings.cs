using Cake.Core;

namespace CreativeCoders.CakeBuild.Tasks.Templates.Settings;

public interface ICakeBuildSettings
{
    ICakeContext Context { get; }
}
