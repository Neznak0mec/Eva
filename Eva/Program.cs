using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Eva;
class Program
{
    static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(theme: AnsiConsoleTheme.Sixteen)
            .WriteTo.File("ClientLogs.logs")
            .CreateLogger();
        Log.Information("Program Started");
        
        // Создание и подключение клиента
        Client client = Client.Instance;
        client.Connect("localhost", 5555);
        

        // Вывод сообщений от сервера
        MessageHandler printer = MessageHandler.Instance;
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        // Запуск асинхронного получения сообщений
        Task receiveTask = client.ReceiveMessagesAsync(printer, cancellationTokenSource.Token);

        Console.ReadLine(); // Ожидание ввода для завершения
        cancellationTokenSource.Cancel();

        await receiveTask; // Ожидание завершения получения сообщений
    }
}
