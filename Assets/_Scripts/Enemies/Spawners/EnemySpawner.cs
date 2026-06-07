using System.Collections;
using Systems.Shared;
using UnityEngine;

namespace Scripts.Enemies.Spawners
{
    public sealed class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Enemy _prefab;
        [SerializeField] private Transform _spawnPoint;

        //Configuration
        [SerializeField] private int _waveSize = 50;
        [SerializeField] private float _spawnInterval = 0.5f;
        [SerializeField] private int _maxEnemiesPerTickSpawn = 5;


        [SerializeField] private Vector3 _spawnOffset = Vector3.up;
        [SerializeField] private float _spawnRadius = 3f;
        private GameManager _manager;

        private void Start()
        {
            _manager = GameManager.Instance;
        }

        [ContextMenu("Spawn enemies")]
        public void Spawn()
        {
            CoroutineRunner.Run(SpawnWithInterval());

        }

        public IEnumerator SpawnWithInterval()
        {
            var remaining = _waveSize;

            var waitTime = new WaitForSeconds(_spawnInterval);
            var player = _manager.Player.gameObject;

            var spawnPoint = _spawnPoint;

            while (remaining > 0)
            {
                var enemyCountToSpawn = Mathf.Min(remaining, _maxEnemiesPerTickSpawn);

                remaining -= enemyCountToSpawn;
                for (int i = 0; i < enemyCountToSpawn; i++)
                {
                    //should be Enemy Factory


                    Vector2 randomCirclePoint = Random.insideUnitCircle * _spawnRadius;

                    // 2. Convert that 2D circle point into a 3D offset (putting the 2D 'Y' into the 3D 'Z')
                    Vector3 spawnOffset = new(randomCirclePoint.x, 0f, randomCirclePoint.y);
                    Vector3 randomizedSpawnPoint = _spawnPoint.position + spawnOffset + _spawnOffset;


                    var enemy = Instantiate(_prefab, randomizedSpawnPoint, _spawnPoint.transform.rotation);
                    enemy.AI.SetTarget(player);
                    enemy.SetEnemy(true);
                }


                yield return waitTime;
            }
        }

    }
}
