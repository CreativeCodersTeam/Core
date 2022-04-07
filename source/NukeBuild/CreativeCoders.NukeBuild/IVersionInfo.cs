namespace CreativeCoders.NukeBuild;

public interface IVersionInfo
{
    string GetAssemblySemVer();

    string GetAssemblySemFileVer();

    string InformationalVersion { get; }

    string NuGetVersionV2 { get; }
}
