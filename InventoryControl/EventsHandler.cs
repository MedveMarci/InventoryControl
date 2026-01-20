using System;
using System.Collections.Generic;
using System.Linq;
using InventoryControl.ApiFeatures;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.Attachments;
using InventorySystem.Items.Firearms.Modules;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Features.Wrappers;
using MEC;
using PlayerRoles;
using Random = UnityEngine.Random;

namespace InventoryControl;

public static class EventsHandler
{
    public static void OnServerRoundStarted()
    {
        ApiManager.CheckForUpdates();
    }
    
    public static void OnPlayerChangedRole(PlayerChangedRoleEventArgs ev)
    {
        try
        {
            LogManager.Debug(
                $"PlayerChangedRole Event triggered for Player: {ev.Player?.UserId}, NewRole: {ev.NewRole?.RoleTypeId}, SpawnFlags: {ev.SpawnFlags}");
            if (!Round.IsRoundStarted || ev.Player == null || ev.SpawnFlags == RoleSpawnFlags.None ||
                ev.SpawnFlags == RoleSpawnFlags.UseSpawnpoint) return;

            Timing.CallDelayed(0.01f, () =>
            {
                LogManager.Debug(
                    $"PlayerChangedRole Delayed Call triggered for Player: {ev.Player?.UserId}, NewRole: {ev.NewRole?.RoleTypeId}, SpawnFlags: {ev.SpawnFlags}");
                if (!ev.Player.IsDummy && InventoryControl.Singleton.Config.InventoryRank?.Count > 0 &&
                    InventoryControl.Singleton.Config.InventoryRank.ContainsKey(GetPlayerGroupName(ev.Player)))
                    if (InventoryControl.Singleton.Config.InventoryRank[GetPlayerGroupName(ev.Player)]
                            ?.Count(x => x.Value.RoleTypeId == ev.NewRole.RoleTypeId) > 0)
                    {
                        LogManager.Debug(
                            $"Setting inventory for Player: {ev.Player?.UserId} based on their rank: {GetPlayerGroupName(ev.Player)}");
                        SetPlayerInventory(ev.Player,
                            InventoryControl.Singleton.Config.InventoryRank[GetPlayerGroupName(ev.Player)]
                                .Where(x => x.Value.RoleTypeId == ev.NewRole.RoleTypeId).ToList().RandomItem().Value);
                        return;
                    }

                if (InventoryControl.Singleton.Config.Inventory?.Count(x =>
                        x.Value.RoleTypeId == ev.NewRole.RoleTypeId) > 0)
                {
                    LogManager.Debug(
                        $"Setting inventory for Player: {ev.Player?.UserId} based on their role: {ev.NewRole?.RoleTypeId ?? RoleTypeId.None}");
                    SetPlayerInventory(ev.Player,
                        InventoryControl.Singleton.Config.Inventory
                            .Where(x => x.Value.RoleTypeId == ev.NewRole.RoleTypeId).ToList().RandomItem().Value);
                }
            });
        }
        catch (Exception e)
        {
            LogManager.Error("[Event: OnPlayerChangedRole] " + e);
        }
    }

    private static void SetPlayerInventory(Player player, RoleInventory roleInventory)
    {
        try
        {
            LogManager.Debug(
                $"Setting inventory for Player: {player?.UserId}, RoleTypeId: {roleInventory?.RoleTypeId}, KeepItems: {roleInventory?.KeepItems}, KeepAmmos: {roleInventory?.KeepAmmos}");
            var Ammos = new Dictionary<ItemType, ushort>(player.Ammo);
            if (!roleInventory.KeepItems && !player.IsWithoutItems) player.ClearInventory();
            else player.ClearInventory(true, false);

            Timing.CallDelayed(0.01f, () =>
            {
                LogManager.Debug(
                    $"SetPlayerInventory Delayed Call triggered for Player: {player?.UserId}, RoleTypeId: {roleInventory?.RoleTypeId}");
                if (roleInventory.Items?.Count > 0)
                    foreach (var Item in roleInventory.Items)
                        if (Item.Value >= Random.Range(0, 101))
                        {
                            var itemBase = player.AddItem(Item.Key).Base;
                            LogManager.Debug(
                                $"Adding item for Player: {player?.UserId}, ItemTypeId: {itemBase.ItemTypeId}");
                            if (itemBase is Firearm firearm)
                            {
                                if (AttachmentsServerHandler.PlayerPreferences.TryGetValue(player.ReferenceHub,
                                        out var value) && value.TryGetValue(itemBase.ItemTypeId, out var value2))
                                {
                                    LogManager.Debug(
                                        $"Applying saved attachments for Player: {player?.UserId}, ItemTypeId: {itemBase.ItemTypeId}");
                                    firearm.ApplyAttachmentsCode(value2, true);
                                }

                                if (firearm.Modules.First(x => x is MagazineModule) is MagazineModule magazineModule)
                                {
                                    LogManager.Debug(
                                        $"Refilling magazine for Player: {player?.UserId}, ItemTypeId: {itemBase.ItemTypeId}");
                                    magazineModule.ServerInsertEmptyMagazine();
                                    magazineModule.AmmoStored = magazineModule.AmmoMax;
                                    magazineModule.ServerResyncData();
                                }
                            }
                        }

                if (roleInventory.KeepAmmos && Ammos?.Count > 0)
                {
                    LogManager.Debug(
                        $"Restoring ammos for Player: {player?.UserId}, AmmosCount: {Ammos?.Count}");
                    foreach (var ammo in Ammos)
                        player.SetAmmo(ammo.Key, ammo.Value);
                }

                if (!(roleInventory.Ammos?.Count > 0)) return;
                LogManager.Debug(
                    $"Adding ammos for Player: {player?.UserId}, AmmosToAddCount: {roleInventory.Ammos?.Count}");
                foreach (var Ammo in roleInventory.Ammos)
                    if (IsAmmo(Ammo.Key))
                        player.AddAmmo(Ammo.Key, (ushort)Ammo.Value);
            });
        }
        catch (Exception e)
        {
            LogManager.Error("[Event: SetPlayerInventory] " + e);
        }
    }

    private static string GetPlayerGroupName(Player player)
    {
        try
        {
            if (player.UserId == null || player.IsDummy) return string.Empty;

            if (ServerStatic.PermissionsHandler.Members.TryGetValue(player.UserId, out var name))
                return name;
            return player.UserGroup != null
                ? ServerStatic.PermissionsHandler.Groups.First(g => EqualsTo(g.Value, player.UserGroup)).Key
                : string.Empty;
        }
        catch (Exception e)
        {
            LogManager.Error("[InventoryControl] [Event: GetPlayerGroupName] " + e);
            return string.Empty;
        }
    }

    private static bool IsAmmo(ItemType type)
    {
        return type is ItemType.Ammo9x19 or ItemType.Ammo762x39 or ItemType.Ammo556x45 or ItemType.Ammo44cal or ItemType.Ammo12gauge;
    }

    private static bool EqualsTo(UserGroup check, UserGroup player)
    {
        return check.BadgeColor == player.BadgeColor
               && check.BadgeText == player.BadgeText
               && check.Permissions == player.Permissions
               && check.Cover == player.Cover
               && check.HiddenByDefault == player.HiddenByDefault
               && check.Shared == player.Shared
               && check.KickPower == player.KickPower
               && check.RequiredKickPower == player.RequiredKickPower;
    }
}