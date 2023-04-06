using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Eva;
class BackEnd
{
    public static async Task Start()
    {
        // подключение логгера
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(theme: AnsiConsoleTheme.Sixteen)
            .WriteTo.File($"logs/ClientLogs{DateTime.Now}.txt")
            .CreateLogger();
        Log.Information("Program Started");
        
        
        // Обработка сообщений от сервера
        MessageHandler handler = new MessageHandler();
        
        // Создание и подключение клиента
        Client client = new Client(handler);
        client.Connect("localhost", 5555);
        
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        
        // Запуск асинхронного получения сообщений
        Task receiveTask = client.ReceiveMessagesAsync(cancellationTokenSource.Token);
        
        Console.ReadLine(); // Ожидание ввода для завершения
        cancellationTokenSource.Cancel();
        
        await receiveTask; // Ожидание завершения получения сообщений
    }
}
