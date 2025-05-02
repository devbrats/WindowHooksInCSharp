// See https://aka.ms/new-console-template for more information

public interface IWindowManager
{
    /// <summary>
    /// Gets the handle of the process from the point which is captured.
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    nint GetWindowFromPoint(POINT point);

    /// <summary>
    /// Get the process name from window handle
    /// </summary>
    /// <param name="processHandle"></param>
    /// <returns></returns>
    string GetProcessName(nint processHandle);

    /// <summary>
    /// Get the element type from the window handle
    /// </summary>
    /// <param name="windowHandle"></param>
    /// <returns></returns>
    string GetElementType(nint windowHandle);

    /// <summary>
    /// Get the current process handle
    /// </summary>
    /// <returns></returns>
    nint GetCurrentProcessModuleHandle();

    int GetLastError();

    /// <summary>
    /// Get the current mouse cursor positions
    /// </summary>
    /// <returns></returns>
    POINT GetCurrentCursorPosition();

}

