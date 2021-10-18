using System;
using System.Threading.Tasks;
using CreativeCoders.Core;

namespace CreativeCoders.SysConsole.App.Verbs
{
    internal class DelegateMain : IMain
    {
        private readonly Func<Task<int>> _executeAsync;

        public DelegateMain(Func<Task<int>> executeAsync)
        {
            _executeAsync = Ensure.Argument(executeAsync, nameof(executeAsync)).NotNull();
        }
        
        public async Task<int> ExecuteAsync()
        {
            return await _executeAsync().ConfigureAwait(false);
        }
    }
}
