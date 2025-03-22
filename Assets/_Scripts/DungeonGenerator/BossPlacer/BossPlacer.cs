using Helpers.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPlacer : MonoBehaviour {

    [SerializeField] List<Enemy> _bossPool;
    [SerializeField] List<Enemy> _miniBossPool;

    public void Init(EnemyPoolData enemies) {
        _bossPool = enemies.Bosses;
        _miniBossPool = enemies.MiniBosses;
    }

    public void GetBossLocation(List<RoomNode> generatedNodes) {

        var deadEnds = GraphUtils.GetDeadEnds(generatedNodes[0]);

        RoomNode selectedNode = CollectionHelpers.RandomElement(deadEnds) as RoomNode;
        var boss = CollectionHelpers.RandomElement(_bossPool);
        Debug.Log(selectedNode, selectedNode);

    }

}
