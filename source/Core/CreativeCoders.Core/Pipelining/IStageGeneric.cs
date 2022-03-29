using System;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Pipelining;

[PublicAPI]
public interface IStage<TInput, TOutput> : IStage
{
    Func<TInput, TOutput> StageWorker { get; set; }
}
