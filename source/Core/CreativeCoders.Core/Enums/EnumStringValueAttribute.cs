using System;

namespace CreativeCoders.Core.Enums
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Enum)]
    public class EnumStringValueAttribute : Attribute, IEnumStringAttribute
    {
        public EnumStringValueAttribute(string text)
        {
            Text = text;
        }

        public string Text { get; }
    }
}