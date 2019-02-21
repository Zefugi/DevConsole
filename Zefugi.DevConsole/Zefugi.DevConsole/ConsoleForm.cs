using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Zefugi.DevConsole
{
    public delegate void ConsoleEntryDelegate(string command, string[] arguments);
    public delegate void ConsoleTypedEntryDelegate(string command, object[] arguments);

    public partial class ConsoleForm : Form
    {
        public const string VERSION_TEXT = "1.0";

        public ConsoleEntryDelegate OnEntry;
        public ConsoleTypedEntryDelegate OnTypedEntry;

        private static object _createLock = new object();
        private static ConsoleForm _form;

        public Cli Cli;
        public bool UseTypedEntry = false;

        public static ConsoleForm CreateAndRun()
        {
            lock (_createLock)
            {
                Thread t = new Thread(new ThreadStart(Run));
                t.Start();
                while (_form == null || !_form.Created)
                    Thread.SpinWait(1);
                var frm = _form;
                _form = null;
                return frm;
            }
        }

        private static void Run()
        {
            _form = new ConsoleForm();
            Application.Run(_form);
        }

        internal class ConsoleEntry
        {
            public string Text;
            public Color Color;
        }

        private Queue<ConsoleEntry> _entryQueue = new Queue<ConsoleEntry>();

        public ConsoleForm()
        {
            InitializeComponent();

            WriteLine("Zefugi - Dev Console v" + VERSION_TEXT + " by Zefugi (https://github.com/Zefugi/)", Color.BurlyWood);
            WriteLine("Distibuted under an MIT license.", Color.BurlyWood);
            WriteLine("See https://github.com/Zefugi/DevConsole for more info.", Color.BurlyWood);
            WriteLine("");
            WriteLine("Console ready, type 'help' for help.", Color.BurlyWood);
            WriteLine("");
        }

        public void WaitForClose()
        {
            while (Created)
                Thread.Sleep(50);
        }

        public void WriteLine(string text, Color color)
        {
            System.Diagnostics.Debug.WriteLine("{" + text + "}");
            _entryQueue.Enqueue(new ConsoleEntry() {
                Text = text,
                Color = color,
            });
        }

        public void WriteLine(string text) { WriteLine(text, Color.White); }

        public void WriteDebug(string text, object obj = null) { WriteLine(text + (obj == null ? "" : " : " + obj.ToString()), Color.LightCoral); }
        public void WriteInfo(string text) { WriteLine(text, Color.BurlyWood); }
        public void WriteWarning(string text) { WriteLine(text, Color.Gold); }
        public void WriteError(string text) { WriteLine(text, Color.OrangeRed); }
        public void WriteException(string text, Exception ex) { WriteLine(text + " : " + ex, Color.OrangeRed); }

        private void _txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Return)
            {
                string line = _txtInput.Text;
                WriteLine("> " + _txtInput.Text);
                _txtInput.Text = "";

                if(Cli != null)
                {
                    CommandLine.Parse(line, out string command, out string[] arguments);
                    bool foundCmd = false;
                    StringBuilder helpTexts = new StringBuilder();
                    bool printHelpTexts = true;
                    foreach(CliCommandInfo cmd in Cli.Commands)
                    {
                        if(cmd.Name == command)
                        {
                            foundCmd = true;
                            if (cmd.Parameters.Length == arguments.Length + 1)
                            {
                                CliInvocationContext context = new CliInvocationContext(this);
                                if (cmd.TryInvoke(context, arguments))
                                {
                                    printHelpTexts = false;
                                    break;
                                }
                                else
                                {

                                }
                            }
                            else
                            {
                                helpTexts.AppendLine(cmd.Description);
                                helpTexts.AppendLine("Usage: " + cmd.Name + " " + cmd.Usage);
                            }
                        }
                    }
                    if (!foundCmd)
                        WriteError("Invalid command: " + command);
                    else if (printHelpTexts)
                        WriteInfo(helpTexts.ToString());
                }
                else if (UseTypedEntry)
                {
                    CommandLine.ParseWithTypes(line, out string command, out object[] arguments);
                    OnTypedEntry(command, arguments);
                }
                else
                {
                    CommandLine.Parse(line, out string command, out string[] arguments);
                    OnEntry(command, arguments);
                }
            }
        }

        private void _txtInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\n' || e.KeyChar == '\r')
                e.Handled = true;
        }

        private void _logTimer_Tick(object sender, EventArgs e)
        {
            while (_entryQueue.Count != 0)
            {
                var entry = _entryQueue.Dequeue();
                bool scroll = _txtLog.SelectionStart == _txtLog.Text.Length;
                _txtLog.SelectionStart = _txtLog.Text.Length;
                _txtLog.SelectionLength = 0;
                _txtLog.SelectionColor = entry.Color;
                _txtLog.SelectedText = entry.Text.Replace("\n", "\n\t") + "\r\n";
                _txtLog.SelectionStart = _txtLog.Text.Length;
                _txtLog.SelectionLength = 0;
                if (scroll)
                    _txtLog.ScrollToCaret();
            }
        }
    }
}
