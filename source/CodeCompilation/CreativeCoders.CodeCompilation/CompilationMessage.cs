using CreativeCoders.Core;
using CreativeCoders.Core.Text;
using JetBrains.Annotations;

namespace CreativeCoders.CodeCompilation
{
    [PublicAPI]
    public class CompilationMessage
    {
        public CompilationMessage(CompilationMessageType messageType, TextSpan sourceSpan, string message)
        {
            Ensure.IsNotNull(messageType, nameof(messageType));
            Ensure.IsNotNull(sourceSpan, nameof(sourceSpan));
            
            MessageType = messageType;
            SourceSpan = sourceSpan;
            Message = message;
            IsInSource = !SourceSpan.IsEmpty;
        }

        public CompilationMessageType MessageType { get; }

        public bool IsInSource { get; }

        public TextSpan SourceSpan { get; }

        public string Message { get; }
    }
}
