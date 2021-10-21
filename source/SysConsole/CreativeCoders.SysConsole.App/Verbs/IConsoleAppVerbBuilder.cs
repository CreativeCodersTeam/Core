namespace CreativeCoders.SysConsole.App.Verbs
{
    public interface IConsoleAppVerbBuilder
    {
        IConsoleAppVerbBuilder AddVerb<TVerb>()
            where TVerb : class, IVerb;

        IConsoleAppVerbBuilder AddErrors<TErrorHandler>()
            where TErrorHandler : IVerbParserErrorHandler;
    }
}
