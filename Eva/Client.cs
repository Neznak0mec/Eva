using System.Net.Sockets;
using System.Text;
using Serilog;
using Serilog.Events;

namespace Eva;

class Client
{
    public string message;
    private TcpClient _tcpClient;
    private NetworkStream _stream;
    private byte[] _data = new byte[2048];

    private static readonly Lazy<Client> _instance = new(() => new Client());

    public static Client Instance => _instance.Value;

    private Client()
    {
    }

    public void Connect(string host, int port)
    {
        try
        {
            _tcpClient = new TcpClient();
            _tcpClient.Connect(host, port);
            _stream = _tcpClient.GetStream();
            Log.Information($"Connected to server {host}:{port}");
        }
        catch (Exception e)
        {
            Log.Error($"Failed to connect to the server, exception:\n{e.Message}");
            throw;
        }

    }

    public async Task ReceiveMessagesAsync(MessageHandler printer, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            int bytes = await _stream.ReadAsync(_data, 0, _data.Length, cancellationToken);
            message = Encoding.UTF8.GetString(_data, 0, bytes);
            Log.Information($"Received message: {message}");

        }
        Log.Information($"Disconnected from server");
    }
}