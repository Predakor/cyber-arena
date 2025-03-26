using Helpers.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Generation/Data/Enemies", fileName = "Level_Enemy_Pool")]
public class EnemyPoolData : ScriptableObject {
    [SerializeField] List<Enemy> enemies;
    [SerializeField] List<Enemy> miniBosses;
    [SerializeField] List<Enemy> bosses;

    public List<Enemy> Enemies => enemies;
    public List<Enemy> MiniBosses => miniBosses;
    public List<Enemy> Bosses => bosses;

    public Enemy GetRandomEnemy() => CollectionUtils.RandomElement(enemies);
    public Enemy GetRandomBoss() => CollectionUtils.RandomElement(bosses);


}
