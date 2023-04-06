using System.Runtime.InteropServices;
using Serilog;
using Desktop.Robot;
using Desktop.Robot.Extensions;
using TextCopy;

namespace Eva.KeyboardSimulators;


public class KeyboardSimulator
{
    Robot keyboard = new();
    TextCopy.Clipboard _clipboard = new Clipboard();

    public void Simulate(string input)
    {
        var currentclipboard = _clipboard.GetText();
        _clipboard.SetText(input);
        
        keyboard.KeyDown(Key.Control);
        keyboard.KeyPress(Key.V);
        keyboard.KeyUp(Key.Control);
        
        _clipboard.SetText(currentclipboard);
    }
    
}