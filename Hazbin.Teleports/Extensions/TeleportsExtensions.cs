using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using PlayerRoles;
using UnityEngine;

namespace Hazbin.Teleports.Extensions;

public static class TeleportExtensions {
    public static Room FindRoom(this RoomType type) => Room.List.ToList().First(x => x.Type == type);

    private static HashSet<Room> _rooms;
    private static HashSet<Player> _players;

    static TeleportExtensions() {
        _rooms = new HashSet<Room>(100);
        _players = new HashSet<Player>(100);
    }

    public static IReadOnlyCollection<Room> Rooms => _rooms;

    public static IReadOnlyCollection<Player> Players => _players;

    public static void AllowTeleport(this Room room) => _rooms.Add(room);

    public static void AllowTeleport(this Player player) => _players.Add(player);

    public static void DenyTeleport(this Room room) => _rooms.Remove(room);

    public static void DenyTeleport(this Player player) => _players.Remove(player);

    public static void AllowAllRooms(params RoomType[] ignoredRooms) {
        foreach (Room? room in Room.List) {
            if (room is null || ignoredRooms.Contains(room.Type)) continue;

            _rooms.Add(room);
        }
    }

    public static void AllowAllPlayers(params RoleTypeId[] ignoredRoles) {
        foreach (Player? player in Player.List) {
            if (player == null || player.IsHost || player.IsNPC || ignoredRoles.Contains(player.Role.Type)) continue;

            _players.Add(player);
        }
    }

    public static void DenyAllRooms(params RoomType[] ignoredRooms) {
        foreach (Room? room in Room.List) {
            if (room is null || !_rooms.Contains(room) || ignoredRooms.Contains(room.Type)) {
                continue;
            }

            _rooms.Remove(room);
        }
    }

    public static void DenyAllPlayers(params RoleTypeId[] ignoredRoles) {
        foreach (Player? player in Player.List) {
            if (player is null || !_players.Contains(player) || player.IsHost || player.IsNPC || ignoredRoles.Contains(player.Role.Type)) continue;

            _players.Remove(player);
        }
    }

    public static void DenyAllRooms() => _rooms.Clear();
    public static void DenyAllPlayers() => _players.Clear();

    public static void TeleportToRandomPlayer(this Player player) {
        if (Player.List.Count(ply => ply != null && !ply.IsHost && !ply.IsNPC) == 1 || _players.Count(ply => ply.UserId != player.UserId) < 1) {
            Log.Debug("Players not found");

            return;
        }

        Player target = _players.GetRandomValue();

        if (target is null || target.IsHost || target.IsNPC) {
            Log.Debug("Player is null");

            return;
        }

        while (target.UserId == player.UserId) {
            target = _players.GetRandomValue();
        }

        Log.Debug("Teleportation");

        player.Position = target.Position + Vector3.up;
    }

    public static void TeleportToRandomRoom(this Player player, bool ignorePocketDimension = false) {
        Room room = Rooms.Where(x => x && (!ignorePocketDimension || x.Type != RoomType.Pocket)).GetRandomValue();
            
        if (room.Type == RoomType.Pocket && !ignorePocketDimension) player.EnableEffect(EffectType.Corroding);

        player.Position = room.GetSafePosition();
    }

    public static Vector3 GetSafePosition(this Room room) => room.Type.IsDangerous() ? room.Doors.GetRandomValue().Position + Vector3.up + Vector3.forward : room.Position + Vector3.up;

    public static bool IsDangerous(this RoomType room) => room switch {
        RoomType.EzShelter or RoomType.EzCollapsedTunnel or RoomType.HczCrossRoomWater or RoomType.HczTestRoom or RoomType.Lcz173 or RoomType.Lcz330 => true,
        _ => false
    };
}