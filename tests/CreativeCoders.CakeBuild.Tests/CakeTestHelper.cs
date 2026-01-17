using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;
using FakeItEasy;

namespace CreativeCoders.CakeBuild.Tests;

public static class CakeTestHelper
{
    public static ICakeContext CreateCakeContext(string toolName = "/bin/tool/")
    {
        var context = A.Fake<ICakeContext>();
        var environment = A.Fake<ICakeEnvironment>();
        var fileSystem = A.Fake<IFileSystem>();
        var log = A.Fake<ICakeLog>();
        var arguments = A.Fake<ICakeArguments>();
        var fakeProcessRunner = A.Fake<IProcessRunner>();
        var globber = A.Fake<IGlobber>();
        var toolLocator = A.Fake<IToolLocator>();

        A.CallTo(() => context.Environment).Returns(environment);
        A.CallTo(() => context.FileSystem).Returns(fileSystem);
        A.CallTo(() => context.Log).Returns(log);
        A.CallTo(() => context.Arguments).Returns(arguments);
        A.CallTo(() => context.ProcessRunner).Returns(fakeProcessRunner);
        A.CallTo(() => context.Globber).Returns(globber);
        A.CallTo(() => context.Tools).Returns(toolLocator);

        A.CallTo(() => toolLocator.Resolve(A<string>._))
            .Returns(new FilePath(toolName));

        A.CallTo(() => toolLocator.Resolve(A<IEnumerable<string>>._))
            .Returns(new FilePath(toolName));

        A.CallTo(() => globber.Match(A<GlobPattern>._, A<GlobberSettings>._)).Returns([
            new FilePath("/repo/coverage.xml")
        ]);

        var toolFile = A.Fake<IFile>();
        A.CallTo(() => toolFile.Exists).Returns(true);
        A.CallTo(() => fileSystem.GetFile(A<FilePath>.That.Matches(x => x.FullPath == toolName)))
            .Returns(toolFile);

        return context;
    }

    public static void SetupFileSystem(ICakeContext context, string workingDir, string solutionFile)
    {
        var repoDir = new DirectoryPath(workingDir);
        A.CallTo(() => context.Environment.WorkingDirectory).Returns(repoDir);

        var fileSystem = context.FileSystem;
        var gitDir = A.Fake<IDirectory>();
        A.CallTo(() => gitDir.Exists).Returns(true);
        A.CallTo(() =>
                fileSystem.GetDirectory(A<DirectoryPath>.That.Matches(x => x.FullPath.EndsWith(".git"))))
            .Returns(gitDir);

        A.CallTo(() => context.Arguments.HasArgument("solution")).Returns(true);
        A.CallTo(() => context.Arguments.GetArguments("solution")).Returns(new[] { solutionFile });
    }
}
