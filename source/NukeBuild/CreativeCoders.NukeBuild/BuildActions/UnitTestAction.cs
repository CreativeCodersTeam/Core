using System.IO;
using JetBrains.Annotations;
using Nuke.Common.IO;
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

            foreach (var unitTestProject in unitTestProjects)
            {
                var projectName = Path.GetFileNameWithoutExtension(unitTestProject);

                var resultFile = Path.Combine(_resultsDirectory, $"results_{projectName}.xml");
                
                DotNetTasks.DotNetTest(
                    x => x
                        .SetProjectFile(unitTestProject)
                        .SetConfiguration(BuildInfo.Configuration)
                        .SetLogger($"xunit;LogFilePath={resultFile}")
                        .SetResultsDirectory(_resultsDirectory));
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