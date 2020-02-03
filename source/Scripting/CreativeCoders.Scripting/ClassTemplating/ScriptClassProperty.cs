using CreativeCoders.Core;

namespace CreativeCoders.Scripting.ClassTemplating
{
    public class ScriptClassProperty : ScriptClassMember
    {
        internal ScriptClassProperty(string name, string valueType, string getterSourceCode, string setterSourceCode) : base(
            ScriptClassMemberType.Property, name)
        {
            Ensure.IsNotNullOrWhitespace(valueType, nameof(valueType));

            ValueType = valueType;
            GetterSourceCode = getterSourceCode;
            SetterSourceCode = setterSourceCode;
        }

        public string ValueType { get; }

        public string GetterSourceCode { get; }

        public string SetterSourceCode { get; }
    }
}