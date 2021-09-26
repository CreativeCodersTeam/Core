using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using CreativeCoders.NukeBuild.Exceptions;
using JetBrains.Annotations;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;

namespace CreativeCoders.NukeBuild.BuildActions
{
    [PublicAPI]
    public class UnitTestAction : BuildActionBase<UnitTestAction>
    {
        private string _unitTestsBasePath;
        
        private string _projectsPattern;
        
        private string _resultsDirectory;

        protected override void OnExecute()
        {
            var unitTestPath = BuildInfo.SourceDirectory / _unitTestsBasePath;

            if (!Directory.Exists(unitTestPath))
            {
                return;
            }
            
            var unitTestProjects = unitTestPath.GlobFiles(_projectsPattern);

            var failedTests = new List<(string UnitTestProject, Exception Exception)>();

            foreach (var unitTestProject in unitTestProjects)
            {
                var projectName = Path.GetFileNameWithoutExtension(unitTestProject);

                var resultFile = Path.Combine(_resultsDirectory, $"results_{projectName}.xml");

                try
                {
                    DotNetTasks.DotNetTest(
                        x => x
                            .SetProjectFile(unitTestProject)
                            .SetConfiguration(BuildInfo.Configuration)
                            .SetLogger($"xunit;LogFilePath={resultFile}")
                            .SetResultsDirectory(_resultsDirectory));
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

        public UnitTestAction SetUnitTestsBasePath(string unitTestsBasePath)
        {
            _unitTestsBasePath = unitTestsBasePath;
            return this;
        }
        
        public UnitTestAction SetProjectsPattern(string projectsPattern)
        {
            _projectsPattern = projectsPattern;
            return this;
        }

        public UnitTestAction SetResultsDirectory(string resultsDirectory)
        {
            _resultsDirectory = resultsDirectory;
            return this;
        }
    }
}
