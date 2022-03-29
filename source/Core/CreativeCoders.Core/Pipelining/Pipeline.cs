using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Pipelining;

[PublicAPI]
public class Pipeline<TInput, TOutput> : IPipeline<TInput, TOutput>
{
    private readonly IList<IStage> _stages;

    public Pipeline()
    {
        _stages = new List<IStage>();
    }

    public Func<IEnumerable<TInput>> Input { get; set; }

    public Action<TOutput> Output { get; set; }

    public bool RunStageParallel { get; set; }

    public IStage<TStageInput, TStageOutput> AddStage<TStageInput, TStageOutput>(Func<TStageInput, TStageOutput> stageWorker)
    {
        Ensure.IsNotNull(stageWorker, nameof(stageWorker));
        Ensure.That(!(_stages.Count == 0 && typeof(TStageInput) != typeof(TInput)), nameof(TStageInput));
        Ensure.That(_stages.LastOrDefault()?.OutputIsOfType(typeof(TStageInput)) != false, nameof(TStageInput));

        var stage = new Stage<TStageInput, TStageOutput> { StageWorker = stageWorker };
        _stages.Add(stage);
        return stage;
    }

    public IEnumerable<IStage> Stages => _stages;

    public void Run()
    {
        var stageRunners = BuildStageRunners();

        var taskFactory = new TaskFactory(TaskCreationOptions.LongRunning, TaskContinuationOptions.None);
        var tasks = stageRunners.Select(runner => taskFactory.StartNew(runner.Run));

        Task.WaitAll(tasks.ToArray());
    }

    private IEnumerable<StageRunner> BuildStageRunners()
    {
        return ConnectStageRunners(_stages
            .Select(stage => new StageRunner(stage)));
    }

    private IEnumerable<StageRunner> ConnectStageRunners(IEnumerable<StageRunner> runners)
    {
        var stageRunners = runners as StageRunner[] ?? runners.ToArray();
            
        var firstRunner = stageRunners.FirstOrDefault();
        if (firstRunner == null)
        {
            return stageRunners;
        }
        firstRunner.Input = () => Input().Cast<object>();

        foreach (var runner in stageRunners.Skip(1))
        {
            ConnectStageRunners(firstRunner, runner);
            firstRunner = runner;
        }

        var lastRunner = stageRunners.LastOrDefault();
        if (lastRunner != null)
        {
            lastRunner.Output = data => Output((TOutput) data);
        }

        return stageRunners;
    }        

    private static void ConnectStageRunners(StageRunner firstRunner, StageRunner secondRunner)
    {
        var collection = new BlockingCollection<object>();
        firstRunner.Output = data => collection.Add(data);
        secondRunner.Input = () => collection.GetConsumingEnumerable();
        firstRunner.Finished = () => collection.CompleteAdding();
    }
}