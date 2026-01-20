using System;
using CommandSystem;

namespace InventoryControl.ApiFeatures;

[CommandHandler(typeof(GameConsoleCommandHandler))]
public class BearmanLogsIc : ICommand
{
    public string Command => "bearmanlogsIC";

    public string[] Aliases { get; } = ["bmlogsIC"];

    public string Description => "Sends collected plugin logs to the log server and returns the log id.";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        var getLogHistory = LogManager.GetLogHistory();
        response = getLogHistory.logResult;
        return getLogHistory.success;
    }
}