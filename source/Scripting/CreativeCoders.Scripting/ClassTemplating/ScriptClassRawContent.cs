namespace CreativeCoders.Scripting.ClassTemplating {
    public class ScriptClassRawContent : ScriptClassMember
    {
        public ScriptClassRawContent(string rawContent) : base(ScriptClassMemberType.Raw, "Raw")
        {
            RawContent = rawContent;
        }

        public string RawContent { get; }
    }
}