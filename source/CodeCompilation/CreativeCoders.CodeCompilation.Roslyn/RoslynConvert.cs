using System;
using CreativeCoders.Core;
using CreativeCoders.Core.Text;
using Microsoft.CodeAnalysis;

namespace CreativeCoders.CodeCompilation.Roslyn
{
    internal static class RoslynConvert
    {
        public static CompilationMessageType ConvertMessageType(DiagnosticSeverity diagnosticSeverity)
        {
            return diagnosticSeverity switch
            {
                DiagnosticSeverity.Hidden => CompilationMessageType.Suggestion,
                DiagnosticSeverity.Info => CompilationMessageType.Info,
                DiagnosticSeverity.Warning => CompilationMessageType.Warning,
                DiagnosticSeverity.Error => CompilationMessageType.Error,
                _ => throw new ArgumentOutOfRangeException(nameof(diagnosticSeverity), diagnosticSeverity, null)
            };
        }

        public static OutputKind ConvertOutputKind(CompilationOutputKind outputKind)
        {
            return outputKind switch
            {
                CompilationOutputKind.DynamicallyLinkedLibrary => OutputKind.DynamicallyLinkedLibrary,
                CompilationOutputKind.ConsoleApplication => OutputKind.ConsoleApplication,
                CompilationOutputKind.WindowsApplication => OutputKind.WindowsApplication,
                _ => throw new ArgumentOutOfRangeException(nameof(outputKind), outputKind, "Output kind unknown")
            };
        }

        public static TextSpan ConvertTextSpan(Microsoft.CodeAnalysis.Text.TextSpan locationSourceSpan)
        {
            return new(locationSourceSpan.Start, locationSourceSpan.Length);
        }
    }
}
