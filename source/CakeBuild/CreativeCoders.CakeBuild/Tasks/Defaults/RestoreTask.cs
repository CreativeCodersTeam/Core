using Cake.Frosting;
using CreativeCoders.CakeBuild.Tasks.Templates;

namespace CreativeCoders.CakeBuild.Tasks.Defaults;

[TaskName("Restore")]
[IsDependentOn(typeof(CleanTask))]
public class RestoreTask : RestoreTask<CakeBuildContext>;
