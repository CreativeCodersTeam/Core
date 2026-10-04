using AwesomeAssertions;
using Cake.Common.Tools.DotNet.Test;
using Cake.Core;
using Cake.Core.IO;
using CreativeCoders.CakeBuild.Tasks.Templates;
using CreativeCoders.CakeBuild.Tasks.Templates.Settings;
using FakeItEasy;
using Xunit;

namespace CreativeCoders.CakeBuild.Tests.Tasks.Templates;

public class TestTaskTests
{
    private class TestTestBuildContext(ICakeContext context, bool generateCoverageReport = true)
        : CakeBuildContext(context), ITestTaskSettings
    {
        public IEnumerable<FilePath> TestProjects =>
            [new FilePath("/repo/tests/Project.Tests.csproj")];

        public bool GenerateCoverageReport { get; } = generateCoverageReport;
    }

    private class MtpTestBuildContext(
        ICakeContext context,
        bool generateCoverageReport = true,
        IEnumerable<FilePath>? testProjects = null)
        : CakeBuildContext(context), ITestTaskSettings
    {
        public IEnumerable<FilePath> TestProjects { get; } =
            testProjects ?? [new FilePath("/repo/tests/Project.Tests.csproj")];

        public bool GenerateCoverageReport { get; } = generateCoverageReport;

        public bool UseMicrosoftTestingPlatform => true;
    }

    private class FrameworkTestTask : TestTask<MtpTestBuildContext>
    {
        protected override void ApplyDotNetTestSettings(MtpTestBuildContext context,
            DotNetTestSettings dotNetBuildSettings)
        {
            dotNetBuildSettings.Framework = "net10.0";
        }
    }

    private class ArgumentCustomizationTestTask : TestTask<MtpTestBuildContext>
    {
        protected override void ApplyDotNetTestSettings(MtpTestBuildContext context,
            DotNetTestSettings dotNetBuildSettings)
        {
            dotNetBuildSettings.ArgumentCustomization = args => args.Append("--custom-argument");
        }
    }

    private static ICakeContext CreateCakeContext(List<string[]> capturedCalls)
    {
        var cakeContext = CakeTestHelper.CreateCakeContext("dotnet-test");
        CakeTestHelper.SetupFileSystem(cakeContext, "/repo", "/repo/test.sln");

        A.CallTo(() => cakeContext.ProcessRunner.Start("dotnet-test", A<ProcessSettings>._))
            .Invokes((FilePath _, ProcessSettings settings) =>
                capturedCalls.Add(settings.Arguments.Select(arg => arg.Render()).ToArray()));

        return cakeContext;
    }

