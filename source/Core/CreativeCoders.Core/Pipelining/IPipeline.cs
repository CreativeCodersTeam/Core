using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Pipelining;

[PublicAPI]
public interface IPipeline<TInput, TOutput>
{
    Func<IEnumerable<TInput>> Input { get; set; }

    Action<TOutput> Output { get; set; }

    IStage<TStageInput, TStageOutput> AddStage<TStageInput, TStageOutput>(Func<TStageInput, TStageOutput> stageWorker);

    IEnumerable<IStage> Stages { get; }

    void Run();
}