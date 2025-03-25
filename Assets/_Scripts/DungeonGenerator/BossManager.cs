using Helpers.Collections;
using System.Linq;
using UnityEngine;

public class BossManager : MonoBehaviour {
    [SerializeField] Room _room;
    [SerializeField] EnemyPoolData _enemyPoolData;
    [SerializeField] Enemy _boss;

    VitalBars _vitalBar;
    bool bossSpawned = false;

    public void Init(EnemyPoolData enemyPoolData) {
        _enemyPoolData = enemyPoolData;
    }

    void Awake() {
        _boss = CollectionUtils.RandomElement(_enemyPoolData.Bosses);
    }

    void OnEnable() {
        _room.OnRoomEnter += OnRoomEnter;
        _room.OnPlayerNearby += OnPlayerNearby;
    }

    void OnDisable() {
        _room.OnRoomEnter -= OnRoomEnter;
        _room.OnPlayerNearby -= OnPlayerNearby;
    }

    void OnPlayerNearby() {
        Debug.Log("boss Room nearby", this);
        if (bossSpawned) {
            return;
        }

        _boss = Instantiate(_boss, _room.transform);
        _boss.Freeze();
        bossSpawned = true;

        if (_vitalBar == null) {
            VitalBars[] healthbars = FindObjectsOfType<VitalBars>();
            if (healthbars.Count() == 0) {
                Debug.LogError("No Healtbar for boss found");
            }
            _vitalBar = healthbars.Last();
            _vitalBar.ShowVitals(false);
            _vitalBar.SetHealthTarget(_boss.Health);
        }
    }

    void OnRoomEnter() {
        Debug.Log("boss room entered", this);

        GameObject player = FindObjectOfType<ControllerMovement>().gameObject;

        HandleBossSetup(player);
        _vitalBar.ShowVitals();
        LockDors();
    }

    void HandleBossSetup(GameObject player) {
        _boss.AI.SetTarget(player);
        _boss.AI.Trigger();
        _boss.Health.OnHealthChange += UpdateHealthUI;
        _boss.ActivateEnemy();
    }

    void UpdateHealthUI(int health) => _vitalBar.SetHealth(health);

    void LockDors() {
        //throw new NotImplementedException();
    }
}
