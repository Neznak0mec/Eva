using Serilog;
using Serilog.Events;

namespace Eva;

class MessageHandler
{
        private static readonly Lazy<MessageHandler> _instance = new(() => new MessageHandler());

        public static MessageHandler Instance => _instance.Value;

        private MessageHandler() { }

        public void Print(string message)
        {
            
        }
}
