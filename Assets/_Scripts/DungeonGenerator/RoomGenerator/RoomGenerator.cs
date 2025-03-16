using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RoomGenerator : MonoBehaviour {
    [SerializeField] RoomStats _roomStats;
    [SerializeField] LevelPrefabs _floorPrefabs;
    [SerializeField] RoomLinks _roomLinks;
    [SerializeField] RoomNode _roomNode;

    [Header("Sizes")]
    [SerializeField] int _tileSize = 20;
    [SerializeField] int _doorSize = 4;
    [SerializeField] int _wallSize = 10;

    [SerializeField] Vector3 _wallRotationOffest = Vector3.zero;
    [SerializeField] Vector3 _doorRotationOffset = Vector3.zero;

    BoxCollider _roomCollider;

    public RoomStats RoomStats { get => _roomStats; set => _roomStats = value; }
    public LevelPrefabs FloorPrefabs { get => _floorPrefabs; private set => _floorPrefabs = value; }
    public RoomLinks RoomLinks { get => _roomLinks; set => _roomLinks = value; }
    public RoomNode RoomNode {
        get {
            if (_roomNode == null) {
                Debug.LogWarning("No room node in prefab", this);
                _roomNode = GetComponent<RoomNode>();
            }
            return _roomNode;
        }
        set => _roomNode = value;
    }

#if UNITY_EDITOR

    [ContextMenu("KillAllCHildren")]
    public void KillAllChildren() {
        for (int i = transform.childCount - 1; i >= 0; i--) {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }

    [ContextMenu("RegenerateRoom")]
    public void RegenerateRoom() {
        KillAllChildren();
        GenerateRoom();
    }

#endif

    public void LoadData(RoomData roomData) {
        _roomStats = roomData.stats;
        _floorPrefabs = roomData.prefabs;

        float size = GetRoomWorldSize();

        if (_roomCollider == null) {
            _roomCollider = GetComponent<BoxCollider>();
        }

        _roomCollider.size = new Vector3(size, 1, size);
    }

    [ContextMenu("GenerateRoom")]
    public void GenerateRoom() {
        SpawnFloors();
        SpawnWalls();
    }

    [ContextMenu("GenerateFlors")]
    void SpawnFloors() {
        int width = GetRoomSizeNumber();
        int height = GetRoomSizeNumber();

        Vector3 roomPosition = Vector3.zero;
        float offsetX = (width - 1) * _tileSize / 2f;
        float offsetZ = (height - 1) * _tileSize / 2f;

        for (int x = 0; x < width; x++) {
            for (int z = 0; z < height; z++) {
                GameObject tilePrefab = _floorPrefabs.RandomFloor().prefab;

                float posX = (x * _tileSize) - offsetX;
                float posZ = (z * _tileSize) - offsetZ;

                roomPosition.Set(posX, 0, posZ);

                Instantiate(tilePrefab, transform.position + roomPosition, Quaternion.identity, transform);
            }
        }
    }

    void SpawnWalls() {
        int roomRadius = GetRoomRadius();

        List<Vector3> roomDirections = RoomHelpers.GetRoomDirections(_roomStats.sides);
        List<Vector3> connectedWalls = _roomLinks.GetConnectedDirections();

        float wallLength = 2 * roomRadius * Mathf.Tan(Mathf.PI / _roomStats.sides);
        Vector3 wallScale = new(1, 1, wallLength / _wallSize);

        foreach (var direction in roomDirections) {
            Vector3 position = direction * roomRadius;

            bool hasDoor = connectedWalls.Exists((connectedDirection) => direction == connectedDirection);

            if (!hasDoor) {
                InstantiateWall(wallScale, position);
            }
            else {
                InstatiateWallWithDoor(wallLength, wallScale, position);
            }
        }
    }

    void Awake() {
        if (!_roomCollider) {
            _roomCollider = GetComponent<BoxCollider>();
        }
    }

    void InstatiateWallWithDoor(float wallLength, Vector3 wallScale, Vector3 position) {
        Vector3 wallSegmentScale = wallScale;
        float wallRemainingSpace = wallLength - _doorSize;
        float scaleRatio = wallRemainingSpace / wallLength / 2;
        float offsetValue = wallRemainingSpace / 4 + (_doorSize / 2);

        Vector3 offset = new(0, 0, offsetValue);
        wallSegmentScale.z *= scaleRatio;

        InstantiateWall(wallSegmentScale, position).transform.Translate(offset, Space.Self);
        InstantiateWall(wallSegmentScale, position).transform.Translate(-offset, Space.Self);
        InstantiateDoor(position);
    }

    GameObject InstantiateDoor(Vector3 position) {
        GameObject generatedDoor = Instantiate(_floorPrefabs.RandomDoor().prefab, transform);
        generatedDoor.transform.localPosition = position;
        generatedDoor.transform.LookAt(transform.position);
        generatedDoor.transform.Rotate(_doorRotationOffset);
        return generatedDoor;
    }

    GameObject InstantiateWall(Vector3 wallScale, Vector3 wallPosition) {
        GameObject generatedWall = Instantiate(_floorPrefabs.RandomWall().prefab, wallPosition, Quaternion.identity, transform);
        generatedWall.transform.localPosition = wallPosition;
        generatedWall.transform.LookAt(transform.position);
        generatedWall.transform.Rotate(_wallRotationOffest);
        generatedWall.transform.localScale = wallScale;
        return generatedWall;
    }

    public int GetRoomRadius() => (GetRoomSizeNumber() * _tileSize) / 2;
    public float GetRoomWorldSize() => GetRoomSizeNumber() * _tileSize;
    public static int GetRoomWorldSize(RoomStats stats) {
        return GetRoomSizeNumber(stats.size) * 20;
    }

    int GetRoomSizeNumber() => (int)_roomStats.size + 1;
    public static int GetRoomSizeNumber(RoomSize size) => (int)(size + 1);

    void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        if (RoomStats.IsGuarded) {
            Gizmos.color = Color.red;
        }

        if (RoomStats.hasTreasure) {
            Gizmos.color = Color.green;
        }

        float size = GetRoomSizeNumber() * _tileSize;
        Gizmos.DrawWireCube(transform.position, new Vector3(size, 1, size));
    }
}
