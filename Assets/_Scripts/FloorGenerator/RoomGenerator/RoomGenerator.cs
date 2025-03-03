using UnityEngine;

public class RoomGenerator : MonoBehaviour {
    [SerializeField] RoomData _roomData;
    [SerializeField] int tileSize = 20;
    [SerializeField] RoomStats _roomStats;


#if UNITY_EDITOR
    [ContextMenu("GenerateFlors")]
    void SpawnFloors() {
        int size = (int)_roomStats.size + 1;
        SpawnFloors(_roomData.prefabs.floorPrefabs, size, size);
    }
    [ContextMenu("GenerateRoom")]
    public void GenerateRoom() {
        int size = (int)_roomStats.size + 1;
        SpawnFloors(_roomData.prefabs.floorPrefabs, size, size);
    }
#endif

    public void GenerateRoom(RoomData roomData) {
        int floorSize = (int)roomData.stats.size + 1;
        GameObject[] florPrefabs = roomData.prefabs.floorPrefabs;
        SpawnFloors(florPrefabs, floorSize, floorSize);
    }

    void SpawnFloors(GameObject[] floors, int width, int height) {
        Vector3 roomPosition = Vector3.zero;

        float offsetX = (width - 1) * tileSize / 2f;
        float offsetZ = (height - 1) * tileSize / 2f;

        float posX;
        float posZ;

        for (int x = 0; x < width; x++) {
            for (int z = 0; z < height; z++) {
                GameObject tilePrefab = floors[Random.Range(0, floors.Length)];
                posX = (x * tileSize) - offsetX;
                posZ = (z * tileSize) - offsetZ;

                roomPosition.Set(posX, 0, posZ);

                Instantiate(tilePrefab, roomPosition, Quaternion.identity, transform);
            }
        }

    }
}
