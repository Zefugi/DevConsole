using System;

namespace Zefugi.DevConsole
{
    public class CliInvocationContext
    {
        public readonly ConsoleForm Window;
        public CliInvocationContext(ConsoleForm window)
        {
            Window = window;
        }
    }
}
