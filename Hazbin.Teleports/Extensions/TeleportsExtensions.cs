using Hazbin.Core.Enums;
using Hazbin.Core.Extensions;
using LabApi.Features.Wrappers;
using MapGeneration;
using PlayerRoles;
using UnityEngine;
using Logger = LabApi.Features.Console.Logger;

namespace Hazbin.Teleports.Extensions;

public static class TeleportExtensions {
    private static HashSet<Room> _rooms;
    private static HashSet<Player> _players;

    static TeleportExtensions() {
        _rooms = new HashSet<Room>(200);
        _players = new HashSet<Player>(100);
    }

    public static IReadOnlyCollection<Room> Rooms => _rooms;

    public static IReadOnlyCollection<Player> Players => _players;

    public static void AllowTeleport(this Room room) => _rooms.Add(room);

    public static void AllowTeleport(this Player player) => _players.Add(player);

    public static void DenyTeleport(this Room room) => _rooms.Remove(room);

    public static void DenyTeleport(this Player player) => _players.Remove(player);
    
    public static void AllowAllRooms(params RoomName[] ignoredRooms) {
        foreach (Room? room in Room.List) {
            if (room is null || ignoredRooms.Any(x => x == room.Name)) continue;
            
            _rooms.Add(room);
        }
    }
    
    public static void AllowAllRooms() {
        foreach (Room? room in Room.List) {
            if (room is null) continue;
            
            _rooms.Add(room);
        }
    }

    public static void AllowAllPlayers(params RoleTypeId[] ignoredRoles) {
        foreach (Player? player in Player.List) {
            if (player == null || player.IsHost || player.IsNpc || ignoredRoles.Contains(player.Role)) continue;

            _players.Add(player);
        }
    }
    
    public static void DenyAllRooms(params RoomName[] ignoredRooms) {
        foreach (Room? room in Room.List) {
            if (room is null || !_rooms.Contains(room) || ignoredRooms.Any(x => x == room.Name)) {
                continue;
            }

            _rooms.Remove(room);
        }
    }

    public static void DenyAllPlayers(params RoleTypeId[] ignoredRoles) {
        foreach (Player? player in Player.List) {
            if (player is null || !_players.Contains(player) || player.IsHost || player.IsNpc || ignoredRoles.Contains(player.Role)) continue;

            _players.Remove(player);
        }
    }

    public static void DenyAllRooms() => _rooms.Clear();
    public static void DenyAllPlayers() => _players.Clear();

    public static void TeleportToRandomPlayer(this Player player) {
        if (Player.List.Count(ply => ply != null && !ply.IsHost && !ply.IsNpc) == 1 || _players.Count(ply => ply.UserId != player.UserId) < 1) {
            Logger.Debug("Players not found");

            return;
        }

        Player target = _players.RandomItem();

        if (target is null || target.IsHost || target.IsNpc) {
            Logger.Debug("Player is null");

            return;
        }

        while (target.UserId == player.UserId) {
            target = _players.RandomItem();
        }
        
        Logger.Debug($"Teleporting {player} to {target}");

        player.Position = target.Position + Vector3.up;
    }

    public static void TeleportToRandomRoom(this Player player, bool ignorePocketDimension = false) {
        Room room = Rooms.Where(x => x != null && (!ignorePocketDimension || x.Name != RoomName.Pocket)).RandomItem();
        
        Logger.Debug($"Teleporting {player} to {room}");
        
        if (room.Name == RoomName.Pocket && !ignorePocketDimension) player.EnableEffect(EffectType.Corroding);

        player.Position = room.Doors.RandomItem().Position + Vector3.up + Vector3.forward;
    }
}