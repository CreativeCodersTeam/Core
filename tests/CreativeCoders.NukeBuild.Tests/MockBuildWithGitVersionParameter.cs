using CreativeCoders.NukeBuild.Components.Parameters;
using Nuke.Common.Tools.GitVersion;

namespace CreativeCoders.NukeBuild.Tests;

public class MockBuildWithGitVersionParameter(GitVersion gitVersion) : MockNukeBuild, IGitVersionParameter
{
    GitVersion? IGitVersionParameter.GitVersion => gitVersion;
}
