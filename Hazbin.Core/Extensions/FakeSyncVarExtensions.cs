using AdminToys;
using Mirror;

namespace Hazbin.Core.Extensions;

public static class FakeSyncVarExtensions {
    private static readonly Dictionary<Type, ulong> SubWriteClassToMinULong = new()
    {
        [typeof(AdminToyBase)] = 32,
    };

    private static ulong GetSubclassMinDirtyBit(Type type)
    {
        foreach (KeyValuePair<Type, ulong> kvp in SubWriteClassToMinULong)
        {
            if (type.IsSubclassOf(kvp.Key))
                return kvp.Value;
        }

        return ulong.MaxValue;
    }

    public static void SendFakeSyncVar<T>(this Player target, NetworkBehaviour networkBehaviour, ulong dirtyBit, T value)
    {
        Type networkType = networkBehaviour.GetType();

        target.SendFakeCore(networkBehaviour,
        (writer) => NetworkWriterExtensions.WriteULong(writer, 0), // Write No SyncData
        (writer) => // Write SyncVar
        {
            // Write DrityBit always
            NetworkWriterExtensions.WriteULong(writer, dirtyBit);

            ulong minDirtyBit = GetSubclassMinDirtyBit(networkType);
            bool isWritten = false;

            if (dirtyBit >= minDirtyBit)
            {
                NetworkWriterExtensions.WriteULong(writer, dirtyBit);
                isWritten = true;
            }

            writer.Write(value);

            if (!isWritten)
                NetworkWriterExtensions.WriteULong(writer, dirtyBit);

        });
    }

    // Sending mulitple Sync Vars to the player. (Not Tested)
    public static void SendFakeSyncVars(this Player target, NetworkBehaviour networkBehaviour,
        params (ulong DirtyBit, object SyncVar)[] syncVars) {
        if (syncVars.Length == 0)
            return;

        Type networkType = networkBehaviour.GetType();

        target.SendFakeCore(networkBehaviour,
            (writer) => NetworkWriterExtensions.WriteULong(writer, 0), // Write No SyncData
            (writer) => // Write SyncVar
            {
                ulong allDirtyBits = syncVars.Aggregate(0UL, (previous, tuple) => previous | tuple.DirtyBit);

                // Write DrityBit always
                NetworkWriterExtensions.WriteULong(writer, allDirtyBits);

                ulong minDirtyBit = GetSubclassMinDirtyBit(networkType);
                bool isWritten = false;

                foreach ((ulong dirtyBit, object syncVar) in syncVars.OrderBy(x => x.DirtyBit)) {
                    if (dirtyBit >= minDirtyBit && !isWritten) {
                        NetworkWriterExtensions.WriteULong(writer, allDirtyBits);
                        isWritten = true;
                    }

                    if (!MirrorWriterExtensions.Write(syncVar.GetType(), syncVar, writer)) {
                        Logger.Error($"No NetworkWriter found for type {syncVar.GetType()}");
                        return;
                    }
                }

                if (!isWritten)
                    writer.WriteULong(allDirtyBits);
            });
    }
}