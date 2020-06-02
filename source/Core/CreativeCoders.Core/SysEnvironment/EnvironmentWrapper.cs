using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using CreativeCoders.Core.IO;

namespace CreativeCoders.Core.SysEnvironment
{
    [ExcludeFromCodeCoverage]
    public class EnvironmentWrapper : IEnvironment
    {
        public void Exit(int exitCode)
        {
            Environment.Exit(exitCode);
        }

        public string ExpandEnvironmentVariables(string name)
        {
            return Environment.ExpandEnvironmentVariables(name);
        }

        public void FailFast(string message)
        {
            Environment.FailFast(message);
        }

        public void FailFast(string message, Exception exception)
        {
            Environment.FailFast(message, exception);
        }

        public string[] GetCommandLineArgs()
        {
            return Environment.GetCommandLineArgs();
        }

        public string GetEnvironmentVariable(string variable)
        {
            return Environment.GetEnvironmentVariable(variable);
        }

        public string GetEnvironmentVariable(string variable, EnvironmentVariableTarget target)
        {
            return Environment.GetEnvironmentVariable(variable, target);
        }

        public IDictionary<string, object> GetEnvironmentVariables()
        {
            var variables = new Dictionary<string, object>();

            // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
            foreach (DictionaryEntry environmentVariable in Environment.GetEnvironmentVariables())
            {
                var key = environmentVariable.Key.ToString();

                if (key == null)
                {
                    continue;
                }

                variables.Add(key, environmentVariable.Value);
            }

            return variables;
        }

        public IDictionary<string, object> GetEnvironmentVariables(EnvironmentVariableTarget target)
        {
            var variables = new Dictionary<string, object>();

            // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
            foreach (DictionaryEntry environmentVariable in Environment.GetEnvironmentVariables(target))
            {
                var key = environmentVariable.Key.ToString();

                if (key == null)
                {
                    continue;
                }

                variables.Add(key, environmentVariable.Value);
            }

            return variables;
        }

        public string GetFolderPath(Environment.SpecialFolder folder)
        {
            return Environment.GetFolderPath(folder);
        }

        public string GetFolderPath(Environment.SpecialFolder folder, Environment.SpecialFolderOption option)
        {
            return Environment.GetFolderPath(folder, option);
        }

        public string[] GetLogicalDrives()
        {
            return Environment.GetLogicalDrives();
        }

        public void SetEnvironmentVariable(string variable, string value)
        {
            Environment.SetEnvironmentVariable(variable, value);
        }

        public void SetEnvironmentVariable(string variable, string value, EnvironmentVariableTarget target)
        {
            Environment.SetEnvironmentVariable(variable, value, target);
        }

        public string GetAppDirectory()
        {
            return FileSys.Path.GetDirectoryName(GetAppFileName());
        }

        public string GetAppFileName()
        {
            return Process.GetCurrentProcess().MainModule?.FileName;
        }

        public string CommandLine => Environment.CommandLine;

        public string CurrentDirectory
        {
            get => Environment.CurrentDirectory;
            set => Environment.CurrentDirectory = value;
        }

        public int CurrentManagedThreadId => Environment.CurrentManagedThreadId;

        public int ExitCode
        {
            get => Environment.ExitCode;
            set => Environment.ExitCode = value;
        }

        public bool HasShutdownStarted => Environment.HasShutdownStarted;

        public bool Is64BitOperatingSystem => Environment.Is64BitOperatingSystem;

        public bool Is64BitProcess => Environment.Is64BitProcess;

        public string MachineName => Environment.MachineName;

        public string NewLine => Environment.NewLine;

        public OperatingSystem OSVersion => Environment.OSVersion;

        public int ProcessorCount => Environment.ProcessorCount;

        public string StackTrace => Environment.StackTrace;

        public string SystemDirectory => Environment.SystemDirectory;

        public int SystemPageSize => Environment.SystemPageSize;

        public int TickCount => Environment.TickCount;

        public string UserDomainName => Environment.UserDomainName;

        public bool UserInteractive => Environment.UserInteractive;

        public string UserName => Environment.UserName;

        public Version Version => Environment.Version;

        public long WorkingSet => Environment.WorkingSet;
    }
}