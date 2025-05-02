// See https://aka.ms/new-console-template for more information

using System.Runtime.InteropServices;

class WndMessageProcessor
{
    public delegate IntPtr LowLevelProc(int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool GetMessage(out Message lpMsg, IntPtr hwnd, uint wMsgFilterMin, uint wMsgFilterMax);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool DispatchMessage(ref Message lpMsg);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool TranslateMessage(ref Message lpMsg);



    private static IntPtr _hookID = IntPtr.Zero;

    private static Action<IntPtr, IntPtr> _callback;

    public WndMessageProcessor(int hook, nint currentProcessModuleHandle, Action<IntPtr, IntPtr> callback)
    {
        _callback = callback;
        _hookID = SetHook(hook, HookCallback, currentProcessModuleHandle);
        Message msg;
        var received = GetMessage(out msg, IntPtr.Zero, 0, 0);
        while (received)
        {
            TranslateMessage(ref msg);
            DispatchMessage(ref msg);
        }
        UnhookWindowsHookEx(_hookID);
    }
   
    private static IntPtr SetHook(int hook, LowLevelProc proc, nint moduleHandle)
    {
        return SetWindowsHookEx(hook, proc, moduleHandle, 0);
    }

    private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0)
        {
            _callback(wParam, lParam);
        }
        return CallNextHookEx(_hookID, nCode, wParam, lParam);
    }
}

