namespace CreativeCoders.Cli.Hosting;

public static class CliExitCodes
{
    public const int Success = 0;

    public const int CommandNotFound = int.MinValue;

    public const int CommandCreationFailed = int.MinValue + 1;

    public const int CommandResultUnknown = int.MinValue + 2;
}
