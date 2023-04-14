using System.Net.Sockets;
using System.Text;
using Serilog;
using NAudio.Wave;

namespace Eva;

class Client
{
    public string message;
    private TcpClient _tcpClient;
    private NetworkStream _stream;
    private byte[] _data = new byte[2048];
    private MessageHandler _handler;
    string serverHost = "127.0.0.1";
    int serverPort = 55555;

    public Client(MessageHandler handler)
    {
        _tcpClient = new TcpClient();
        _handler = handler;
    }

    public async Task Connect(string host, int port)
    {
        try
        {
            await _tcpClient.ConnectAsync(serverHost, serverPort);
            Console.WriteLine($"Подключено к серверу {serverHost}:{serverPort}");

            // Запуск асинхронных функций отправки и приема данных
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;

            Task sendTask = SendAudioAsync(_tcpClient, cancellationToken);
            Task receiveTask = ReceiveMessagesAsync(cancellationToken);

            // Ожидание окончания работы асинхронных функций
            await Task.WhenAll(sendTask, receiveTask);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
        finally
        {
            // Закрытие клиентского соединения
            _tcpClient.Close();
        }

    }

    public async Task ReceiveMessagesAsync(CancellationToken cancellationToken)
    {
        while(true)
        {
            int bytes;
            try {
                bytes = await _stream.ReadAsync(_data, cancellationToken);
            }
            catch (Exception operationCanceledException) {
                Log.Information($"client\tDisconnected from server");
                return;
            }
            
            message = Encoding.UTF8.GetString(_data, 0, bytes);
            Log.Information("client\tReceived message: {Message}", message);
            await _handler.Handle(message);
        }   

    }
    
    async Task SendAudioAsync(TcpClient client, CancellationToken cancellationToken)
    {
        _stream = client.GetStream();
        using (BinaryWriter writer = new BinaryWriter(_stream))
        {
            // Создание объекта для записи звука с микрофона
            WaveInEvent waveIn = new WaveInEvent();
            waveIn.WaveFormat = new WaveFormat(16000, 16, 1); // Формат звука (16000 Гц, 16 бит, 1 канал)

            // Обработчик события получения звука с микрофона
            waveIn.DataAvailable += (sender, e) =>
            {
                // Отправка буфера на сервер
                writer.Write(e.Buffer, 0, e.BytesRecorded);
            };

            // Начало записи звука с микрофона
            waveIn.StartRecording();

            // Ожидание окончания записи (можно заменить на свой механизм остановки)
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(100);
            }

            // Остановка записи звука с микрофона
            waveIn.StopRecording();
        }
    }
}
