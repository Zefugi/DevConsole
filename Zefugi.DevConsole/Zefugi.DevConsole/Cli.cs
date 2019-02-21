using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Zefugi.DevConsole
{
    public class Cli
    {
        public readonly List<CliCommandInfo> Commands = new List<CliCommandInfo>();

        public void AddCommandsFrom(Type type)
        {
            foreach(MethodInfo method in type.GetMethods())
            {
                if (method.GetCustomAttribute<CliCommandAttribute>() == null)
                    continue;
                if (!method.IsStatic)
                    throw new Exception("Only static methods can be CliCommands.");
                Commands.Add(new CliCommandInfo(method));
            }
        }
    }
}
