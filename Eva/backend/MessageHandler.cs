using Eva.backend.KeyboardSimulators;
using Serilog;
using Eva.KeyboardSimulators;

namespace Eva;

class MessageHandler
{
        private NaturalLanguageConverter _conventer = new();
        private KeyboardSimulator _simulator = new();

        public async Task Handle(string message)
        {
                if (message.StartsWith("partial:"))
                        return;

                message = message.Replace("result:", "");
                var resault = _conventer.Convert(message);
                Log.Information("Converted to: {Resault}",resault);
                _simulator.Simulate(resault);
        }
}
