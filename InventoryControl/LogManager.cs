using System;
using LabApi.Features.Console;

namespace InventoryControl;

internal abstract class LogManager
{
    public static bool DebugEnabled => InventoryControl.Instance.Config.Debug;

    public static void Debug(string message)
    {
        if (!DebugEnabled)
            return;

        Logger.Raw($"[DEBUG] [{InventoryControl.Instance.Name}] {message}", ConsoleColor.Green);
    }

    public static void Info(string message, ConsoleColor color = ConsoleColor.Cyan)
    {
        Logger.Raw($"[INFO] [{InventoryControl.Instance.Name}] {message}", color);
    }

    public static void Warn(string message)
    {
        Logger.Warn(message);
    }

    public static void Error(string message)
    {
        Logger.Raw($"[ERROR] [{InventoryControl.Instance.Name}] Details:\nVersion: {InventoryControl.Instance.Version}\n{message}", ConsoleColor.Red);
    }
}