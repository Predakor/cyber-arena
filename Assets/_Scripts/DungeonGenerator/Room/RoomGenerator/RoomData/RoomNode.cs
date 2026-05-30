using UnityEngine;

public class RoomNode : Node
{

    [SerializeField] private RoomStats roomStats;
    [SerializeField] private RoomGenerator room;
    [SerializeField] private Vector3 position;
    [SerializeField] private int depth;

    public RoomStats Data { get => roomStats; set => roomStats = value; }
    public Vector3 Position { get => position; set => position = value; }
    public int Depth { get => depth; set => depth = value; }

    private void OnDrawGizmos()
    {
        _ = roomStats.Type switch
        {
            RoomType.Normal => Gizmos.color = Color.blue,
            RoomType.Guarded => Gizmos.color = Color.yellow,
            RoomType.Loot => Gizmos.color = Color.green,
            RoomType.Boss => Gizmos.color = Color.red,
            RoomType.Puzzle => Gizmos.color = Color.magenta,
            RoomType.Special => Gizmos.color = Color.cyan,
            _ => Gizmos.color = Color.white
        };
        float size = RoomGenerator.GetRoomWorldSize(roomStats);
        Gizmos.DrawWireCube(transform.position, new Vector3(size, 1, size));
    }
}

#if UNITY_EDITOR


#endif