    [Fact]
    public async Task RunAsync_ValidContext_RunsWithoutError()
    {
        // Arrange
        var cakeContext = CakeTestHelper.CreateCakeContext("dotnet-test");
        CakeTestHelper.SetupFileSystem(cakeContext, "/repo", "/repo/test.sln");

        var context = new TestTestBuildContext(cakeContext);
        var task = new TestTask<TestTestBuildContext>();

        // Act
        var act = () => task.RunAsync(context);

        // Assert
        await act
            .Should()
            .NotThrowAsync();

        A.CallTo(() => cakeContext.ProcessRunner.Start("dotnet-test",
                A<ProcessSettings>.That.Matches(x =>
                    x.Arguments.Select(arg => arg.Render()).FirstOrDefault() == "test" &&
                    x.Arguments.Select(arg => arg.Render())
                        .Contains("\"/repo/tests/Project.Tests.csproj\""))))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void UseMicrosoftTestingPlatform_NotOverridden_IsFalse()
    {
        // Arrange
        var cakeContext = CakeTestHelper.CreateCakeContext("dotnet-test");
        CakeTestHelper.SetupFileSystem(cakeContext, "/repo", "/repo/test.sln");

        ITestTaskSettings settings = new TestTestBuildContext(cakeContext);

        // Act
        var useMicrosoftTestingPlatform = settings.UseMicrosoftTestingPlatform;

        // Assert
        useMicrosoftTestingPlatform
            .Should()
            .BeFalse();
    }

    [Fact]
    public async Task RunAsync_DefaultSettings_PassesVsTestArguments()
    {
        // Arrange
        var calls = new List<string[]>();
        var context = new TestTestBuildContext(CreateCakeContext(calls));
        var task = new TestTask<TestTestBuildContext>();

        var expectedLogger = $"\"trx;LogFileName={context.TestResultsDir.FullPath}/Project.Tests.trx\"";

        // Act
        await task.RunAsync(context);

        // Assert
        calls
            .Should()
            .ContainSingle()
            .Which
            .Should()
            .HaveElementAt(0, "test")
            .And.HaveElementAt(1, "\"/repo/tests/Project.Tests.csproj\"")
            .And.ContainInConsecutiveOrder("--logger", expectedLogger)
            .And.ContainInConsecutiveOrder("--collect", "\"XPlat Code Coverage\"")
            .And.NotContain("--project")
            .And.NotContain("--report-trx")
            .And.NotContain("--coverage");
    }

    [Fact]
    public async Task RunAsync_VsTestWithoutCoverage_OmitsCollector()
    {
        // Arrange
        var calls = new List<string[]>();
        var context = new TestTestBuildContext(CreateCakeContext(calls), false);
        var task = new TestTask<TestTestBuildContext>();

        // Act
        await task.RunAsync(context);

        // Assert
        calls
            .Should()
            .ContainSingle()
            .Which
            .Should()
            .Contain("--logger")
            .And.NotContain("--collect")
            .And.NotContain("\"XPlat Code Coverage\"");
    }

    [Fact]
    public async Task RunAsync_UseMicrosoftTestingPlatformWithCoverage_PassesMtpArguments()
    {
        // Arrange
        var calls = new List<string[]>();
        var context = new MtpTestBuildContext(CreateCakeContext(calls));
        var task = new TestTask<MtpTestBuildContext>();

        // Act
        await task.RunAsync(context);

        // Assert
        calls
            .Should()
            .ContainSingle()
            .Which
            .Should()
            .HaveElementAt(0, "test")
            .And.ContainInConsecutiveOrder("--project", "\"/repo/tests/Project.Tests.csproj\"")
            .And.ContainInConsecutiveOrder("--results-directory", $"\"{context.CodeCoverageDir.FullPath}\"")
            .And.ContainInConsecutiveOrder("--report-trx", "--report-trx-filename", "\"Project.Tests.trx\"")
            .And.ContainInConsecutiveOrder("--coverage", "--coverage-output-format", "cobertura")
            .And.ContainInConsecutiveOrder("--coverage-output", "\"Project.Tests.cobertura.xml\"")
            .And.NotContain("--logger")
            .And.NotContain("--collect");
    }

    [Fact]
    public async Task RunAsync_UseMicrosoftTestingPlatformWithoutCoverage_OmitsCoverageArguments()
    {
        // Arrange
        var calls = new List<string[]>();
        var context = new MtpTestBuildContext(CreateCakeContext(calls), false);
        var task = new TestTask<MtpTestBuildContext>();

        // Act
        await task.RunAsync(context);

        // Assert
        calls
            .Should()
            .ContainSingle()
            .Which
            .Should()
            .ContainInConsecutiveOrder("--project", "\"/repo/tests/Project.Tests.csproj\"")
            .And.ContainInConsecutiveOrder("--report-trx", "--report-trx-filename", "\"Project.Tests.trx\"")
            .And.NotContain("--coverage")
            .And.NotContain("--coverage-output-format")
            .And.NotContain("--coverage-output")
            .And.NotContain("--logger")
            .And.NotContain("--collect");
    }

    [Fact]
    public async Task RunAsync_UseMicrosoftTestingPlatformWithMultipleProjects_PassesProjectSpecificArguments()
    {
        // Arrange
        var calls = new List<string[]>();
        var context = new MtpTestBuildContext(CreateCakeContext(calls), testProjects:
        [
            new FilePath("/repo/tests/B.Tests.csproj"),
            new FilePath("/repo/tests/A.Tests.csproj")
        ]);
        var task = new TestTask<MtpTestBuildContext>();

        // Act
        await task.RunAsync(context);

        // Assert
        calls
            .Should()
            .HaveCount(2);

        calls[0]
            .Should()
            .ContainInConsecutiveOrder("--project", "\"/repo/tests/A.Tests.csproj\"")
            .And.ContainInConsecutiveOrder("--report-trx-filename", "\"A.Tests.trx\"");

        calls[1]
            .Should()
            .ContainInConsecutiveOrder("--project", "\"/repo/tests/B.Tests.csproj\"")
            .And.ContainInConsecutiveOrder("--report-trx-filename", "\"B.Tests.trx\"");
    }

    [Fact]
    public async Task RunAsync_UseMicrosoftTestingPlatformAfterBuildTask_PassesNoBuild()
    {
        // Arrange
        var calls = new List<string[]>();
        var context = new MtpTestBuildContext(CreateCakeContext(calls));
        context.AddExecutedTask(new BuildTask<MtpTestBuildContext>());
        var task = new TestTask<MtpTestBuildContext>();

        // Act
        await task.RunAsync(context);

        // Assert
        calls
            .Should()
            .ContainSingle()
            .Which
            .Should()
            .Contain("--no-build")
            .And.ContainInConsecutiveOrder("--project", "\"/repo/tests/Project.Tests.csproj\"")
            .And.Contain("--report-trx");
    }

    [Fact]
    public async Task RunAsync_UseMicrosoftTestingPlatformWithApplyDotNetTestSettingsOverride_KeepsMtpArguments()
    {
        // Arrange
        var calls = new List<string[]>();
        var context = new MtpTestBuildContext(CreateCakeContext(calls));
        var task = new FrameworkTestTask();

        // Act
        await task.RunAsync(context);

        // Assert
        calls
            .Should()
            .ContainSingle()
            .Which
            .Should()
            .ContainInConsecutiveOrder("--framework", "net10.0")
            .And.ContainInConsecutiveOrder("--report-trx", "--report-trx-filename", "\"Project.Tests.trx\"")
            .And.ContainInConsecutiveOrder("--coverage", "--coverage-output-format", "cobertura");
    }

    [Fact]
    public async Task RunAsync_UseMicrosoftTestingPlatformWithArgumentCustomizationOverride_KeepsBothArguments()
    {
        // Arrange
        var calls = new List<string[]>();
        var context = new MtpTestBuildContext(CreateCakeContext(calls));
        var task = new ArgumentCustomizationTestTask();

        // Act
        await task.RunAsync(context);

        // Assert
        calls
            .Should()
            .ContainSingle()
            .Which
            .Should()
            .Contain("--custom-argument")
            .And.ContainInConsecutiveOrder("--report-trx", "--report-trx-filename", "\"Project.Tests.trx\"")
            .And.ContainInConsecutiveOrder("--coverage", "--coverage-output-format", "cobertura");
    }
}
