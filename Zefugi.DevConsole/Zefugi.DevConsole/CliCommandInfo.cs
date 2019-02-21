using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Zefugi.DevConsole
{
    public class CliCommandInfo
    {
        public readonly MethodInfo Method;
        public readonly ParameterInfo[] Parameters;
        public readonly Type[] ParameterTypes;

        private CliCommandAttribute _commandAttribute;

        public CliCommandInfo(MethodInfo method)
        {
            Method = method;
            Parameters = method.GetParameters();
            ParameterTypes = new Type[Parameters.Length];
            for (int i = 0; i < Parameters.Length; i++)
                ParameterTypes[i] = Parameters[i].ParameterType;
            if (Parameters.Length < 1 || Parameters[0].ParameterType != typeof(CliInvocationContext))
                throw new Exception("Invalid command format for command " + Method.Name + ". A commands first parameter must be of type CliInvocationContext.");

            for (int i = 1; i < Parameters.Length; i++)
            {
                if (ParameterTypes[i] != typeof(string))
                {
                    MethodInfo parse = ParameterTypes[i].GetMethod("Parse", new Type[] { typeof(string) });
                    if (parse == null ||
                        !parse.IsStatic ||
                        parse.GetParameters().Length != 1 ||
                        parse.GetParameters()[0].ParameterType != typeof(string))
                        throw new Exception("Invalid command argument type '" + ParameterTypes[i].FullName + "'. The type does not have a static Parse method that accepts a string argument.");
                }
            }

            _commandAttribute = Method.GetCustomAttribute<CliCommandAttribute>();
        }

        public string Name { get { return Method.Name.ToLower(); } }

        public string Description
        {
            get { return _commandAttribute.Description; }
        }

        public string Usage
        {
            get { return _commandAttribute.Usage; }
        }

        public bool TryInvoke(CliInvocationContext context, string[] args)
        {
            if (args.Length != Parameters.Length - 1)
            {
                context.Window.WriteError("Usage: " + Usage);
                return false;
            }
            else
            {
                object[] pars = new object[args.Length + 1];
                pars[0] = context;
                for(int i = 1; i <= args.Length; i++)
                {
                    if (ParameterTypes[i] == typeof(string))
                        pars[i] = args[i - 1];
                    else
                    {
                        MethodInfo parse = ParameterTypes[i].GetMethod("Parse", new Type[] { typeof(string) });
                        pars[i] = parse.Invoke(null, new object[] { args[i - 1] });
                    }
                }
                //try { Method.Invoke(null, pars); }
                //catch { return false; }
                Method.Invoke(null, pars);
                return true;
            }
        }
    }
}
