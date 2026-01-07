using Cake.Common.Tools.DotNet.MSBuild;

namespace CreativeCoders.CakeBuild;

public static class DotNetMSBuildSettingsExtensions
{
    public static DotNetMSBuildSettings WithPackageProjectUrl(this DotNetMSBuildSettings settings,
        string packageProjectUrl)
    {
        return settings.SetPropertyIfNotEmpty("PackageProjectUrl", packageProjectUrl);
    }

    public static DotNetMSBuildSettings WithPackageLicenseUrl(this DotNetMSBuildSettings settings,
        string packageLicenseUrl)
    {
        return settings.SetPropertyIfNotEmpty("PackageLicenseUrl", packageLicenseUrl);
    }

    public static DotNetMSBuildSettings WithPackageLicenseExpression(this DotNetMSBuildSettings settings,
        string packageLicenseExpression)
    {
        return settings.SetPropertyIfNotEmpty("PackageLicenseExpression", packageLicenseExpression);
    }

    public static DotNetMSBuildSettings WithCopyright(this DotNetMSBuildSettings settings, string copyright)
    {
        return settings.SetPropertyIfNotEmpty("Copyright", copyright);
    }

    public static DotNetMSBuildSettings SetProperty(this DotNetMSBuildSettings settings, string propertyName,
        string propertyValue)
    {
        settings.Properties[propertyName] = [propertyValue];
        return settings;
    }

    public static DotNetMSBuildSettings SetPropertyIfNotEmpty(this DotNetMSBuildSettings settings,
        string propertyName,
        string? propertyValue)
    {
        return string.IsNullOrEmpty(propertyValue)
            ? settings
            : settings.SetProperty(propertyName, propertyValue);
    }
}
