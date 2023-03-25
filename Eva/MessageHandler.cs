namespace Eva;

class MessageHandler
{

        public void Handle(string message)
        {
                if (message.StartsWith("partial:"))
                        return;

                message.Replace("result:", "");
                //todo: парсинг сообщения

        }
}
