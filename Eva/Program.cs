using Avalonia;
using Avalonia.ReactiveUI;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace window;

class Program
{

    public static AppBuilder app;
    [STAThread]
    public static async Task Main(string[] args)
    {
        // подключение логгера
        if (OperatingSystem.IsWindows())
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(theme: AnsiConsoleTheme.Sixteen)
                .WriteTo.RollingFile(AppDomain.CurrentDomain.BaseDirectory + "\\logs\\ClientLogs-{Date}.log")
                .CreateLogger();
        }
        else if (OperatingSystem.IsLinux())
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(theme: AnsiConsoleTheme.Sixteen)
                .WriteTo.File($"logs/ClientLogs{DateTime.Now}.log")
                .CreateLogger();
        }
        else
        {
            throw new Exception("Unsupported OS");
        }
        
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