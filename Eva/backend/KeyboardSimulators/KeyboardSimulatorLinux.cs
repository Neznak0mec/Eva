using System.Runtime.InteropServices;

namespace Eva.KeyboardSimulators;


public static class KeyboardSimulatorLinux
{
    [DllImport("libX11.so.6")]
    static extern IntPtr XOpenDisplay(string display);

    [DllImport("libX11.so.6")]
    static extern int XCloseDisplay(IntPtr display);

    [DllImport("libXtst.so.6")]
    static extern void XTestFakeKeyEvent(IntPtr display, uint keycode, bool is_press, ulong delay);

    [DllImport("libX11.so.6")]
    static extern uint XKeysymToKeycode(IntPtr display, uint keysym);

    public static void SimulateKeyPresses(string input)
    {
        IntPtr display = XOpenDisplay(null);
        if (display == IntPtr.Zero)
        {
            Console.WriteLine("Error: Unable to open X display.");
            return;
        }

        foreach (var c in input)
        {
            uint keycode = XKeysymToKeycode(display, (uint)c);
            if (keycode == 0) continue;
            XTestFakeKeyEvent(display, keycode, true, 0);  // Key press
            XTestFakeKeyEvent(display, keycode, false, 0); // Key release
        }

        XCloseDisplay(display);
    }
}