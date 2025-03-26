using UnityEngine;

public class RoomNode : Node {

    [SerializeField] RoomStats roomStats;
    [SerializeField] RoomGenerator room;
    [SerializeField] Vector3 position;
    [SerializeField] int depth;

    public RoomStats Data { get => roomStats; set => roomStats = value; }
    public Vector3 Position { get => position; set => position = value; }
    public int Depth { get => depth; set => depth = value; }

    void OnDrawGizmos() {
        switch (roomStats.Type) {
            case RoomType.Normal: Gizmos.color = Color.blue; break;
            case RoomType.Guarded: Gizmos.color = Color.red; break;
            case RoomType.Loot: Gizmos.color = Color.green; break;
            case RoomType.Boss: Gizmos.color = Color.black; break;
            case RoomType.Puzzle: Gizmos.color = Color.yellow; break;
            case RoomType.Special: Gizmos.color = Color.white; break;
        }
        float size = RoomGenerator.GetRoomWorldSize(roomStats);
        Gizmos.DrawWireCube(transform.position, new Vector3(size, 1, size));
    }
}

#if UNITY_EDITOR


#endif
