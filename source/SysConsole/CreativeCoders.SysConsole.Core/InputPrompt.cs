using CreativeCoders.Core;
using CreativeCoders.SysConsole.Core.Abstractions;

namespace CreativeCoders.SysConsole.Core
{
    public class InputPrompt<T>
    {
        private readonly ISysConsole _sysConsole;

        private string _promptText = string.Empty;

        public InputPrompt(ISysConsole sysConsole)
        {
            _sysConsole = Ensure.NotNull(sysConsole, nameof(sysConsole));
        }

        public InputPrompt<T> PromptText(string promptText)
        {
            _promptText = promptText;

            return this;
        }

        public T? Show()
        {
            _sysConsole.Write(_promptText);

            return default;
        }
    }
}
