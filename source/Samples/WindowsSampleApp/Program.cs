using System;
using CreativeCoders.Core;
using CreativeCoders.Windows.Window;

namespace WindowsSampleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var win32Windows = new Win32Windows();

            var windows = win32Windows.EnumerateWindows();

            windows.ForEach(x => Console.WriteLine($"{x.WindowHandle}: {x.Title}"));

            Console.WriteLine("Press key to exit...");
            Console.ReadKey();
        }
    }
}
