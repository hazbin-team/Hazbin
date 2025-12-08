using System.Collections.ObjectModel;
using System.Reflection;
using System.Reflection.Emit;
using Hazbin.Core.Enums;
using Interactables.Interobjects.DoorUtils;
using Mirror;
using PlayerRoles;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.PlayableScps.Scp049.Zombies;
using PlayerRoles.PlayableScps.Scp1507;
using RelativePositioning;
using Respawning;
using UnityEngine;

namespace Hazbin.Core.Extensions;

public static class NetworkExtensions {
    private static Dictionary<string, string> _rpcFullNames = new();
    private static Dictionary<Type, MethodInfo> _writerExtensions = new();
    private static ReadOnlyDictionary<string, string> _readOnlyRpcFullNames = new(_rpcFullNames);
    private static ReadOnlyDictionary<Type, MethodInfo> _readOnlyWriterExtensions = new(_writerExtensions);

    private static int GetComponentIndex(NetworkIdentity identity, Type type) =>
        Array.FindIndex(identity.NetworkBehaviours, x => x.GetType() == type);

    public static ReadOnlyDictionary<string, string> RpcFullNames {
        get {
            if (_rpcFullNames.Count != 0)
                return _readOnlyRpcFullNames;

            Assembly assembly = typeof(ServerConsole).Assembly;
            IEnumerable<MethodInfo> methods = assembly.GetTypes()
                .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                .Where(m => m.GetCustomAttributes(typeof(ClientRpcAttribute), false).Length > 0 ||
                            m.GetCustomAttributes(typeof(TargetRpcAttribute), false).Length > 0);

            foreach (MethodInfo method in methods) {
                if (method.GetMethodBody() is not { } body)
                    continue;

                byte[] ilCode = body.GetILAsByteArray();
                int index = Array.IndexOf(ilCode, (byte)OpCodes.Ldstr.Value);
                if (index < 0 || method.ReflectedType is null)
                    continue;

                int token = BitConverter.ToInt32(ilCode, index + 1);
                string fullName = $"{method.ReflectedType.Name}.{method.Name}";
                if (!_rpcFullNames.ContainsKey(fullName))
                    _rpcFullNames.Add(fullName, method.Module.ResolveString(token));
            }

            return _readOnlyRpcFullNames;
        }
    }

    public static bool HasGeneratorPermission(this Player player, Generator generator, DoorPermissionCheck checkFlags = DoorPermissionCheck.Default)
        => player.HasDoorPermission(generator.Base, checkFlags);
    
    public static bool HasLockerChamberPermission(this Player player, LockerChamber chamber, DoorPermissionCheck checkFlags = DoorPermissionCheck.Default)
        => player.HasDoorPermission(chamber.Base, checkFlags);
    
    public static bool HasDoorPermission(this Player player, Door door, DoorPermissionCheck checkFlags = DoorPermissionCheck.Default)
        => player.HasDoorPermission(door.Base, checkFlags);
    
    public static bool HasDoorPermission(this Player player, IDoorPermissionRequester requester, DoorPermissionCheck checkFlags = DoorPermissionCheck.Default) {
        if (checkFlags.HasFlag(DoorPermissionCheck.Bypass) && player.IsBypassEnabled)
            return true;

        if (checkFlags.HasFlag(DoorPermissionCheck.Role) && player.RoleBase is IDoorPermissionProvider roleProvider && requester.PermissionsPolicy.CheckPermissions(roleProvider.GetPermissions(requester)))
            return true;

        foreach (Item item in player.Items) {
            bool isCurrent = item == player.CurrentItem;
            if (!checkFlags.HasFlag(DoorPermissionCheck.CurrentItem) && isCurrent)
                continue;

            if (!checkFlags.HasFlag(DoorPermissionCheck.InventoryExcludingCurrent) && !isCurrent)
                continue;

            if (item.Base is IDoorPermissionProvider itemProvider && requester.PermissionsPolicy.CheckPermissions(itemProvider.GetPermissions(requester)))
                return true;
        }

        return false;
    }
    
    public static void SendFakeCassieMessage(
        this Player target,
        string message,
        bool isHeld = false,
        bool isNoisy = true,
        bool isSubtitles = true,
        string customSubtitles = "") {
        foreach (RespawnEffectsController allController in RespawnEffectsController.AllControllers) {
            if (!allController)
                continue;

            target.SendFakeRPC(allController, nameof(RespawnEffectsController.RpcCassieAnnouncement), message, isHeld, isNoisy, isSubtitles, customSubtitles);
        }
    }
    
