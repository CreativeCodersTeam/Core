using System;
using System.Diagnostics.CodeAnalysis;
using CreativeCoders.Core;
using JetBrains.Annotations;
using Nuke.Common.IO;
using Nuke.Common.Tools.DotNet;

namespace CreativeCoders.NukeBuild.BuildActions;

[ExcludeFromCodeCoverage]
[PublicAPI]
public class PushBuildAction : BuildActionBase<PushBuildAction>
{
    private string _apiKey;

    private string _source;

    private string _symbolSource;

    private string _symbolApiKey;

    protected override void OnExecute()
    {
        foreach (var packagePath in BuildInfo.ArtifactsDirectory.GlobFiles("*.nupkg"))
        {
            PushPackage(packagePath);
        }
    }

    private void PushPackage(string package)
    {
        DotNetTasks.DotNetNuGetPush(x => x
            .SetTargetPath(package)
            .FluentIf(_source != null, config => config.SetSource(_source))
            .FluentIf(_apiKey != null, config => config.SetApiKey(_apiKey))
            .FluentIf(_symbolSource != null, config => config.SetSymbolSource(_symbolSource))
            .FluentIf(_symbolApiKey != null, config => config.SetSymbolApiKey(_symbolApiKey))
        );
    }

    public PushBuildAction SetApiKey(string apiKey)
    {
        _apiKey = apiKey;

        return this;
    }

    public PushBuildAction SetSource(string source)
    {
        _source = source;

        if (_source != null && !_source.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
        {
            _source = "https://" + _source;
        }

        return this;
    }

    public PushBuildAction SetSymbolSource(string symbolSource)
    {
        _symbolSource = symbolSource;

        return this;
    }

    public PushBuildAction SetSymbolApiKey(string symbolApiKey)
    {
        _symbolApiKey = symbolApiKey;

        return this;
    }
}
