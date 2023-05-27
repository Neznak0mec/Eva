using Serilog;
using WindowsInput;
using WindowsInput.Native;

namespace Eva.backend.KeyboardSimulators;

public class KeyboardSimulatorWindows
{
    InputSimulator keyboard = new();

    // Define a dictionary with the key codes and their corresponding virtual key codes
    private static readonly Dictionary<char,VirtualKeyCode> keyCodes = new()
    {
        { 'ƒ', VirtualKeyCode.CONTROL },
        { '⟵', VirtualKeyCode.LEFT },
        { '⟶', VirtualKeyCode.RIGHT },
        { '⌃', VirtualKeyCode.UP },
        { '⌄', VirtualKeyCode.DOWN },
        { '⟳', VirtualKeyCode.DELETE },
        { '⟲', VirtualKeyCode.BACK },
        { '⟿', VirtualKeyCode.SHIFT },
        { '«', VirtualKeyCode.HOME},
        { '»', VirtualKeyCode.END},
        { '⊳', VirtualKeyCode.F5}
    };

    public void TypeString(string text)
    {
        bool needAdd = false, fastPress = false, cancelLast = false;
        List<VirtualKeyCode> keyGroup = new List<VirtualKeyCode>();
        foreach (char c in text)
        {
            Log.Information("current: {c}",c);
            switch (c)
            {
                case '⟰':
                    fastPress = true;
                    continue;
                case '⟱':
                    needAdd = true;
                    continue;
                case '✕':
                    string command = CommandStack.RemoveCommand();
                    keyboard.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_Z);
                    continue;
            }

            if (keyCodes.TryGetValue(c, out var code))
            {
                if (fastPress){
                    keyboard.Keyboard.ModifiedKeyStroke(keyGroup,code);
                }
                else if (needAdd)
                {
                    keyGroup.Add(code);
                }
                else
                {
                    keyboard.Keyboard.KeyPress(code);
                }
            }
            else
                keyboard.Keyboard.TextEntry(c);
        }
    }
}