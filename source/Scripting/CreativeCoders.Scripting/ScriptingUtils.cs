using CreativeCoders.Core;
using JetBrains.Annotations;

namespace CreativeCoders.Scripting
{
    [PublicAPI]
    public static class ScriptingUtils
    {
        public static string GetValidClassName(string className)
        {
            Ensure.IsNotNullOrWhitespace(className, nameof(className));

            return className.Filter(char.IsLetterOrDigit);
        }
    }
}