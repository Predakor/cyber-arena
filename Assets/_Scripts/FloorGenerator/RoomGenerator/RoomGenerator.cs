using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour {
    [SerializeField] RoomStats _roomStats;
    [SerializeField] RoomPrefabs _roomPrefabs;

    [Header("Sizes")]
    [SerializeField] int _tileSize = 20;
    [SerializeField] int _doorSize = 4;
    [SerializeField] int _wallSize = 10;

    [SerializeField] Vector3 _wallRotationOffest = Vector3.zero;

    [SerializeField] List<bool> _doors = new();

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

    void OnValidate() {
        _doors.Clear();
        for (int i = 0; i < _roomStats.sides; i++) {
            bool random = Random.Range(0, 4) == 1;
            _doors.Add(random);
        }
    }

#endif

    public void LoadData(RoomData roomData) {
        _roomStats = roomData.stats;
        _roomPrefabs = roomData.prefabs;
        _doors = new List<bool>() { true, false, true, false };


    }

    [ContextMenu("GenerateRoom")]
    public void GenerateRoom() {
        SpawnFloors();
        SpawnWalls();
        SpawnDoors();
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
        int roomWorldSize = GetRoomSizeNumber() * _tileSize;
        int roomRadius = roomWorldSize / 2;

        SpawnPolygonalWalls();

        void SpawnPolygonalWalls() {
            float rotationBase = 360 / _roomStats.sides;

            float wallLength = 2 * roomRadius * Mathf.Tan(Mathf.PI / _roomStats.sides);

            Vector3 wallScale = new(1, 1, wallLength / _wallSize);
            Vector3 wallPosition = new(0, 0, 0);

            for (int i = 0; i < _roomStats.sides; i++) {
                bool hasDoor = _doors[i];

                float angle = rotationBase * i;
                float posX = roomRadius * Mathf.Cos(Mathf.Deg2Rad * angle);
                float posZ = roomRadius * Mathf.Sin(Mathf.Deg2Rad * angle);

                wallPosition.Set(posX, 0, posZ);

                if (hasDoor) {
                    Vector3 wallSegmentScale = wallScale;
                    float wallRemainingSpace = wallLength - _doorSize;
                    float scaleRatio = wallRemainingSpace / wallLength / 2;
                    float offsetValue = wallRemainingSpace / 4 + (_doorSize / 2);

                    Vector3 offset = new(0, 0, offsetValue);
                    wallSegmentScale.z *= scaleRatio;

                    InstantiateWall(wallSegmentScale, wallPosition, GetRandomWall())
                        .transform.Translate(offset, Space.Self);
                    InstantiateWall(wallSegmentScale, wallPosition, GetRandomWall())
                        .transform.Translate(-offset, Space.Self);

                }
                else {
                    InstantiateWall(wallScale, wallPosition, GetRandomWall());
                }

            }
        }

        GameObject InstantiateWall(Vector3 wallScale, Vector3 wallPosition, GameObject randomWall) {
            GameObject generatedWall = Instantiate(randomWall, transform);
            generatedWall.transform.localPosition = wallPosition;
            generatedWall.transform.LookAt(transform.position);
            generatedWall.transform.Rotate(_wallRotationOffest);
            generatedWall.transform.localScale = wallScale;
            return generatedWall;
        }
    }
    void SpawnDoors() {
        throw new System.NotImplementedException();
    }


    GameObject GetRandomWall() => GetRandomPrefab(_roomPrefabs.wallPrefabs);
    GameObject GetRandomFloor() => GetRandomPrefab(_roomPrefabs.floorPrefabs);
    GameObject GetRandomDoor() => GetRandomPrefab(_roomPrefabs.doorPrefabs);
    GameObject GetRandomPrefab(GameObject[] prefabs) => prefabs[Random.Range(0, prefabs.Length)];

    int GetRoomSizeNumber() => (int)_roomStats.size + 1;
}
