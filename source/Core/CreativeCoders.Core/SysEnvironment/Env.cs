using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace CreativeCoders.Core.SysEnvironment
{
    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public static class Env
    {
        private static IEnvironment EnvironmentInstance = new EnvironmentWrapper();

        public static void SetEnvironmentImpl(IEnvironment environment)
        {
            Ensure.IsNotNull(environment, nameof(environment));

            EnvironmentInstance = environment;
        }

        public static void Exit(int exitCode)
        {
            EnvironmentInstance.Exit(exitCode);
        }

        public static string ExpandEnvironmentVariables(string name)
        {
            return EnvironmentInstance.ExpandEnvironmentVariables(name);
        }

        public static void FailFast(string message)
        {
            EnvironmentInstance.FailFast(message);
        }

        public static void FailFast(string message, Exception exception)
        {
            EnvironmentInstance.FailFast(message, exception);
        }

        public static string[] GetCommandLineArgs()
        {
            return EnvironmentInstance.GetCommandLineArgs();
        }

        public static string GetEnvironmentVariable(string variable)
        {
            return EnvironmentInstance.GetEnvironmentVariable(variable);
        }

        public static string GetEnvironmentVariable(string variable, EnvironmentVariableTarget target)
        {
            return EnvironmentInstance.GetEnvironmentVariable(variable, target);
        }

        public static IDictionary<string, object> GetEnvironmentVariables()
        {
            return EnvironmentInstance.GetEnvironmentVariables();
        }

        public static IDictionary<string, object> GetEnvironmentVariables(EnvironmentVariableTarget target)
        {
            return EnvironmentInstance.GetEnvironmentVariables(target);
        }

        public static string GetFolderPath(Environment.SpecialFolder folder)
        {
            return EnvironmentInstance.GetFolderPath(folder);
        }

        public static string GetFolderPath(Environment.SpecialFolder folder, Environment.SpecialFolderOption option)
        {
            return EnvironmentInstance.GetFolderPath(folder, option);
        }

        public static string[] GetLogicalDrives()
        {
            return EnvironmentInstance.GetLogicalDrives();
        }

        public static void SetEnvironmentVariable(string variable, string value)
        {
            EnvironmentInstance.SetEnvironmentVariable(variable, value);
        }

        public static void SetEnvironmentVariable(string variable, string value, EnvironmentVariableTarget target)
        {
            EnvironmentInstance.SetEnvironmentVariable(variable, value, target);
        }

        public static string GetAppDirectory()
        {
            return EnvironmentInstance.GetAppDirectory();
        }

        public static string GetAppFileName()
        {
            return EnvironmentInstance.GetAppFileName();
        }

        public static string CommandLine => EnvironmentInstance.CommandLine;

        public static string CurrentDirectory
        {
            get => EnvironmentInstance.CurrentDirectory;
            set => EnvironmentInstance.CurrentDirectory = value;
        }

        public static int CurrentManagedThreadId => EnvironmentInstance.CurrentManagedThreadId;

        public static int ExitCode
        {
            get => EnvironmentInstance.ExitCode;
            set => EnvironmentInstance.ExitCode = value;
        }

        public static bool HasShutdownStarted => EnvironmentInstance.HasShutdownStarted;

        public static bool Is64BitOperatingSystem => EnvironmentInstance.Is64BitOperatingSystem;

        public static bool Is64BitProcess => EnvironmentInstance.Is64BitProcess;

        public static string MachineName => EnvironmentInstance.MachineName;

        public static string NewLine => EnvironmentInstance.NewLine;

        public static OperatingSystem OSVersion => EnvironmentInstance.OSVersion;

        public static int ProcessorCount => EnvironmentInstance.ProcessorCount;

        public static string StackTrace => EnvironmentInstance.StackTrace;

        public static string SystemDirectory => EnvironmentInstance.SystemDirectory;

        public static int SystemPageSize => EnvironmentInstance.SystemPageSize;

        public static int TickCount => EnvironmentInstance.TickCount;

        public static string UserDomainName => EnvironmentInstance.UserDomainName;

        public static bool UserInteractive => EnvironmentInstance.UserInteractive;

        public static string UserName => EnvironmentInstance.UserName;

        public static Version Version => EnvironmentInstance.Version;

        public static long WorkingSet => EnvironmentInstance.WorkingSet;
    }
}