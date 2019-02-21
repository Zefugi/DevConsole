# DevConsole
A proper command line console for developing and debugging .NET applications.

![Screenshot](https://github.com/Zefugi/DevConsole/blob/master/Screenshot.png "Screenshot")

## Example:

```C#
static class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        var console = ConsoleForm.CreateAndRun();
        console.Cli = new Cli();
        // Add exit, echo, help and help <command name> commands.
        console.Cli.AddCommandsFrom(typeof(BasicCliCommands));
        // Add my custom commands.
        console.Cli.AddCommandsFrom(typeof(MyCommands));
        console.WaitForClose();
    }
}

static class MyCommands
{
    public class BasicCliCommands
    {
        [CliCommand("This will gree you.", "<your name>")]
        public static void Hello(CliInvocationContext context, string name)
        {
            context.Window.WriteInfo("Hello " + name);
        }

        [CliCommand("This will add two ints.", "<your name>")]
        public static void Add(CliInvocationContext context, int a, int b)
        {
            context.Window.WriteInfo(
                a + " plus " + b + " is equal to " + (a + b)
            );
        }
    }
}
```

