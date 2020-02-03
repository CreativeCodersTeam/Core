using System;

namespace CreativeCoders.Core.Enums
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumStringValueAttribute : Attribute, IEnumStringAttribute
    {
        public EnumStringValueAttribute(string text)
        {
            Ensure.IsNotNull(text, nameof(text));

            Text = text;
        }

        public string Text { get; }
    }
}