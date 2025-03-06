using UnityEngine;

public class RoomGenerator : MonoBehaviour {
    [SerializeField] RoomStats _roomStats;
    [SerializeField] RoomPrefabs _roomPrefabs;

    [Header("Sizes")]
    [SerializeField] int _tileSize = 20;
    [SerializeField] int _doorSize = 4;
    [SerializeField] int _wallSize = 10;

    [SerializeField] Vector3 _wallRotationOffest = Vector3.zero;

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
        _roomPrefabs = roomData.prefabs;
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
        int roomWorldSize = GetRoomSizeNumber() * _tileSize;
        int roomRadius = roomWorldSize / 2;

        SpawnPolygonalWalls();

        void SpawnPolygonalWalls() {
            float rotationBase = 360 / _roomStats.sides;

            float wallLength = 2 * roomRadius * Mathf.Tan(Mathf.PI / _roomStats.sides);

            Vector3 wallScale = new(1, 1, wallLength / _wallSize);
            Vector3 wallPosition = new(0, 0, 0);

            for (int i = 0; i < _roomStats.sides; i++) {
                float angle = rotationBase * i;
                float posX = roomRadius * Mathf.Cos(Mathf.Deg2Rad * angle);
                float posZ = roomRadius * Mathf.Sin(Mathf.Deg2Rad * angle);

                wallPosition.Set(posX, 0, posZ);

                InstantiateWall(wallScale, wallPosition, GetRandomWall());
            }
        }

        void InstantiateWall(Vector3 wallScale, Vector3 wallPosition, GameObject randomWall) {
            GameObject generatedWall = Instantiate(randomWall, transform);
            generatedWall.transform.localPosition = wallPosition;
            generatedWall.transform.LookAt(transform.position);
            generatedWall.transform.Rotate(_wallRotationOffest);
            generatedWall.transform.localScale = wallScale;
        }
    }



    GameObject GetRandomWall() => GetRandomPrefab(_roomPrefabs.wallPrefabs);
    GameObject GetRandomFloor() => GetRandomPrefab(_roomPrefabs.floorPrefabs);
    GameObject GetRandomDoor() => GetRandomPrefab(_roomPrefabs.doorPrefabs);
    GameObject GetRandomPrefab(GameObject[] prefabs) => prefabs[Random.Range(0, prefabs.Length)];

    int GetRoomSizeNumber() => (int)_roomStats.size + 1;
}
