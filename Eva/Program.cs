using NAudio.Wave;
    using Vosk;
    
namespace Eva;


class Program
{

    static void Main(string[] args)
    {
        
        var model = new Model("model/en");
        var tools = new Tools(model);
        tools.Start();
    }
}