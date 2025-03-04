using UnityEngine;

public class RoomGenerator : MonoBehaviour {
    [SerializeField] RoomData _roomData;
    [SerializeField] int _tileSize = 20;
    [SerializeField] int _doorSize = 4;
    [SerializeField] int _wallSize = 10;
    [SerializeField] int _roomSize = 1;
    [SerializeField] int _sides = 4;

    [SerializeField] Vector3 _wallRotationOffest = Vector3.zero;
    [SerializeField] RoomStats _roomStats;


#if UNITY_EDITOR
    [ContextMenu("GenerateFlors")]
    void SpawnFloors() {
        int size = GetRoomSizeNumber();
        SpawnFloors(_roomData.prefabs.floorPrefabs, size, size);
    }

    [ContextMenu("KillAllCHildren")]
    public void KillAllChildren() {
        for (int i = transform.childCount - 1; i >= 0; i--) {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
#endif 

    public void LoadData(RoomData roomData) {
        _roomData = roomData;
        _roomStats = roomData.stats;
        _roomSize = GetRoomSizeNumber();
        _roomStats = roomData.stats;
    }

    [ContextMenu("GenerateRoom")]
    public void GenerateRoom() {
        GameObject[] floorPrefabs = _roomData.prefabs.floorPrefabs;
        GameObject[] wallPrefabs = _roomData.prefabs.wallPrefabs;
        SpawnFloors(floorPrefabs, _roomSize, _roomSize);
        SpawnWalls(wallPrefabs);
    }
    public void GenerateRoom(RoomData roomData) {
        int floorSize = GetRoomSizeNumber(roomData.stats.size);

        SpawnFloors(roomData.prefabs.floorPrefabs, floorSize, floorSize);
        SpawnWalls(roomData.prefabs.wallPrefabs);
    }

    void SpawnFloors(GameObject[] floors, int width, int height) {
        Vector3 roomPosition = Vector3.zero;

        float offsetX = (width - 1) * _tileSize / 2f;
        float offsetZ = (height - 1) * _tileSize / 2f;

        for (int x = 0; x < width; x++) {
            for (int z = 0; z < height; z++) {
                GameObject tilePrefab = floors[Random.Range(0, floors.Length)];

                float posX = (x * _tileSize) - offsetX;
                float posZ = (z * _tileSize) - offsetZ;

                roomPosition.Set(posX, 0, posZ);

                GameObject floor = Instantiate(tilePrefab, transform);
                floor.transform.SetLocalPositionAndRotation(roomPosition, Quaternion.identity);
            }
        }
    }
    void SpawnWalls(GameObject[] wallPrefabs) {
        int roomWorldSize = _roomSize * _tileSize;
        int roomRadius = roomWorldSize / 2;

        // Rectangular room logic
        if (_sides == 4) {
            SpawnRectangularWalls(wallPrefabs, roomRadius);
        }
        // Polygonal room logic
        else {
            SpawnPolygonalWalls(wallPrefabs, roomRadius);
        }

        void SpawnRectangularWalls(GameObject[] wallPrefabs, int roomRadius) {
            // Left and right walls

            //default wall is 10 units default floor is  so number of floors x2
            float scale = _roomSize * 2;

            for (int i = 0; i < 2; i++) {
                GameObject randomWall = GetRandomPrefab(wallPrefabs);
                int dist = i % 2 == 0 ? roomRadius : roomRadius * -1;
                int rotation = i % 2 == 0 ? 0 : 180;
                Vector3 wallPosition = new(dist, 0, 0);
                Quaternion wallRotation = Quaternion.Euler(0, rotation, 0);
                GenerateWall(randomWall, wallPosition, wallRotation, scale);
            }

            // Up and down walls
            for (int i = 0; i < 2; i++) {
                GameObject randomWall = GetRandomPrefab(wallPrefabs);
                int dist = i % 2 == 0 ? roomRadius : roomRadius * -1;
                int rotation = i % 2 == 0 ? -90 : 90;
                Vector3 wallPosition = new(0, 0, dist);
                Quaternion wallRotation = Quaternion.Euler(0, rotation, 0);
                GenerateWall(randomWall, wallPosition, wallRotation, scale);
            }
        }

        void SpawnPolygonalWalls(GameObject[] wallPrefabs, int roomRadius) {
            float rotationBase = 360 / _sides;

            float wallLength = 2 * roomRadius * Mathf.Tan(Mathf.PI / _sides);

            Vector3 wallScale = new(1, 1, wallLength / _wallSize);
            Vector3 wallPosition = new(0, 0, 0);

            for (int i = 0; i < _sides; i++) {
                GameObject randomWall = GetRandomPrefab(wallPrefabs);
                float angle = rotationBase * i;
                float posX = roomRadius * Mathf.Cos(Mathf.Deg2Rad * angle);
                float posZ = roomRadius * Mathf.Sin(Mathf.Deg2Rad * angle);

                wallPosition.Set(posX, 0, posZ);

                GameObject generatedWall = Instantiate(randomWall, transform);
                generatedWall.transform.localPosition = wallPosition;
                generatedWall.transform.LookAt(transform.position);
                generatedWall.transform.Rotate(_wallRotationOffest);
                generatedWall.transform.localScale = wallScale;
            }
        }
    }



    void GenerateWall(GameObject wall, Vector3 wallPosition, Quaternion wallRotation, float scale) {
        GameObject generatedWall = Instantiate(wall, transform);
        generatedWall.transform.localScale = new Vector3(1, 1, scale);
        generatedWall.transform.SetLocalPositionAndRotation(wallPosition, wallRotation);
    }


    GameObject GetRandomPrefab(GameObject[] prefabs) => prefabs[Random.Range(0, prefabs.Length)];
    int GetRoomSizeNumber(RoomSize size) => (int)size + 1;
    int GetRoomSizeNumber() => (int)_roomData.stats.size + 1;

}
