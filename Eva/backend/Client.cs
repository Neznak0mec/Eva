using System.Net.Sockets;
using System.Text;
using Serilog;

namespace Eva;

class Client
{
    public string message;
    private TcpClient _tcpClient;
    private NetworkStream _stream;
    private byte[] _data = new byte[2048];
    private MessageHandler _handler;

    public Client(MessageHandler handler)
    {
        _tcpClient = new TcpClient();
        _handler = handler;
    }

    public void Connect(string host, int port)
    {
        try
        {
            _tcpClient.Connect(host, port);
            _stream = _tcpClient.GetStream();
            Log.Information("Connected to server {Host}:{Port}", host, port);
        }
        catch (Exception e)
        {
            Log.Error("Failed to connect to the server, exception:\\n{EMessage}", e.Message);
            throw;
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
                Log.Information($"Disconnected from server");
                return;
            }
            
            message = Encoding.UTF8.GetString(_data, 0, bytes);
            Log.Information("Received message: {Message}", message);
            _handler.Handle(message);
        }   
        
    }
}