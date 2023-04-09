using Avalonia.Controls;
using Avalonia.Input;
using Eva.backend.KeyboardSimulators;
using ReactiveUI;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Eva;

class BackEnd
{
    public async Task Start()
    {
        
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
