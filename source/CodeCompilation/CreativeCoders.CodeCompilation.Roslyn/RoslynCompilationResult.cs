using System.Collections.Generic;
using CreativeCoders.Core;
using CreativeCoders.Core.Collections;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CreativeCoders.CodeCompilation.Roslyn
{
    internal class RoslynCompilationResult : ICompilationResult
    {
        private readonly CSharpCompilation _compilation;

        private readonly ICompilationOutputData _outputData;

        private readonly IList<CompilationMessage> _messages;

        public RoslynCompilationResult(CSharpCompilation compilation, ICompilationOutputData outputData)
        {
            Ensure.IsNotNull(compilation, nameof(compilation));
            Ensure.IsNotNull(outputData, nameof(outputData));

            _compilation = compilation;
            _outputData = outputData;
            _messages = new List<CompilationMessage>();
            
            EmitCompilation();
        }

        private void EmitCompilation()
        {
            var peStream = _outputData.GetPeStream();
            var emitResult = _compilation.Emit(peStream);
            Success = emitResult.Success;
            emitResult.Diagnostics.ForEach(AddMessage);
        }

        private void AddMessage(Diagnostic diagnostic)
        {
            var messageType = RoslynConvert.ConvertMessageType(diagnostic.Severity);
            var textSpan = RoslynConvert.ConvertTextSpan(diagnostic.Location.SourceSpan);
            
            var message = new CompilationMessage(messageType, textSpan, diagnostic.GetMessage());

            _messages.Add(message);
        }

        public bool Success { get; private set; }

        public IEnumerable<CompilationMessage> Messages => _messages;
    }
}