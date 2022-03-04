using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Nuke.Common.IO;
using Nuke.Common.Tools.DotNet;

namespace CreativeCoders.NukeBuild.BuildActions
{
    [PublicAPI]
    public class DotNetTestAction : BuildActionBase<DotNetTestAction>
    {
        private string _projectsPattern;
        
        private string _resultsDirectory;

        private AbsolutePath _testProjectsBaseDirectory;

        private AbsolutePath _coverageDirectory;

        private bool _enableCodeCoverage;

        private CoverletOutputFormat _coverageOutputFormat = CoverletOutputFormat.cobertura;

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

                var testResultFile = $"{projectName}.{_resultFileExt}";

                var coverageResultFile = _coverageDirectory / $"coverage_{ projectName}.xml";
                
                try
                {
                    DotNetTasks.DotNetTest(
                        x => x
                            .SetProjectFile(unitTestProject)
                            .SetConfiguration(BuildInfo.Configuration)
                            .SetLoggers($"{_logger};LogFileName={testResultFile}")
                            .SetResultsDirectory(_resultsDirectory)
                            .SetCollectCoverage(_enableCodeCoverage)
                            .SetCoverletOutput(coverageResultFile)
                            .SetCoverletOutputFormat(_coverageOutputFormat));
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

        private DotNetTestSettings CreateTestSettings(string unitTestProject, string testResultFile,
            string coverageDirectory)
        {
            var settings = new DotNetTestSettings()
                .SetProjectFile(unitTestProject)
                .SetConfiguration(BuildInfo.Configuration)
                .SetLogger($"xunit;LogFilePath={testResultFile}")
                .SetResultsDirectory(_resultsDirectory);

            if (_enableCodeCoverage)
            {
                return settings
                    .SetDataCollector("XPlat Code Coverage")
                    .SetResultsDirectory(coverageDirectory);
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

        public DotNetTestAction SetCoverageFormat(CoverletOutputFormat coverageOutputFormat)
        {
            _coverageOutputFormat = coverageOutputFormat;

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
}
