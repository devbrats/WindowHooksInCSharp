// See https://aka.ms/new-console-template for more information

public interface IWindowManager
{
    nint GetWindowFromPoint(POINT point);

    string GetProcessName(nint processHandle);

    string GetElementType(nint windowHandle);

    nint GetCurrentProcessModuleHandle();

    int GetLastError();

    POINT GetCurrentCursorPosition();

}

