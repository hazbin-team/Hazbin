using Hazbin.Core.Enums;
using Mirror;

namespace Hazbin.Core.Extensions;

public static class FakeSyncListExtensions {
    public class ListChanger<T> {
        public T Value = default!;
        public int Index;
        public ListOperation Operation;
    }

    public static void SendFakeSyncListAdd<T>(this Player target, NetworkBehaviour networkBehaviour, ulong listIndex, T value)
        => target.SendFakeSyncList<T>(networkBehaviour, listIndex, new() {
            Operation = ListOperation.Add,
            Value = value
        });

    public static void SendFakeSyncListClear<T>(this Player target, NetworkBehaviour networkBehaviour, ulong listIndex)
        => target.SendFakeSyncList<T>(networkBehaviour, listIndex, new() {
            Operation = ListOperation.Clear,
        });

    public static void SendFakeSyncListRemoveAt<T>(this Player target, NetworkBehaviour networkBehaviour, ulong listIndex, int index)
        => target.SendFakeSyncList<T>(networkBehaviour, listIndex, new() {
            Operation = ListOperation.RemoveAt,
            Index = index
        });

    public static void SendFakeSyncListInsert<T>(this Player target, NetworkBehaviour networkBehaviour, ulong listIndex, int index, T value)
        => target.SendFakeSyncList<T>(networkBehaviour, listIndex, new() {
            Operation = ListOperation.Insert,
            Index = index,
            Value = value
        });
    
    public static void SendFakeSyncListSet<T>(this Player target, NetworkBehaviour networkBehaviour, ulong listIndex, int index, T value)
        => target.SendFakeSyncList<T>(networkBehaviour, listIndex, new() {
            Operation = ListOperation.Set,
            Index = index,
            Value = value
        });
    
    public static void SendFakeSyncList<T>(this Player target, NetworkBehaviour networkBehaviour, ulong listIndex, ListChanger<T> changer) {
        target.SendFakeCore(networkBehaviour,
        (writer) => {
            // Serialize Object Sync Data.
            writer.WriteULong(listIndex);

            // Copy from OnSerializeDelta
            writer.WriteUInt(1);
            writer.WriteByte((byte)changer.Operation);
            switch (changer.Operation)
            {
                case ListOperation.Add:
                    writer.Write(changer.Value);
                    break;
                case ListOperation.Insert:
                case ListOperation.Set:
                    writer.WriteUInt((uint)changer.Index);
                    writer.Write(changer.Value);
                    break;
                case ListOperation.RemoveAt:
                    writer.WriteUInt((uint)changer.Index);
                    break;
            }
        },
        (writer) => writer.WriteULong(0) // Write No SyncData
        );
    }
}