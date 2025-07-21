using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using JetBrains.Annotations;
using Nuke.Common.IO;
using Nuke.Common.Utilities.Collections;

namespace CreativeCoders.NukeBuild.BuildActions;

[ExcludeFromCodeCoverage]
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

    private void CopyFileToArtifacts(AbsolutePath fullFileName)
    {
        var fileName = Path.GetFileName(fullFileName);

        if (string.IsNullOrEmpty(fileName))
        {
            return;
        }

        var targetFile = Path.Combine(BuildInfo.ArtifactsDirectory, fileName);
        fullFileName.Copy(targetFile, ExistsPolicy.FileOverwrite);
    }

    public void SetFileMask(string fileMask)
    {
        _fileMask = fileMask;
    }
}
