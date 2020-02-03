using CreativeCoders.Core;
using JetBrains.Annotations;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;

namespace CreativeCoders.NukeBuild.BuildActions
{
    [PublicAPI]
    public class PackBuildAction : BuildActionBase<PackBuildAction>
    {
        private DotNetSymbolPackageFormat _symbolPackageFormat;
        
        private string _packageLicenseUrl;
        
        private string _packageLicenseExpression;

        public PackBuildAction()
        {
            _symbolPackageFormat = DotNetSymbolPackageFormat.snupkg;
        }

        protected override void OnExecute()
        {
            DotNetTasks.DotNetPack(s => s
                .SetWorkingDirectory(BuildInfo.Solution.Directory)
                .SetSymbolPackageFormat(_symbolPackageFormat)
                .SetProject(BuildInfo.Solution)
                .EnableNoBuild()
                .SetConfiguration(BuildInfo.Configuration)
                .EnableIncludeSymbols()
                .SetOutputDirectory(BuildInfo.ArtifactsDirectory)
                .SetVersion(BuildInfo.VersionInfo.NuGetVersionV2)
                .FluentIf(!string.IsNullOrEmpty(_packageLicenseUrl), x => x.SetPackageLicenseUrl(_packageLicenseUrl))
                .FluentIf(!string.IsNullOrWhiteSpace(_packageLicenseExpression),
                    x => x.SetProperty("PackageLicenseExpression", _packageLicenseExpression))
            );
        }

        public PackBuildAction SetSymbolPackageFormat(DotNetSymbolPackageFormat symbolPackageFormat)
        {
            _symbolPackageFormat = symbolPackageFormat;

            return this;
        }

        public PackBuildAction SetPackageLicenseUrl(string packageLicenseUrl)
        {
            _packageLicenseUrl = packageLicenseUrl;

            return this;
        }

        public PackBuildAction SetPackageLicenseExpression(string packageLicenseExpression)
        {
            _packageLicenseExpression = packageLicenseExpression;

            return this;
        }
    }
}