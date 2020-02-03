using System;
using System.IO;
using JetBrains.Annotations;
using Nuke.Common.IO;
using Nuke.Common.Utilities.Collections;

namespace CreativeCoders.NukeBuild.BuildActions
{
    [PublicAPI]
    public class CopyToArtifactsBuildAction : BuildActionBase<CopyToArtifactsBuildAction>
    {
        private string _fileMask;

        protected override void OnExecute()
        {
            if (string.IsNullOrEmpty(_fileMask))
            {
                throw new ArgumentException("file mask must not be null or empty");
            }

            BuildInfo.SourceDirectory.GlobFiles(_fileMask).ForEach(CopyFileToArtifacts);
        }

        private void CopyFileToArtifacts(PathConstruction.AbsolutePath fullFileName)
        {
            var fileName = Path.GetFileName(fullFileName);

            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }

            var targetFile = Path.Combine(BuildInfo.ArtifactsDirectory, fileName);
            FileSystemTasks.CopyFile(fullFileName, targetFile, FileExistsPolicy.Overwrite);
        }

        public void SetFileMask(string fileMask)
        {
            _fileMask = fileMask;
        }
    }
}