    public static ReadOnlyDictionary<Type, MethodInfo> WriterExtensions {
        get {
            if (_writerExtensions.Count != 0)
                return _readOnlyWriterExtensions;

            IEnumerable<MethodInfo> writerMethods = typeof(NetworkWriterExtensions).GetMethods()
                .Where(m => !m.IsGenericMethod &&
                            m.GetCustomAttribute(typeof(ObsoleteAttribute)) == null &&
                            m.GetParameters().Length == 2);

            foreach (MethodInfo method in writerMethods) {
                ParameterInfo param = method.GetParameters().First(p => p.ParameterType != typeof(NetworkWriter));
                if (!_writerExtensions.ContainsKey(param.ParameterType))
                    _writerExtensions.Add(param.ParameterType, method);
            }

            Type? generatedType = Assembly.GetAssembly(typeof(RoleTypeId))
                .GetType("Mirror.GeneratedNetworkCode");

            if (generatedType != null) {
                IEnumerable<MethodInfo> genMethods = generatedType.GetMethods()
                    .Where(m => !m.IsGenericMethod &&
                                m.GetParameters().Length == 2 &&
                                m.ReturnType == typeof(void));

                foreach (MethodInfo method in genMethods) {
                    ParameterInfo param = method.GetParameters().First(p => p.ParameterType != typeof(NetworkWriter));
                    if (!_writerExtensions.ContainsKey(param.ParameterType))
                        _writerExtensions.Add(param.ParameterType, method);
                }
            }

            IEnumerable<Type> serializerTypes = typeof(ServerConsole).Assembly.GetTypes()
                .Where(t => t.Name.EndsWith("Serializer"));

            foreach (Type serializer in serializerTypes) {
                IEnumerable<MethodInfo> writeMethods = serializer.GetMethods()
                    .Where(m => m.ReturnType == typeof(void) && m.Name.StartsWith("Write"));

                foreach (MethodInfo method in writeMethods) {
                    ParameterInfo param = method.GetParameters().First(p => p.ParameterType != typeof(NetworkWriter));
                    if (!_writerExtensions.ContainsKey(param.ParameterType))
                        _writerExtensions.Add(param.ParameterType, method);
                }
            }

            return _readOnlyWriterExtensions;
        }
    }

    public static void PlayBeepSound(this Player player) =>
        SendFakeTargetRpc(player, ReferenceHub._hostHub.networkIdentity, typeof(AmbientSoundPlayer),
            nameof(AmbientSoundPlayer.RpcPlaySound), 7);

    public static void ChangeAppearance(this Player player, RoleTypeId type, bool skipJump = false, byte unitId = 0) =>
        ChangeAppearance(player, type, Player.List.Where(x => x != player), skipJump, unitId);

    public static void ChangeAppearance(this Player player, RoleTypeId type, IEnumerable<Player> playersToAffect, bool skipJump = false, byte unitId = 0) {
        if (!PlayerRoleLoader.TryGetRoleTemplate(type, out PlayerRoleBase roleBase))
            return;

        bool isRisky = type.GetTeam() is Team.Dead || !player.IsAlive;
        NetworkWriterPooled writer = NetworkWriterPool.Get();

        writer.WriteUShort(38952);
        writer.WriteUInt(player.NetworkId);
        writer.WriteRoleType(type);

        switch (roleBase) {
            case HumanRole { UsesUnitNames: true }: {
                if (player.RoleBase is not HumanRole)
                    isRisky = true;
                writer.WriteByte(unitId);
                break;
            }
            case ZombieRole: {
                if (player.RoleBase is not ZombieRole)
                    isRisky = true;
                writer.WriteUShort((ushort)Mathf.Clamp(Mathf.CeilToInt(player.MaxHealth), ushort.MinValue, ushort.MaxValue));
                writer.WriteBool(true);
                break;
            }
        }

        if (roleBase is Scp1507Role) {
            if (player.RoleBase is not Scp1507Role)
                isRisky = true;
            writer.WriteByte((byte)player.RoleBase._spawnReason);
        }

        if (roleBase is FpcStandardRoleBase fpc) {
            if (player.RoleBase is not FpcStandardRoleBase playerFpc)
                isRisky = true;
            else
                fpc = playerFpc;

            fpc.FpcModule.MouseLook.GetSyncValues(0, out ushort value, out ushort _);
            writer.WriteRelativePosition(new RelativePosition(player.Position));
            writer.WriteUShort(value);
        }

        foreach (Player target in playersToAffect) {
            if (target != player || !isRisky)
                target.Connection.Send(writer.ToArraySegment());
            else
                Logger.Error($"Prevent Seld-Desync of {player.Nickname} with {type}");
        }

        NetworkWriterPool.Return(writer);
        if (!skipJump)
            player.Position += Vector3.up * 0.25f;
    }

    public static void PlayCassieAnnouncement(this Player player, string words, bool makeHold = false, bool makeNoise = true, bool isSubtitles = false) {
        foreach (RespawnEffectsController controller in RespawnEffectsController.AllControllers) {
            if (controller != null) {
                SendFakeTargetRpc(player, controller.netIdentity, typeof(RespawnEffectsController),
                    nameof(RespawnEffectsController.RpcCassieAnnouncement), words, makeHold, makeNoise, isSubtitles);
            }
        }
    }

    public static void SendFakeTargetRpc(Player target, NetworkIdentity behaviorOwner, Type targetType, string rpcName, params object[] values) {
        NetworkWriterPooled writer = NetworkWriterPool.Get();

        foreach (object value in values) {
            Type valueType = value.GetType();
            if (_writerExtensions.TryGetValue(valueType, out MethodInfo method)) {
                method.Invoke(null, [writer, value]);
            }
        }

        RpcMessage msg = new() {
            netId = behaviorOwner.netId,
            componentIndex = (byte)GetComponentIndex(behaviorOwner, targetType),
            functionHash = (ushort)RpcFullNames[$"{targetType.Name}.{rpcName}"].GetStableHashCode(),
            payload = writer.ToArraySegment(),
        };

        target.Connection.Send(msg);
        NetworkWriterPool.Return(writer);
    }
}