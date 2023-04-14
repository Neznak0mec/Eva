using Avalonia.Controls;
using Avalonia.Input;
using Eva.backend.KeyboardSimulators;
using ReactiveUI;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using window;
using window.ViewModels;

namespace Eva;

class BackEnd
{
    MainWindowViewModel window;
    public BackEnd(MainWindowViewModel window)
    {
        this.window = window;
    }
    public async Task Start()
    {

        // Обработка сообщений от сервера
        MessageHandler handler = new MessageHandler(window);

        // Создание и подключение клиента
        Client client = new Client(handler);
        await client.Connect("localhost", 5555);

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        // Запуск асинхронного получения сообщений
        Task receiveTask = client.ReceiveMessagesAsync(cancellationTokenSource.Token);

        Console.ReadLine(); // Ожидание ввода для завершения
        cancellationTokenSource.Cancel();

        await receiveTask; // Ожидание завершения получения сообщений
    }
}
