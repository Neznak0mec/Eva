
using Desktop.Robot;
using TextCopy;

namespace Eva.backend.KeyboardSimulators;


public class KeyboardSimulator
{
    Robot keyboard = new();
    KeyboardSimulatorWindows buttonMasterWindows = new();
    private char[] keysarr;
    TextCopy.Clipboard _clipboard = new Clipboard();

    private Dictionary<char, Key> _keys = new()
    {
        { 'ƒ', Key.Control },
        { '⟵', Key.Left},
        { '⟶', Key.Right},
        { '⌃', Key.PageUp},
        { '⌄', Key.PageDown},
        { '⟳', Key.Delete},
        { '⟲', Key.Backspace},
        { '⟿',  Key.Shift}
    };

    public KeyboardSimulator()
    {
        keysarr = new []{'ƒ', '⟵', '⟶', '⌃', '⌄', '⟳', '⟲', '⟿','⟰','⟱' } ;
    }

    public void Simulate(string input)
    {
        if (CheckToContains(input))
        {
            List<string> words = input.Split().ToList();
            foreach (var i in words)
            {
                if (!CheckToContains(i))
                {
                    TypeString(i);
                    continue;
                }

                ButtonMaster(i);
            }
        }
        else
        {
            TypeString(input);
        }
    }

    void ButtonMaster(string buttons)
    {
        buttonMasterWindows.TypeString(buttons);
    }

    void TypeString(string input)
    {
        var currentclipboard = _clipboard.GetText();

        _clipboard.SetText(input);

        keyboard.KeyDown(Key.Control);
        keyboard.KeyPress(Key.V);
        keyboard.KeyUp(Key.Control);


        Thread.Sleep(50);
        _clipboard.SetText(currentclipboard ?? "");
    }

    bool CheckToContains(string input) => keysarr.Any(input.Contains);

}