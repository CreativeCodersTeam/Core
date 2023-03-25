using Nuke.Common;
using Nuke.Common.ProjectModel;

namespace CreativeCoders.NukeBuild.Components.Parameters;

public interface ISolutionParameter : INukeBuild
{
    [Solution]
    [Required]
    Solution Solution => TryGetValue(() => Solution) ?? throw new ArgumentNullException(nameof(Solution));
}
