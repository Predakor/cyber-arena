using Assets.Scripts.Utils;
using Scripts.DungeonGenerator;
using Systems.Shared;
using UnityEngine;

namespace _Scripts.Enemy
{
    public sealed class BossUiManager : Singleton<BossUiManager>
    {
        [SerializeField] private VitalBars _bossBar;
        [SerializeField] private FloorChannel _channel;

        //TODO  [SerializeField] private TextName _bossName

        protected override void Awake()
        {
            base.Awake();
            gameObject
                .EnsureComponent(out _bossBar);
        }

        private void OnEnable()
        {
            _channel.Subscribe<FloorEvents.BossStarted>(OnBossStarted, destroyCancellationToken);
            _channel.Subscribe<FloorEvents.BossKilled>(OnBossKilled, destroyCancellationToken);
        }

        private void OnBossStarted(FloorEvents.BossStarted evt)
        {
            _bossBar.SetHealthTarget(evt.Boss.Health);
            _bossBar.ShowVitals();
        }

        private void OnBossKilled(FloorEvents.BossKilled data)
        {
            _bossBar.ShowVitals(false);
        }

    }
}
