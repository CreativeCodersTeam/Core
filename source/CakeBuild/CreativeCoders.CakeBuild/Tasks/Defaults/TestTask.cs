using Cake.Frosting;
using CreativeCoders.CakeBuild.Tasks.Templates;

namespace CreativeCoders.CakeBuild.Tasks.Defaults;

[TaskName("Test")]
[IsDependentOn(typeof(BuildTask))]
public class TestTask : TestTask<CakeBuildContext>;
