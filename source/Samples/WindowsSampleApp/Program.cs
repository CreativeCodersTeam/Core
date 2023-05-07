using System;
using System.Threading.Tasks;
using CreativeCoders.Core.IO;
using CreativeCoders.IO;
using CreativeCoders.IO.Archives;

namespace WindowsSampleApp;

public static class Program
{
    public static async Task Main()
    {
        var outputStream = FileSys.File.Create(@"c:\temp\test.tar.gz");

        var tarArchiveWriter = new DefaultTarArchiveWriterBuilder()
            .WithGZip()
            .Build(outputStream);

        //await tarArchiveWriter.AddFileAsync("test1.txt", @"c:\temp\test1.txt").ConfigureAwait(false);

        await tarArchiveWriter.AddFromDirectoryAsync(@"c:\temp\simbasrv", @"c:\temp\").ConfigureAwait(false);

        await tarArchiveWriter.DisposeAsync();

        return;

        var archiveCreator = new TarArchiveCreator()
            .SetArchiveFileName(@"c:\temp\test.tar")
            .AddFromDirectory(@"c:\temp\simbasrv", "*.*", true);

        archiveCreator.Create();

        archiveCreator
            .SetArchiveFileName(@"c:\temp\test.tar.gz")
            .Create(true);

        // var win32Windows = new Win32Windows();
        //
        // var windows = win32Windows.EnumerateWindows();
        //
        // windows.ForEach(x => Console.WriteLine($"{x.WindowHandle}: {x.Title}"));

        Console.WriteLine("Press key to exit...");
        Console.ReadKey();
    }
}
