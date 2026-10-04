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

    /// <summary>
    /// Gets a value that indicates whether <c>dotnet test</c> is run in Microsoft.Testing.Platform (MTP) mode
    /// instead of VSTest mode.
    /// </summary>
    /// <value>
    /// <see langword="true" /> to pass Microsoft.Testing.Platform arguments to <c>dotnet test</c>; otherwise,
    /// <see langword="false" /> to pass VSTest arguments. The default is <see langword="false" />.
    /// </value>
    /// <remarks>
    /// This setting only selects the arguments passed to <c>dotnet test</c>; it does not switch the test mode of the
    /// .NET SDK. The repository must enable the Microsoft.Testing.Platform mode in its <c>global.json</c>
    /// (<c>"test": { "runner": "Microsoft.Testing.Platform" }</c>). The test projects must reference
    /// <c>Microsoft.Testing.Extensions.TrxReport</c> and, if <see cref="GenerateCoverageReport" /> is
    /// <see langword="true" />, <c>Microsoft.Testing.Extensions.CodeCoverage</c>. In this mode, the TRX file
    /// (<c>&lt;Project&gt;.trx</c>) is written to <see cref="ICakeBuildContext.CodeCoverageDir" /> instead of
    /// <see cref="ICakeBuildContext.TestResultsDir" />, and the coverage report is written in Cobertura format to
    /// <c>&lt;Project&gt;.cobertura.xml</c> in <see cref="ICakeBuildContext.CodeCoverageDir" />. Because these files are
    /// named after the test project file, the file names of all test projects must be unique.
    /// </remarks>
    bool UseMicrosoftTestingPlatform => false;

    DirectoryPath TestSourceDir => Context.RootDir.Combine("tests");
}
