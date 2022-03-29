using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CreativeCoders.Core.Collections;

namespace CreativeCoders.Core.Pipelining;

internal class StageRunner
{
    private readonly IStage _stage;

    public StageRunner(IStage stage)
    {
        Ensure.IsNotNull(stage, nameof(stage));

        _stage = stage;
    }

    public Func<IEnumerable<object>> Input { get; set; }

    public Action<object> Output { get; set; }

    public Action Finished { get; set; }

    public void Run()
    {
        if (Input == null || Output == null)
        {
            throw new InvalidOperationException();
        }

        var input = Input();
        try
        {
            if (_stage.RunParallel)
            {
                if (_stage.MaxDegreeOfParallelism > 0)
                {
                    Parallel.ForEach(input,
                        new ParallelOptions {MaxDegreeOfParallelism = _stage.MaxDegreeOfParallelism},
                        ProcessInputData);
                }
                else
                {
                    Parallel.ForEach(input, ProcessInputData);
                }
            }
            else
            {
                input.ForEach(ProcessInputData);
            }
        }
        finally
        {
            Finished?.Invoke();
        }
    }

    private void ProcessInputData(object inputData)
    {
        var outputData = _stage.ProcessData(inputData);
        Output(outputData);
    }
}