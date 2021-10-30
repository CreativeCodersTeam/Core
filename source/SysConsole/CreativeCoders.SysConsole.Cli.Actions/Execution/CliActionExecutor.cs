using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using CreativeCoders.Core.Collections;

namespace CreativeCoders.SysConsole.Cli.Actions.Execution
{
    public class CliActionExecutor : ICliActionExecutor
    {
        private IList<CliActionExecutionMiddlewareBase> _middlewareList;

        public CliActionExecutor()
        {
            _middlewareList = new List<CliActionExecutionMiddlewareBase>();
        }


        public async Task<int> ExecuteAsync(string[] args)
        {
            var context = new CliActionContext(new CliActionRequest(args));

            foreach (var middleware in _middlewareList)
            {
                await middleware.InvokeAsync(context);
            }

            return context.ReturnCode;
        }
    }
}
