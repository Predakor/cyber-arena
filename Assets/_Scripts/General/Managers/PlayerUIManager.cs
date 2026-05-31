using Assets.Scripts.Utils;
using Scripts.Player;
using Systems.Shared;
using UnityEngine;

namespace _Scripts.General.Managers
{
    public sealed class PlayerUIManager : Singleton<PlayerUIManager>
    {
        [SerializeField] private VitalBars _vitalBars;
        [SerializeField] private AmmoTracker _ammoTracker;
        [SerializeField] private Player _player;

        protected override void Awake()
        {
            base.Awake();
            gameObject
                .EnsureComponent(out _vitalBars)
                .EnsureComponent(out _ammoTracker);
        }

        //TODO change to init and explicit execution in gamestartup/bootstraped
        private void Start()
        {
            if (_player == null)
            {
                _player = FindObjectOfType<Player>();
            }

            if (_player.TryGetComponent(out Health health))
            {
                _vitalBars.SetHealthTarget(health);
            }
        }
    }
}
