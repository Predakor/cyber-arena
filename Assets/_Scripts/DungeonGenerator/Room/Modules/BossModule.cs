using Helpers.Collections;
using System.Linq;
using UnityEngine;

public class BossModule : RoomModule {
    [SerializeField] EnemyPoolData _enemyPoolData;
    [SerializeField] Enemy _boss;

    VitalBars _vitalBar;

    public void Init(EnemyPoolData enemyPoolData) {
        _enemyPoolData = enemyPoolData;
    }

    override public void HandlePlayerNearby() {
        Debug.Log("boss Room nearby", this);
        if (IsPreloaded) {
            return;
        }

        PreloadBoss();
        IsPreloaded = true;

        if (_vitalBar == null) {
            HandleBossHealthBar();
        }
    }

    override public void HandlePlayerEnter() {
        Debug.Log("boss room entered", this);

        GameObject player = GetPlayer();

        HandleBossSetup(player);
        _vitalBar.ShowVitals();
    }

    public override void HandlePlayerFaraway() {
        base.HandlePlayerFaraway();
        IsPreloaded = false;
        UnloadBoss();
    }

    void Awake() {
        _boss = CollectionUtils.RandomElement(_enemyPoolData.Bosses);
    }

    void HandleBossHealthBar() {
        VitalBars[] healthbars = FindObjectsOfType<VitalBars>();
        if (healthbars.Count() == 0) {
            Debug.LogError("No Healtbar for boss found");
        }
        _vitalBar = healthbars.Last();
        _vitalBar.ShowVitals(false);
        _vitalBar.SetHealthTarget(_boss.Health);
    }

    void PreloadBoss() {
        _boss = Instantiate(_boss, _room.transform);
        _boss.Health.OnHealthChange += UpdateHealthUI;
        _boss.Freeze();
    }

    void UnloadBoss() {
        _boss.Health.OnHealthChange -= UpdateHealthUI;
        Destroy(_boss);
        _boss = null;
    }

    void HandleBossSetup(GameObject player) {
        _boss.AI.SetTarget(player);
        _boss.AI.Trigger();
        _boss.ActivateEnemy();
    }

    GameObject GetPlayer() => FindObjectOfType<ControllerMovement>().gameObject;
    void UpdateHealthUI(int health) => _vitalBar.SetHealth(health);
}
