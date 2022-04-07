using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Nuke.Common.IO;
using Nuke.Common.Tools.DotNet;

namespace CreativeCoders.NukeBuild.BuildActions;

[PublicAPI]
public class DotNetTestAction : BuildActionBase<DotNetTestAction>
{
    private string _projectsPattern;

    private string _resultsDirectory;

    private AbsolutePath _testProjectsBaseDirectory;

    private AbsolutePath _coverageDirectory;

    private bool _enableCodeCoverage;

    private string _logger = "xunit";

    private string _resultFileExt = "xml";

    protected override void OnExecute()
    {
        if (!Directory.Exists(_testProjectsBaseDirectory))
        {
            return;
        }

        var unitTestProjects = _testProjectsBaseDirectory.GlobFiles(_projectsPattern);

        var failedTests = new List<(string UnitTestProject, Exception Exception)>();

        foreach (var unitTestProject in unitTestProjects)
        {
            var projectName = Path.GetFileNameWithoutExtension(unitTestProject);

            var testResultFile = Path.Combine(_resultsDirectory, $"{projectName}.{_resultFileExt}");

            try
            {
                var testSettings =
                    CreateTestSettings(unitTestProject, testResultFile);

                DotNetTasks.DotNetTest(testSettings);
            }
            catch (Exception e)
            {
                failedTests.Add((UnitTestProject: unitTestProject, Exception: e));
            }
        }

        if (failedTests.Any())
        {
            throw failedTests.First().Exception;
        }
    }

    private DotNetTestSettings CreateTestSettings(string unitTestProject, string testResultFile)
    {
        var settings = new DotNetTestSettings()
            .SetProjectFile(unitTestProject)
            .SetConfiguration(BuildInfo.Configuration)
            .SetLoggers($"{_logger};LogFileName={testResultFile}");

        if (_enableCodeCoverage)
        {
            return settings
                .SetDataCollector("XPlat Code Coverage")
                .SetResultsDirectory(_coverageDirectory);
        }

        return settings;
    }

    public DotNetTestAction SetTestProjectsBaseDirectory(AbsolutePath testProjectsBaseDirectory)
    {
        _testProjectsBaseDirectory = testProjectsBaseDirectory;

        return this;
    }

    public DotNetTestAction SetProjectsPattern(string projectsPattern)
    {
        _projectsPattern = projectsPattern;

        return this;
    }

    public DotNetTestAction SetResultsDirectory(string resultsDirectory)
    {
        _resultsDirectory = resultsDirectory;

        return this;
    }

    public DotNetTestAction SetCoverageDirectory(AbsolutePath coverageDirectory)
    {
        _coverageDirectory = coverageDirectory;

        return this;
    }

    public DotNetTestAction EnableCoverage()
    {
        _enableCodeCoverage = true;

        return this;
    }

    public DotNetTestAction UseLogger(string logger)
    {
        _logger = logger;

        return this;
    }

    public DotNetTestAction SetResultFileExt(string resultFileExt)
    {
        _resultFileExt = resultFileExt;

        return this;
    }
}
