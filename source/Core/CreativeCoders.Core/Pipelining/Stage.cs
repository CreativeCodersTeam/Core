using System;

namespace CreativeCoders.Core.Pipelining
{
    public class Stage<TInput, TOutput> : IStage<TInput, TOutput>
    {
        public Func<TInput, TOutput> StageWorkerFunc { get; set; }

        public object ProcessData(object inputData)
        {
            if (!(inputData is TInput))
            {
                throw new InvalidOperationException();
            }
            return StageWorkerFunc((TInput)inputData);
        }

        public bool OutputIsOfType(Type type)
        {
            return type == typeof(TOutput);
        }

        public bool RunParallel { get; set; }

        public int MaxDegreeOfParallelism { get; set; }
    }
}
