using CreativeCoders.Core;
using JetBrains.Annotations;

namespace CreativeCoders.Scripting.CSharp.ClassTemplating
{
    [PublicAPI]
    public abstract class ScriptClassMember
    {
        protected ScriptClassMember(ScriptClassMemberType memberType, string name)
        {
            Ensure.IsNotNullOrWhitespace(name, nameof(name));
            
            MemberType = memberType;
            Name = name;
        }

        public ScriptClassMemberType MemberType { get; }

        public string Name { get; }        
    }
}