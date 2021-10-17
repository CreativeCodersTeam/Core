namespace CreativeCoders.SysConsole.App.Verbs
{
    public interface IConsoleApplicationVerbBuilder
    {
        IConsoleApplicationVerbBuilder AddVerb<TVerb>()
            where TVerb : class, IVerb;

        IConsoleApplicationVerbBuilder AddErrors<TErrorHandler>()
            where TErrorHandler : IVerbParserErrorHandler;
    }
}
