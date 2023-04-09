using System;
using System.Runtime.InteropServices;


public class KeyboardSimulatorWindows
{
    public class Keyboard{

        // Import the keybd_event function
        [DllImport("user32.dll")]
        private static extern void keybd_event(byte virtualKey, byte scanCode, uint flags, UIntPtr extraInfo);

    // Define constants for the keydown and keyup events
    private const uint KEYEVENTF_EXTENDEDKEY = 0x0000;
    private const uint KEYEVENTF_KEYUP = 0x0002;

    // Define a dictionary with the key codes and their corresponding virtual key codes
    private static readonly Dictionary<char, byte> keyCodes = new Dictionary<char, byte>()
    {
        { 'ƒ', 0x11 },
        { '⟵', 0x25 },
        { '⟶', 0x27 },
        { '⌃', 0x26 },
        { '⌄', 0x28 },
        { '⟳', 0x2E },
        { '⟲', 0x08 },
        { '⟿', 0x10 }
    };

    public void TypeString(string text)
    {
        bool needPress = false, needRealise = false;
        foreach (char c in text)
        {
            switch (c)
            {
                case '⟰':
                    needRealise = true;
                    continue;
                case '⟱':
                    needPress = true;
                    continue;
            }

            if (keyCodes.TryGetValue(c, out byte keyCode))
            {
                if (needPress)
                {
                    keybd_event(keyCode, 0, KEYEVENTF_EXTENDEDKEY, UIntPtr.Zero);
                    needPress = false;
                }
                else if (needRealise)
                {
                    keybd_event(keyCode, 0, KEYEVENTF_KEYUP | KEYEVENTF_EXTENDEDKEY, UIntPtr.Zero);
                    needRealise = false;
                }
                else
                {
                    keybd_event(keyCode, 0, KEYEVENTF_EXTENDEDKEY, UIntPtr.Zero);
                    keybd_event(keyCode, 0, KEYEVENTF_KEYUP | KEYEVENTF_EXTENDEDKEY, UIntPtr.Zero);
                }
            }
        }
    }
}
}
