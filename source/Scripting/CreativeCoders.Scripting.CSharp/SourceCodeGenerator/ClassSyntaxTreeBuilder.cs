using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CreativeCoders.Core;
using CreativeCoders.Core.Collections;
using CreativeCoders.Scripting.CSharp.ClassTemplating;
using CreativeCoders.Scripting.CSharp.SourceCodeGenerator.Nodes;

namespace CreativeCoders.Scripting.CSharp.SourceCodeGenerator;

[SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
public class ClassSyntaxTreeBuilder
{
    private readonly ScriptClassTemplate _template;

    private readonly string _nameSpace;

    private readonly string _className;

    private readonly IEnumerable<string> _scriptUsings;

    public ClassSyntaxTreeBuilder(ScriptClassTemplate template, string nameSpace, string className, IEnumerable<string> scriptUsings)
    {
        Ensure.IsNotNull(template, nameof(template));
        Ensure.IsNotNullOrWhitespace(nameSpace, nameof(nameSpace));
        Ensure.IsNotNullOrWhitespace(className, nameof(className));
        Ensure.IsNotNull(scriptUsings, nameof(scriptUsings));

        _template = template;
        _nameSpace = nameSpace;
        _className = className;
        _scriptUsings = scriptUsings;
    }

    public ClassSyntaxTree Build()
    {
        var syntaxTree = new ClassSyntaxTree();

        AddUsings(syntaxTree);
        syntaxTree.RootNode.AddSubNode(new EmptyLineSyntaxNode());
        AddNameSpaceBlock(syntaxTree);

        return syntaxTree;
    }

    private void AddNameSpaceBlock(ClassSyntaxTree syntaxTree)
    {
        var nameSpaceNode = new NameSpaceSyntaxNode(_nameSpace);
        syntaxTree.RootNode.AddSubNode(nameSpaceNode);
        AddClassBlock(nameSpaceNode);
    }

    private void AddClassBlock(NameSpaceSyntaxNode nameSpaceNode)
    {
        var classNode = new ClassSyntaxNode(_className, _template.ImplementsInterfaces);
        nameSpaceNode.AddSubNode(classNode);
        AddMethods(classNode);
        AddProperties(classNode);
        AddRawContents(classNode);
    }

    private void AddRawContents(ClassSyntaxNode classNode)
    {
        _template.Members.OfType<ScriptClassRawContent>().ForEach(rawContent => AddRawContent(classNode, rawContent));
    }

    private static void AddRawContent(ClassSyntaxNode classNode, ScriptClassRawContent rawContent)
    {
        var rawContentNode = new RawContentSyntaxNode(rawContent.RawContent);
        classNode.AddSubNode(rawContentNode);
    }

    private void AddProperties(ClassSyntaxNode classNode)
    {
        _template.Members.OfType<ScriptClassProperty>().ForEach(property => AddProperty(classNode, property));
    }

    private static void AddProperty(ClassSyntaxNode classNode, ScriptClassProperty property)
    {
        var propertyNode = new PropertySyntaxNode(property.Name, property.ValueType, property.GetterSourceCode, property.SetterSourceCode);
        classNode.AddSubNode(propertyNode);
        classNode.AddSubNode(new EmptyLineSyntaxNode());
    }

    private void AddMethods(ClassSyntaxNode classNode)
    {
        _template.Members.OfType<ScriptClassMethod>().ForEach(method => AddMethod(classNode, method));
    }

    private static void AddMethod(ClassSyntaxNode classNode, ScriptClassMethod method)
    {
        var methodNode = new MethodSyntaxNode(method.Name, method.SourceCode);
        classNode.AddSubNode(methodNode);
        classNode.AddSubNode(new EmptyLineSyntaxNode());
    }

    private void AddUsings(ClassSyntaxTree syntaxTree)
    {
        var usings = _template.Usings.Concat(_scriptUsings).Distinct().OrderBy(use => use);
        usings.ForEach(use => syntaxTree.RootNode.AddSubNode(new UsingSyntaxNode(use)));
    }
}