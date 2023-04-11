using Eva.backend.KeyboardSimulators;
using Serilog;
using Eva.KeyboardSimulators;
using window.ViewModels;

namespace Eva;

class MessageHandler
{
        private NaturalLanguageConverter _conventer = new();
        private KeyboardSimulator _simulator = new();
        MainWindowViewModel window;
        public MessageHandler(MainWindowViewModel window)
        {
                this.window = window;
        }
        public async Task Handle(string message)
        {
                if (message.StartsWith("partial:"))
                        return;

                message = message.Replace("result:", "");
                var resault = _conventer.Convert(message);
                Log.Information("Converted to: {Resault}",resault);
                window.ShowLast = "message";
                _simulator.Simulate(resault);
        }
}
