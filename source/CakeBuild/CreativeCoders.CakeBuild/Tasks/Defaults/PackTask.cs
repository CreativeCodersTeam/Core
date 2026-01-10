using Cake.Frosting;
using CreativeCoders.CakeBuild.Tasks.Templates;

namespace CreativeCoders.CakeBuild.Tasks.Defaults;

[TaskName("Pack")]
[IsDependentOn(typeof(CodeCoverageTask))]
public class PackTask : PackTask<CakeBuildContext>;
