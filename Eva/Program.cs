using Avalonia;
using Avalonia.ReactiveUI;

using System.Globalization;

using Eva;

using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System.Globalization;
using Eva.Parsers;

namespace window;

class Program
{

    public static AppBuilder app;
    [STAThread]
    public static async Task Main(string[] args)
    {
        // подключение логгера
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(theme: AnsiConsoleTheme.Sixteen)
            .WriteTo.File($"logs/ClientLogs{DateTime.Now}.txt")
            .CreateLogger();
        Log.Information("Program Started");

        Task guiTask = Task.Run(() =>
        {
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args); // запуск графического окна
        }); // запуск GUI в отдельном потоке

        await Task.WhenAll(guiTask); // ожидание завершения обоих потоков
    }


    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace()
            .UseReactiveUI();
}
