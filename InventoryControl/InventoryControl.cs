using System;
using LabApi.Events.Handlers;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;

namespace InventoryControl;

public class InventoryControl : Plugin<Config>
{
    public static InventoryControl Singleton { get; private set; }

    public override string Name => "InventoryControl";

    public override string Description => "A plugin that will allow you to control the inventory of various roles.";

    public override string Author => "MrAfitol & MedveMarci";

    public override Version Version { get; } = new(1, 0, 1);

    public override Version RequiredApiVersion { get; } = new(LabApiProperties.CompiledVersion);
    
    public string githubRepo = "MedveMarci/InventoryControl";
    
    public override void Enable()
    {
        Singleton = this;
        PlayerEvents.ChangedRole += EventsHandler.OnPlayerChangedRole;
        ServerEvents.RoundStarted += EventsHandler.OnServerRoundStarted;
    }

    public override void Disable()
    {
        PlayerEvents.ChangedRole -= EventsHandler.OnPlayerChangedRole;
        ServerEvents.RoundStarted -= EventsHandler.OnServerRoundStarted;
        Singleton = null;
    }
}