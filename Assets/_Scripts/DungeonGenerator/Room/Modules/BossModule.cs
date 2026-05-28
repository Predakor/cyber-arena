using Helpers.Collections;
using System.Linq;
using UnityEngine;

public sealed class BossModule : RoomModule<BossModule>
{
    [SerializeField] private EnemyPoolData _enemyPoolData;
    [SerializeField] private Enemy _boss;

    private VitalBars _vitalBar;

    public void Init(EnemyPoolData enemyPoolData)
    {
        _enemyPoolData = enemyPoolData;
    }



    public override void HandlePlayerNearby()
    {
        logger.Info("boss Room nearby");
        if (IsPreloaded)
        {
            return;
        }

        PreloadBoss();
        IsPreloaded = true;

        if (_vitalBar == null)
        {
            HandleBossHealthBar();
        }
    }

    public override void HandlePlayerEnter()
    {
        logger.Info("boss room entered", this);

        GameObject player = GetPlayer();

        HandleBossSetup(player);
        _vitalBar.ShowVitals();
    }

    public override void HandlePlayerFaraway()
    {
        base.HandlePlayerFaraway();
        IsPreloaded = false;
        UnloadBoss();
    }

    private void Awake()
    {
        _boss = CollectionUtils.RandomElement(_enemyPoolData.Bosses);
    }

    private void HandleBossHealthBar()
    {
        VitalBars[] healthbars = FindObjectsOfType<VitalBars>();
        if (healthbars.Count() == 0)
        {
            Debug.LogError("No Healtbar for boss found");
        }
        _vitalBar = healthbars.Last();
        _vitalBar.ShowVitals(false);
        _vitalBar.SetHealthTarget(_boss.Health);
    }

    private void PreloadBoss()
    {
        _boss = Instantiate(_boss, _room.transform);
        _boss.Health.OnHealthChange += UpdateHealthUI;
        _boss.Freeze();
    }

    private void UnloadBoss()
    {
        _boss.Health.OnHealthChange -= UpdateHealthUI;
        Destroy(_boss);
        _boss = null;
    }

    private void HandleBossSetup(GameObject player)
    {
        _boss.AI.SetTarget(player);
        _boss.AI.Trigger();
        _boss.ActivateEnemy();
    }

    private GameObject GetPlayer() => FindObjectOfType<ControllerMovement>().gameObject;
    private void UpdateHealthUI(int health) => _vitalBar.SetHealth(health);
}
