using System;
using CreativeCoders.Core.Collections;
using CreativeCoders.IO;
using CreativeCoders.Windows.Window;

namespace WindowsSampleApp;

public static class Program
{
    public static void Main()
    {
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
