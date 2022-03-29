using System;
using System.Collections.Generic;
using System.Text;
using CreativeCoders.Core.Collections;
using CreativeCoders.Core.SysEnvironment;
using CreativeCoders.Scripting.Base;

namespace CreativeCoders.Scripting.CSharp.Preprocessors;

public class UsingsPreprocessor : ISourcePreprocessor
{
    public void Preprocess(ScriptPackage scriptPackage, CSharpScriptClassDefinition classDefinition)
    {
        var usings = new List<string>();
        var sourceCode = new StringBuilder();

        foreach (var line in classDefinition.SourceCode.Split(new[]{Env.NewLine}, StringSplitOptions.None))
        {
            if (line.Trim().StartsWith("using ", StringComparison.InvariantCulture))
            {
                var trimmedLine = line.Trim();
                var endIndex = trimmedLine.IndexOf(";", StringComparison.InvariantCulture);
                var use = trimmedLine.Substring("using ".Length, endIndex - "using ".Length);
                usings.Add(use);
            }
            else
            {
                sourceCode.AppendLine(line);
            }
        }

        classDefinition.SourceCode = sourceCode.ToString();
        classDefinition.Usings.AddRange(usings);
    }
}