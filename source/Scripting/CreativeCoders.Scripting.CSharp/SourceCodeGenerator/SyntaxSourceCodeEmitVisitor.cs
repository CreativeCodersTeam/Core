using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CreativeCoders.Core;
using CreativeCoders.Core.Collections;
using CreativeCoders.Core.Visitors;
using CreativeCoders.Scripting.CSharp.SourceCodeGenerator.Nodes;

namespace CreativeCoders.Scripting.CSharp.SourceCodeGenerator;

public class SyntaxSourceCodeEmitVisitor : VisitorBase<SyntaxSourceCodeEmitVisitor, UsingSyntaxNode>,
    IVisitor<SyntaxSourceCodeEmitVisitor, NameSpaceSyntaxNode>,
    IVisitor<SyntaxSourceCodeEmitVisitor, ClassSyntaxNode>,
    IVisitor<SyntaxSourceCodeEmitVisitor, MethodSyntaxNode>,
    IVisitor<SyntaxSourceCodeEmitVisitor, PropertySyntaxNode>,
    IVisitor<SyntaxSourceCodeEmitVisitor, RawContentSyntaxNode>,
    IVisitor<SyntaxSourceCodeEmitVisitor, EmptyLineSyntaxNode>
{
    private readonly StringBuilder _output;

    private readonly string _scriptSourceCode;

    private int _indentLevel;

    public SyntaxSourceCodeEmitVisitor(StringBuilder output, string scriptSourceCode) : base(false)
    {
        Ensure.IsNotNull(output, nameof(output));
        Ensure.IsNotNull(scriptSourceCode, nameof(scriptSourceCode));

        _output = output;
        _scriptSourceCode = scriptSourceCode;
    }

    private void AppendLine(string text)
    {
        var indent = new string(' ', _indentLevel * 4);
        _output.AppendLine(indent + text);
    }

    public override void Visit(UsingSyntaxNode usingSyntax)
    {
        AppendLine($"using {usingSyntax.UsingNameSpace};");
    }

    public void Visit(NameSpaceSyntaxNode nameSpaceSyntax)
    {
        AppendLine($"namespace {nameSpaceSyntax.NameSpace}");
        AppendLine("{");
        _indentLevel++;
        VisitSubItems(nameSpaceSyntax);
        _indentLevel--;
        AppendLine("}");
    }

    public void Visit(ClassSyntaxNode classSyntax)
    {
        AppendLine($"public class {classSyntax.ClassName}{GetImplementingInterfaces(classSyntax.InheritsFrom)}");
        AppendLine("{");
        _indentLevel++;
        VisitSubItems(classSyntax);
        _indentLevel--;
        AppendLine("}");
    }

    private static string GetImplementingInterfaces(IEnumerable<string> inheritsFrom)
    {
        var interfaces = inheritsFrom.Distinct().ToArray();
        if (interfaces.Length == 0)
        {
            return string.Empty;
        }
            
        var interfacesText = string.Join(", ", interfaces);
        return $" : {interfacesText}";
    }

    public void Visit(MethodSyntaxNode methodSyntax)
    {
        AppendLine($"public void {methodSyntax.MethodName}()");
        AppendLine("{");
        _indentLevel++;
        AppendCodeLines(methodSyntax.SourceCode);
        _indentLevel--;
        AppendLine("}");
    }

    private void AppendCodeLines(string sourceCode)
    {
        sourceCode = sourceCode.Replace(CSharpScriptConstants.CodePlaceHolder, _scriptSourceCode);
        var lines = sourceCode.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
        lines.ForEach(AppendLine);
    }

    public void Visit(PropertySyntaxNode propertySyntax)
    {
        if (string.IsNullOrWhiteSpace(propertySyntax.PropertyGetterSourceCode) &&
            string.IsNullOrWhiteSpace(propertySyntax.PropertySetterSourceCode))
        {
            AppendLine($"public {propertySyntax.ValueType} {propertySyntax.PropertyName} {{ get; set; }}");
            return;
        }
        AppendLine($"public {propertySyntax.ValueType} {propertySyntax.PropertyName}");
        AppendLine("{");
        _indentLevel++;

        if (!string.IsNullOrWhiteSpace(propertySyntax.PropertyGetterSourceCode))
        {
            AddPropertyPart("get", propertySyntax.PropertyGetterSourceCode);
        }

        if (!string.IsNullOrWhiteSpace(propertySyntax.PropertySetterSourceCode))
        {
            AddPropertyPart("set", propertySyntax.PropertySetterSourceCode);
        }

        _indentLevel--;
        AppendLine("}");
    }

    private void AddPropertyPart(string partName, string sourceCode)
    {
        AppendLine(partName);
        AppendLine("{");
        _indentLevel++;

        var lines = sourceCode.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        lines.ForEach(AppendLine);

        _indentLevel--;
        AppendLine("}");
    }

    public void Visit(RawContentSyntaxNode rawContentSyntax)
    {
        AppendCodeLines(rawContentSyntax.RawContent);            
    }

    public void Visit(EmptyLineSyntaxNode visitableObject)
    {
        _output.AppendLine("");
    }
}