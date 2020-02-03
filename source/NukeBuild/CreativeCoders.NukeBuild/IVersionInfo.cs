namespace CreativeCoders.NukeBuild
{
    public interface IVersionInfo
    {
        string GetNormalizedAssemblyVersion();
        
        string GetNormalizedFileVersion();
        
        string InformationalVersion { get; }
        
        string NuGetVersionV2 { get; }
    }
}