using JetBrains.Annotations;

namespace CreativeCoders.NukeBuild.BuildActions
{
    [PublicAPI]
    public static class PackageLicenseExpressions
    {
        public const string LGPL30Only = "LGPL-3.0-only";
        
        public const string LGPL30OrLater = "LGPL-3.0-or-later";

        public const string MIT = "MIT";

        public const string GPL30Only = "GPL-3.0-only";

        public const string GPL30OrLater = "GPL-3.0-or-later";

        public const string ApacheLicense20 = "Apache-2.0";

        public const string MPL20 = "MPL-2.0";
    }
}