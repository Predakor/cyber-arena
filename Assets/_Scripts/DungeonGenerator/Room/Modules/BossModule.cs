using Scripts.DungeonGenerator;
using Scripts.Player;
using UnityEngine;

public sealed class BossModule : RoomModule<BossModule>
{
    [SerializeField] private Enemy _boss;
    [SerializeField] private FloorChannel _channel;

    public void Init(Enemy enemy, FloorChannel channel)
    {
        _boss = enemy;
        _channel = channel;
    }

    public override void HandlePlayerNearby(Player player)
    {
        logger.Info("boss Room nearby");
        if (IsPreloaded)
        {
            return;
        }

        PreloadBoss();
        IsPreloaded = true;

    }

    public override void HandlePlayerEnter(Player player)
    {
        _channel.RaiseRoomEntered(_room);

        _boss.AI.SetTarget(player.gameObject);
        _boss.AI.Trigger();
        _boss.ActivateEnemy();

        _channel.RaiseBossStarted(new(_boss.name, _boss.Health));

    }

    public override void HandlePlayerFaraway(Player player)
    {
        base.HandlePlayerFaraway(player);
        IsPreloaded = false;
        UnloadBoss();
    }

    private void PreloadBoss()
    {
        _boss = Instantiate(_boss, _room.transform);
        _boss.Freeze();
    }

    private void UnloadBoss()
    {
        Destroy(_boss);
        _boss = null;
    }

}
