using Cake.Frosting;
using CreativeCoders.CakeBuild.Tasks.Templates;

namespace CreativeCoders.CakeBuild.Tasks.Defaults;

[TaskName("NuGetPublish")]
[IsDependentOn(typeof(PackTask))]
public class NuGetPushTask : NuGetPushTask<BuildContext> { }
