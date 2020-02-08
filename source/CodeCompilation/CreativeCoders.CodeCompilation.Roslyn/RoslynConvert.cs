using System;
using CreativeCoders.Core;
using Microsoft.CodeAnalysis;

namespace CreativeCoders.CodeCompilation.Roslyn
{
    internal static class RoslynConvert
    {
        public static CompilationMessageType ConvertMessageType(DiagnosticSeverity diagnosticSeverity)
        {
            switch (diagnosticSeverity)
            {
                case DiagnosticSeverity.Hidden:
                    return CompilationMessageType.Suggestion;
                case DiagnosticSeverity.Info:
                    return CompilationMessageType.Info;
                case DiagnosticSeverity.Warning:
                    return CompilationMessageType.Warning;
                case DiagnosticSeverity.Error:
                    return CompilationMessageType.Error;
                default:
                    throw new ArgumentOutOfRangeException(nameof(diagnosticSeverity), diagnosticSeverity, null);
            }
        }

        public static OutputKind ConvertOutputKind(CompilationOutputKind outputKind)
        {
            switch (outputKind)
            {
                case CompilationOutputKind.DynamicallyLinkedLibrary:
                    return OutputKind.DynamicallyLinkedLibrary;
                case CompilationOutputKind.ConsoleApplication:
                    return OutputKind.ConsoleApplication;
                case CompilationOutputKind.WindowsApplication:
                    return OutputKind.WindowsApplication;
                default:
                    throw new ArgumentOutOfRangeException(nameof(outputKind), outputKind, "Output kind unknown");
            }
        }

        public static TextSpan ConvertTextSpan(Microsoft.CodeAnalysis.Text.TextSpan locationSourceSpan)
        {
            return new TextSpan(locationSourceSpan.Start, locationSourceSpan.Length);
        }
    }
}