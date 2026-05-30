using Systems.Shared.Channels;
using UnityEngine;

namespace Scripts.DungeonGenerator
{
    public sealed record BossData(string Name, IHealthMonitor Health);

    [ChannelEvents(typeof(FloorChannel))]
    public static class FloorEvents
    {
        public sealed record RoomEntered(Room Room);
        public sealed record RoomExited(Room Room);

        public sealed record BossRoomEntered(BossModule Room);
        public sealed record BossRoomExited(BossModule Room);

        public sealed record BossStarted(BossData Boss);
        public sealed record BossKilled(BossData Boss);
    }

    [CreateAssetMenu(fileName = nameof(FloorChannel), menuName = MenuName + nameof(FloorChannel))]
    public sealed class FloorChannel : EventChannelBase<FloorChannel>
    {
        public void RaiseRoomEntered(Room room) => Raise(new FloorEvents.RoomEntered(room));
        public void RaiseRoomExited(Room room) => Raise(new FloorEvents.RoomExited(room));

        public void RaiseBossRoomEntered(BossModule room) => Raise(new FloorEvents.BossRoomEntered(room));
        public void RaiseBossRoomExited(BossModule room) => Raise(new FloorEvents.BossRoomExited(room));

        public void RaiseBossStarted(BossData boss) => Raise(new FloorEvents.BossStarted(boss));
        public void RaiseBossKilled(BossData boss) => Raise(new FloorEvents.BossKilled(boss));

    }
}
