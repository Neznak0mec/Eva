using System;

namespace Eva;

class CommandStack
{
    static Stack<string> commandStack = new Stack<string>(){};
    static int size = 15;

    public static int Size { get => size; set => size = value; }

    public static void AddCommand(string command)
    {
        if(!command.Contains('✕'))
            if(commandStack.Count < Size)
                commandStack.Push(command);
            else
            {
                var tmp = new string[size];
                commandStack.CopyTo(tmp,0);
                commandStack.Clear();
                for(int i = size - 2; i >= 0; i--)
                {
                    commandStack.Push(tmp[i]);
                }
                commandStack.Push(command);
            }
    }

    public static string RemoveCommand()
    {
        if(commandStack.Count > 0)
        {
            return commandStack.Pop();
        }
        else
        {
            return string.Empty;
        }
    }
}
