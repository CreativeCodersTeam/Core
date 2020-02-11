using System;

namespace CreativeCoders.Core.Pipelining
{
    public class Stage<TInput, TOutput> : IStage<TInput, TOutput>
    {
        public Func<TInput, TOutput> StageWorker { get; set; }

        public object ProcessData(object inputData)
        {
            if (!(inputData is TInput))
            {
                throw new InvalidOperationException();
            }
            return StageWorker((TInput)inputData);
        }

        public bool OutputIsOfType(Type type)
        {
            return type == typeof(TOutput);
        }

        public bool RunParallel { get; set; }

        public int MaxDegreeOfParallelism { get; set; }
    }
}
