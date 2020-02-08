using System;
using JetBrains.Annotations;

namespace CreativeCoders.Net.WebApi.Definition
{
    [PublicAPI]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
    public class ViaBodyAttribute : Attribute
    {
        public Type DataFormatterType { get; set; }
    }
}