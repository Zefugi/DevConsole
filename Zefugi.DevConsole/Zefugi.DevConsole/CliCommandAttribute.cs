using System;

namespace Zefugi.DevConsole
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CliCommandAttribute : Attribute
    {
        public string Description { get; internal set; }
        public string Usage { get; internal set; }

        public CliCommandAttribute(
            string description,
            string usage
            )
        {
            Description = description;
            Usage = usage;
        }
    }
}
