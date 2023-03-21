
using System.Net.Sockets;
using System.Text;

namespace Eva;
class Program
{
        static void Main(string[] args)
        {
                // Подключение к серверу
                TcpClient client = new TcpClient();
                client.Connect("localhost", 5555);
                Console.WriteLine("Connected to server");

                // Получение данных от сервера
                byte[] data = new byte[2048];
                NetworkStream stream = client.GetStream();

                while (true)
                {
                        int bytes = stream.Read(data, 0, data.Length);
                        string message = Encoding.UTF8.GetString(data, 0, bytes);
                        Console.WriteLine($"Received random number: {message}");
                }
        }
}