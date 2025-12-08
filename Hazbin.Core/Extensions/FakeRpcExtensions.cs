using System.Reflection;
using Mirror;

namespace Hazbin.Core.Extensions;

public static class FakeRpcExtensions {
    public static void SendFakeRPC(this Player player, NetworkBehaviour networkBehaviour, int functionHash, params object[] objects) {
        using NetworkWriterPooled networkWriterPooled = NetworkWriterPool.Get();
        foreach (object obj in objects) {
            if (!MirrorWriterExtensions.Write(obj.GetType(), obj, networkWriterPooled))
            {
                Logger.Error($"Not found NetworkWriter for type {obj.GetType()}");
                return;
            }
        }
        player.Connection.Send(new RpcMessage()
        {
            netId = networkBehaviour.netId,
            componentIndex = networkBehaviour.ComponentIndex,
            functionHash = (ushort)functionHash,
            payload = networkWriterPooled.ToArraySegment()
        });
    }

    public static void SendFakeRPC(this Player player, NetworkBehaviour networkBehaviour, string functionName, params object[] objects) {
        Type type = networkBehaviour.GetType();
        MethodInfo? method = type.GetMethod(functionName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
        string longName = GetLongFuncName(type, method!);
        int funcHash = longName.GetStableHashCode();
        player.SendFakeRPC(networkBehaviour, funcHash, objects);
    }

    public static string GetLongFuncName(Type type, MethodInfo method) {
        return $"{method.ReturnType.FullName} {type.FullName}::{method.Name}({string.Join(",", method.GetParameters().Select(x => x.ParameterType.FullName))})";
    }
}