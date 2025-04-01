using System.Collections.Generic;
using UnityEngine;

public class EnemyModule : RoomModule {
    [SerializeField] EnemyPoolData _avaiableEnemies;
    [SerializeField] List<Enemy> _enemies = new();

    [SerializeField] int _enemiesToGenerate = 4;
    [SerializeField] int _spawnedEnemies = 0;

    public void Init(EnemyPoolData levelEnemies) {
        _avaiableEnemies = levelEnemies;
    }

    public void RequestEnemies() {
        //call for level enemies manager to get some prefabs?
    }

    public override void HandlePlayerEnter() => ActivateEnemies();
    public override void HandlePlayerExit() => DisableEnemies();
    public override void HandlePlayerNearby() => PreloadEnemies();
    public override void HandlePlayerFaraway() => UnloadEnemies();

    void ActivateEnemies() => SetEnemies(true);
    void DisableEnemies() => SetEnemies(false);

    void PreloadEnemies() {
        if (!IsPreloaded) {
            SpawnEnemies();
            IsPreloaded = true;
        }
    }

    void UnloadEnemies() {
        foreach (Enemy enemy in _enemies) {
            enemy.OnDeath -= HandleEnemyDeath;
            Destroy(enemy);
        }
        IsPreloaded = false;
    }

    void SpawnEnemies() {
        GameObject player = GetPlayer();
        Transform parent = transform.parent;
        Vector3 roomCenter = transform.position;
        Vector3 offset = Vector3.zero;

        for (int i = 0; i < _enemiesToGenerate; i++) {
            Enemy randomEnemy = _avaiableEnemies.GetRandomEnemy();

            float offsetX = RoomHelpers.GetPointInsideRoom(_room);
            float offsetZ = RoomHelpers.GetPointInsideRoom(_room);

            offset.Set(offsetX, 1, offsetZ);
            Vector3 position = roomCenter + offset;

            Enemy enemy = Instantiate(randomEnemy, position, Quaternion.identity, parent);

            enemy.OnDeath += HandleEnemyDeath;
            enemy.AI.SetTarget(player);
            enemy.Freeze();

            _enemies.Add(enemy);
            _spawnedEnemies++;
        }
    }

    void HandleEnemyDeath(Enemy enemy) {
        _enemies.Remove(enemy);
        enemy.OnDeath -= HandleEnemyDeath;
    }

    void SetEnemies(bool active) {
        if (!HasPreloadedEnemies()) {
            return;
        }

        _enemies.ForEach(enemy => enemy.SetEnemy(active));
    }
    GameObject GetPlayer() => FindObjectOfType<ControllerMovement>().gameObject;
    bool HasPreloadedEnemies() => _enemies.Count > 0;
}
