using Cake.Frosting;
using CreativeCoders.CakeBuild.Tasks.Templates;

namespace CreativeCoders.CakeBuild.Tasks.Defaults;

[TaskName("CodeCoverage")]
[IsDependentOn(typeof(TestTask))]
public class CodeCoverageTask : CodeCoverageTask<CakeBuildContext>;
