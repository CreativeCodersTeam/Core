using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.App.Verbs
{
    public abstract class VerbBase<TOptions> : IVerb
    {
        protected VerbBase(TOptions options)
        {
            Options = options;
        }

        public abstract Task<int> ExecuteAsync();

        [UsedImplicitly]
        public static Type OptionsType => typeof(TOptions);

        protected TOptions Options { get; }
    }
}