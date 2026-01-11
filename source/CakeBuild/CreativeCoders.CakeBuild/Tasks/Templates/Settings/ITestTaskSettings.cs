using Cake.Common.Solution;
using Cake.Core.IO;

namespace CreativeCoders.CakeBuild.Tasks.Templates.Settings;

[CakeTaskSettings]
public interface ITestTaskSettings : IBuildContextAccessor
{
    IEnumerable<FilePath> TestProjects
    {
        get
        {
            if (Context.SolutionFile.GetExtension()?.ToLower() != ".sln")
            {
                // Currently no support for new XML based slnx solution file format
                return [];
            }

            var solution = Context.ParseSolution(Context.SolutionFile);

            var testProjects = solution.Projects
                .Where(p =>
                    p.Path.MakeAbsolute(Context.Environment).FullPath.StartsWith(TestSourceDir.FullPath,
                        StringComparison.InvariantCulture));

            return testProjects
                .Select(x => x.Path)
                .Where(x => x.GetExtension()?.ToLower() == ".csproj");
        }
    }

    bool GenerateCoverageReport => true;

    DirectoryPath TestSourceDir => Context.RootDir.Combine("tests");
}
