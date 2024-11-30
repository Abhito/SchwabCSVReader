using System;
using System.Runtime.InteropServices;

namespace SchwabCSVReader
{
    public static class ConsoleHelper
    {
        private const int ATTACH_PARENT_PROCESS = -1;

        [DllImport("kernel32.dll")]
        private static extern bool AttachConsole(int dwProcessId);

        /// <summary>
        /// Redirects the console output of the current process to the parent process.
        /// </summary>
        /// <remarks>
        /// Must be called before calls to <see cref="Console.WriteLine()" />.
        /// </remarks>
        public static void AttachToParentConsole()
        {
            AttachConsole(ATTACH_PARENT_PROCESS);
        }
    }
}