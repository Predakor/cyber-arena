using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Generation/Data/Enemies", fileName = "Level_Enemy_Pool")]
public class EnemyPoolData : ScriptableObject {
    public List<Enemy> enemies;
    public List<Enemy> bosses;


}
