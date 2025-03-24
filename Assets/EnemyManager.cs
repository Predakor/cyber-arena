using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
    [SerializeField] Room room;
    [SerializeField] EnemyPoolData _avaiableEnemies;
    [SerializeField] List<Enemy> _enemies = new();
    [SerializeField] List<Enemy> _spawnedEnemies = new();

    public int enemiesToGenerate = 4;

    public void Init(EnemyPoolData levelEnemies) {
        _avaiableEnemies = levelEnemies;

    }

    public void RequestEnemies() {
        //call for level enemies manager to get some prefabs?
    }

    public void GenerateEnemies() {
        for (int i = 0; i < enemiesToGenerate; i++) {
            Enemy randomEnemy = _avaiableEnemies.GetRandomEnemy();
            _enemies.Add(randomEnemy);
        }
    }

    public void SpawnEnemies() {
        if (_spawnedEnemies.Count > _enemies.Count) { return; }
        if (_enemies.Count == 0) { GenerateEnemies(); }

        Vector3 roomCenter = transform.position;
        Vector3 offset = roomCenter;

        _enemies.ForEach(enemy => {
            float offsetX = Random.Range(-40, 40);
            float offsetZ = Random.Range(-40, 40);

            offset.Set(offsetX, 1, offsetZ);
            Vector3 position = roomCenter + offset;

            GameObject go = Instantiate(enemy.gameObject, transform);
            _spawnedEnemies.Add(enemy);
        });
    }

    public void ActivateEnemies() {
        if (_spawnedEnemies.Count == 0) {
            SpawnEnemies();
        }

        foreach (Enemy enemy in _spawnedEnemies) {
            enemy.gameObject.SetActive(true);
        }
    }
    public void DisableEnemies() {
        if (_spawnedEnemies.Count <= 0) {
            return;
        }
        foreach (Enemy enemy in _spawnedEnemies) {
            enemy.gameObject.SetActive(false);
        }

    }
    void OnEnable() {
        room.OnPlayerNearby += OnPlayerNearby;
        room.OnRoomEnter += OnRoomEnter;
        room.OnRoomExit += OnRoomExit;
    }

    void OnDisable() {
        room.OnPlayerNearby -= OnPlayerNearby;
        room.OnRoomEnter -= OnRoomEnter;
        room.OnRoomExit -= OnRoomExit;
    }



    void OnRoomExit() {
        DisableEnemies();
    }

    void OnRoomEnter() {
        ActivateEnemies();
    }

    void OnPlayerNearby() {
        SpawnEnemies();
    }


}
