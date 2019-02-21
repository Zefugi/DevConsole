using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Zefugi.DevConsole
{
    static class Program
    {
        static ConsoleForm _console;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //TextedExample();
            //TypedExample();
            TestCli();
        }

        static void TestCli()
        {
            _console = ConsoleForm.CreateAndRun();
            _console.Cli = new Cli();
            _console.Cli.AddCommandsFrom(typeof(BasicCliCommands));
            _console.WaitForClose();
        }

        static void TextedExample()
        {
            _console = ConsoleForm.CreateAndRun();
            _console.OnEntry = OnConsoleEntry;
            _console.WaitForClose();
            var rnd = new Random();
            while (_console.Created)
            {
                switch (rnd.Next(0, 5))
                {
                    case 0:
                        _console.WriteDebug("This is for debugging");
                        break;
                    case 1:
                        _console.WriteInfo("This is some info");
                        break;
                    case 2:
                        _console.WriteWarning("This is a warning");
                        break;
                    case 3:
                        _console.WriteError("This is an error");
                        break;
                    case 4:
                        _console.WriteException("This is an exception", new NotImplementedException());
                        break;
                }
                Thread.Sleep(2500);
            }
        }

        static void TypedExample()
        {
            _console = ConsoleForm.CreateAndRun();
            _console.UseTypedEntry = true;
            _console.OnTypedEntry = OnTypedConsoleEntry;
            _console.WaitForClose();
        }

        static void OnConsoleEntry(string command, string[] arguments)
        {
            switch (command)
            {
                case "exit":
                    _console.Dispose();
                    break;

                default:
                    _console.WriteError("Unknown command " + command + (arguments.Length == 0 ? "" : " {" + string.Join(", ", arguments) + "}"));
                    break;
            }
        }

        static void OnTypedConsoleEntry(string command, object[] arguments)
        {
            switch (command)
            {
                case "exit":
                    _console.Dispose();
                    break;

                default:
                    var str = new StringBuilder();
                    for (int i = 0; i < arguments.Length; i++)
                    {
                        if (i != 0)
                            str.Append(", ");
                        str.Append(arguments[i].GetType().Name);
                    }
                    _console.WriteDebug("Types", str);
                    break;
            }
        }
    }
}
