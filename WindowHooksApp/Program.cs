// See https://aka.ms/new-console-template for more information
using System.Runtime.InteropServices;

public class Program
{
    private static IWindowManager _hook;
    // Constants for mouse messages
    private const int WH_MOUSE_LL = 14;
    private const int WM_LBUTTONDOWN = 0x0201;
    private const int WM_RBUTTONDOWN = 0x0204;

    static void Main(string[] args)
    {
        try
        {
            _hook = new Win32WindowsManager();
            var currentProcessHandle = _hook.GetCurrentProcessModuleHandle();
            WndMessageProcessor handler = new WndMessageProcessor(WH_MOUSE_LL, currentProcessHandle, MouseWndProcCallback);
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        
    }

    static void MouseWndProcCallback(IntPtr wParam, IntPtr lParam)
    {
        if (wParam == WM_LBUTTONDOWN || wParam == WM_RBUTTONDOWN)
        {
            MouseHookStruct hookData = Marshal.PtrToStructure<MouseHookStruct>(lParam);

            var point = _hook.GetCurrentCursorPosition();

            var windowHandle = _hook.GetWindowFromPoint(point);
            PrintErrorIfAny(windowHandle.ToString(), "WindowFromPoint");
            if (windowHandle != 0)
            {
                var className = _hook.GetElementType(windowHandle);
                PrintErrorIfAny(className, "ElementType");

                var processFullName = _hook.GetProcessName(windowHandle);
                PrintErrorIfAny(processFullName, "GetProcessName");
                if (!string.IsNullOrEmpty(processFullName))
                {
                    var exename = processFullName.Split('\\').LastOrDefault();
                    PrintErrorIfAny(processFullName, "ProcessName");
                    Console.WriteLine($"{exename}: {{X={point.X},Y={point.Y}}}: {className}");
                }
                
            }
        }
    }

    static void PrintErrorIfAny(string result, string title)
    {
        if(result == "0" || string.IsNullOrEmpty(result))
        {
            var error = _hook.GetLastError();
            Console.WriteLine($"Error while fetching {title}: {error}");
        }
    }
}


