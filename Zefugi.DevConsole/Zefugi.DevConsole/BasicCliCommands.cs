using System;
using System.Text;

namespace Zefugi.DevConsole
{
    public class BasicCliCommands
    {
        [CliCommand("This will close the console.", "")]
        public static void Exit(CliInvocationContext context)
        {
            context.Window.Close();
        }

        [CliCommand("This will print out the given text.", "<text>")]
        public static void Echo(CliInvocationContext context, string text)
        {
            context.Window.WriteInfo(text);
        }

        [CliCommand("Display the help text for this CLI.", "")]
        public static void Help(CliInvocationContext context)
        {
            var helpText = new StringBuilder();
            helpText.AppendLine("Zefugi - Dev Console - Usage help:");
            helpText.AppendLine("This is a simple command parsing console. Commands come in the format of a command name (single word) and no or a number of arguments.");
            helpText.AppendLine("Arguments can either be a string (use \" escape characters to include spaces) or a parsable string that will result in an object being parsed to the command.");
            helpText.AppendLine();
            helpText.AppendLine("To get help on a specific command type 'help <commandName>'");
            helpText.AppendLine();
            helpText.AppendLine("For more information see http://github.com/Zefugi/DevConsole");
            context.Window.WriteInfo(helpText.ToString());
        }

        [CliCommand("Display the help text for a given command.", "<command name>")]
        public static void Help(CliInvocationContext context, string commandName)
        {
            var helpTexts = new StringBuilder();
            foreach (CliCommandInfo cmd in context.Window.Cli.Commands)
            {
                if (cmd.Name == commandName)
                {
                    helpTexts.AppendLine(cmd.Description);
                    helpTexts.AppendLine("Usage: " + cmd.Name + " " + cmd.Usage);
                }
            }
            if (helpTexts.Length == 0)
                context.Window.WriteInfo("Unknown command " + commandName);
            else
                context.Window.WriteInfo("Help for command '" + commandName + "':\n" + helpTexts.ToString());
        }
    }
}
