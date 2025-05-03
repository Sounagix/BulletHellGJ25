
using UnityEngine;

/// <summary>
/// Debug colors for the log messages
/// </summary>
public enum CustomLogColor
{
    Red = 0xFF0000,
    Green = 0x00FF00,
    Blue = 0x0000FF,
    Black = 0x000000,
    White = 0xFFFFFF,
    Yellow = 0xFFFF00,
    UnityDefault = 0xB8B8B8,
}

/// <summary>
/// Log messages with different colors
/// </summary>
public static class CustomLog
{
    /// <summary>
    /// Enable or disable the debug log
    /// </summary>
    public static bool EnableDebugLog = true;

    /// <summary>
    /// Given the <see cref="CustomLogColor"/> enum, it returns the corresponding color in HTML format"/>
    /// </summary>
    /// <param name="color">The color to be selected</param>
    /// <returns></returns>
    private static string SelectLogColor(CustomLogColor color)
    {
        string selectedColor = "<color=#B8B8B8>";

        switch (color)
        {
            case CustomLogColor.Red:
                selectedColor = "<color=#FF0000>";
                break;
            case CustomLogColor.Green:
                selectedColor = "<color=#00FF00>";
                break;
            case CustomLogColor.Blue:
                selectedColor = "<color=#0000FF>";
                break;
            case CustomLogColor.Black:
                selectedColor = "<color=#000000>";
                break;
            case CustomLogColor.White:
                selectedColor = "<color=#FFFFFF>";
                break;
            case CustomLogColor.Yellow:
                selectedColor = "<color=#FFFF00>";
                break;
            default:
                break;
        }

        return selectedColor;
    }

    /// <summary>
    /// Logs a message with a specific color
    /// <para></para>
    /// It is possible to disable the debug log by setting <see cref="EnableDebugLog"/> to false
    /// <para></para>
    /// All the messages are prefixed with "Foundation Architecture - " by default
    /// </summary>
    /// <param name="message">The message to be displayed</param>
    /// <param name="color">The color of the message to be displayed</param>
    /// <param name="prefix">The string to be displayed at the start of the message</param>
    public static void Log(string message, CustomLogColor color = CustomLogColor.UnityDefault, string prefix = "Foundation Architecture - ")
    {
        if (!EnableDebugLog)
            return;

        string debugColor = SelectLogColor(color);
        Debug.Log($"{debugColor}{prefix}{message}</color>");
    }

    /// <summary>
    /// Log an error message. 
    /// <para></para>
    /// It is not possible to disable this log and it is always displayed with red color
    /// <para></para>
    /// All the messages are prefixed with "Foundation Architecture - " by default
    /// </summary>
    /// <param name="message">The message to be displayed</param>
    /// <param name="prefix">The string to be displayed at the start of the message</param>
    public static void LogError(string message, string prefix = "Foundation Architecture - ")
    {
        string debugColor = SelectLogColor(CustomLogColor.Red);
        Debug.Log($"{debugColor}{prefix}{message}</color>");
    }
}
