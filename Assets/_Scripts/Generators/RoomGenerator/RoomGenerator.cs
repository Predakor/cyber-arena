using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RoomGenerator : MonoBehaviour {
    [SerializeField] RoomStats _roomStats;
    [SerializeField] FloorPrefabs _floorPrefabs;
    [SerializeField] RoomLinks _roomLinks;

    [Header("Sizes")]
    [SerializeField] int _tileSize = 20;
    [SerializeField] int _doorSize = 4;
    [SerializeField] int _wallSize = 10;

    [SerializeField] Vector3 _wallRotationOffest = Vector3.zero;
    [SerializeField] Vector3 _doorRotationOffset = Vector3.zero;

    BoxCollider _roomCollider;

    public RoomStats RoomStats { get => _roomStats; private set => _roomStats = value; }
    public FloorPrefabs FloorPrefabs { get => _floorPrefabs; private set => _floorPrefabs = value; }
    public RoomLinks RoomLinks { get => _roomLinks; set => _roomLinks = value; }

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
        //_roomLinks = roomData.links;

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
        Vector3 roomPosition = Vector3.zero;

        int width = GetRoomSizeNumber();
        int height = GetRoomSizeNumber();

        float offsetX = (width - 1) * _tileSize / 2f;
        float offsetZ = (height - 1) * _tileSize / 2f;

        for (int x = 0; x < width; x++) {
            for (int z = 0; z < height; z++) {
                GameObject tilePrefab = GetRandomFloor();

                float posX = (x * _tileSize) - offsetX;
                float posZ = (z * _tileSize) - offsetZ;

                roomPosition.Set(posX, 0, posZ);

                GameObject floor = Instantiate(tilePrefab, transform);
                floor.transform.SetLocalPositionAndRotation(roomPosition, Quaternion.identity);
            }
        }
    }

    void SpawnWalls() {
        int roomRadius = GetRoomRadius();

        SpawnPolygonalWalls();

        void SpawnPolygonalWalls() {
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
        GameObject generatedDoor = Instantiate(GetRandomDoor(), transform);
        generatedDoor.transform.localPosition = position;
        generatedDoor.transform.LookAt(transform.position);
        generatedDoor.transform.Rotate(_doorRotationOffset);
        return generatedDoor;

    }

    GameObject InstantiateWall(Vector3 wallScale, Vector3 wallPosition) {
        GameObject generatedWall = Instantiate(GetRandomWall(), transform);
        generatedWall.transform.localPosition = wallPosition;
        generatedWall.transform.LookAt(transform.position);
        generatedWall.transform.Rotate(_wallRotationOffest);
        generatedWall.transform.localScale = wallScale;
        return generatedWall;
    }

    GameObject GetRandomWall() => GetRandomPrefab(_floorPrefabs.wallPrefabs);
    GameObject GetRandomFloor() => GetRandomPrefab(_floorPrefabs.floorPrefabs);
    GameObject GetRandomDoor() => GetRandomPrefab(_floorPrefabs.doorPrefabs);
    GameObject GetRandomPrefab(GameObject[] prefabs) => prefabs[Random.Range(0, prefabs.Length)];

    public int GetRoomRadius() => (GetRoomSizeNumber() * _tileSize) / 2;
    public float GetRoomWorldSize() => GetRoomSizeNumber() * _tileSize;

    int GetRoomSizeNumber() => (int)_roomStats.size + 1;
    public static int GetRoomSizeNumber(RoomSize size) => (int)(size + 1);

    void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        float size = GetRoomSizeNumber() * _tileSize;
        Gizmos.DrawWireCube(transform.position, new Vector3(size, 1, size));
    }
}
