using System;

namespace CreativeCoders.Core.Pipelining
{
    public interface IStage
    {
        object ProcessData(object inputData);

        bool OutputIsOfType(Type type);

        bool RunParallel { get; set; }

        int MaxDegreeOfParallelism { get; set; }
    }
}
