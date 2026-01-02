using Cake.Frosting;
using CreativeCoders.CakeBuild.Tasks.Templates;

namespace CreativeCoders.CakeBuild.Tasks.Defaults;

[TaskName("Build")]
[IsDependentOn(typeof(RestoreTask))]
public class BuildTask() : BuildTask<BuildContext>();
