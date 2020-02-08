using System;
using System.Reflection;

namespace CreativeCoders.Core.Reflection
{
    public static class ParameterInfoExtensions
    {
        public static bool IsParams(this ParameterInfo parameterInfo)
        {
            return parameterInfo.IsDefined(typeof(ParamArrayAttribute), false);
        }
    }
}