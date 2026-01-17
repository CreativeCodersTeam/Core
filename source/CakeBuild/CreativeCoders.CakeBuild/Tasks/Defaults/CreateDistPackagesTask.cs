using Cake.Frosting;
using CreativeCoders.CakeBuild.Tasks.Templates;
using CreativeCoders.IO.Archives;

namespace CreativeCoders.CakeBuild.Tasks.Defaults;

[TaskName("CreateDistPackages")]
public class CreateDistPackagesTask(IArchiveWriterFactory archiveWriterFactory)
    : CreateDistPackagesTask<CakeBuildContext>(archiveWriterFactory);
