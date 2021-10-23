namespace CreativeCoders.SysConsole.App.Verbs
{
    public interface IConsoleAppVerbsBuilder
    {
        IConsoleAppVerbsBuilder AddVerb<TVerb>()
            where TVerb : class, IVerb;

        IConsoleAppVerbsBuilder AddErrors<TErrorHandler>()
            where TErrorHandler : IVerbParserErrorHandler;
    }
}
