using System;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Pipelining
{
    [PublicAPI]
    public interface IStage
    {
        object ProcessData(object inputData);

        bool OutputIsOfType(Type type);

        bool RunParallel { get; set; }

        int MaxDegreeOfParallelism { get; set; }
    }
}
