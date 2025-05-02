// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

public class Win32WindowsManager: IWindowManager
{
    const uint PROCESS_QUERY_INFORMATION = 0x0400;
    const uint PROCESS_VM_READ = 0x0010;

    [DllImport("user32.dll")]
    private static extern bool GetCursorPos(out POINT lpPoint);

    [DllImport("user32.dll")]
    private static extern IntPtr WindowFromPoint(POINT Point);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr OpenProcess(uint processAccess, bool bInheritHandle, uint processId);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CloseHandle(IntPtr hObject);

    [DllImport("psapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, StringBuilder lpBaseName, int nSize);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);


    public nint GetWindowFromPoint(POINT point)
    {
        IntPtr hWnd = WindowFromPoint(point);
        return hWnd;

    }

    public string GetProcessName(nint processHandle)
    {
        uint processId = 0;

        if (processHandle != IntPtr.Zero)
        {
            GetWindowThreadProcessId(processHandle, out processId);
            
        }
        var result = string.Empty;

        if (processId != 0)
        {
            IntPtr hProcess = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_VM_READ, false, processId);
            if (hProcess != IntPtr.Zero)
            {
                StringBuilder exeName = new StringBuilder(1024);
                var opResult = GetModuleFileNameEx(hProcess, IntPtr.Zero, exeName, exeName.Capacity);
                if (opResult > 0)
                {
                    result = exeName.ToString();
                }
                CloseHandle(hProcess);
            }
        }
       return result;
    }

    public string GetElementType(nint windhowHandle)
    {
        var bufferSize = 128;
        var result = 0;
        StringBuilder className = null;

        while (result == 0 && bufferSize <= int.MaxValue && bufferSize > 0)
        {
            className = new StringBuilder(bufferSize);
            result = GetClassName(windhowHandle, className, bufferSize);
            bufferSize *= 2;
        }

        return className.ToString();

    }

    public nint GetCurrentProcessModuleHandle()
    {
        var process = Process.GetCurrentProcess();
        var moduleHandle = GetModuleHandle(process.MainModule.ModuleName);

        return moduleHandle;
    }

    public int GetLastError()
    {
        return Marshal.GetLastWin32Error();
    }

    public POINT GetCurrentCursorPosition()
    {
        POINT point;
        GetCursorPos(out point);
        return point;
    }
}

