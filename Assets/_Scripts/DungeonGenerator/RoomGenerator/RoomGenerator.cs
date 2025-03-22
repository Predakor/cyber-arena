using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour {
    [SerializeField] TemplatesHolderData _roomTemplates;
    [SerializeField] LevelPrefabs _levelPrefabs;

    [Header("Sizes")]
    [SerializeField] float _tileSize = 20;
    [SerializeField] float _roomHeight = 6;
    [SerializeField] float _doorWidth = 6;

    [SerializeField] Vector3 _wallRotationOffest = Vector3.zero;
    [SerializeField] Vector3 _doorRotationOffset = Vector3.zero;

#if UNITY_EDITOR

    [ContextMenu("KillAllCHildren")]
    public void KillAllChildren() {
        for (int i = transform.childCount - 1; i >= 0; i--) {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }

    [SerializeField] RoomNode _debugSelectNode;
    [ContextMenu("Generate debugNode")]
    public void GenerateDebugNdoe() {
        GenerateRoom(_debugSelectNode);
    }

#endif

    public void Init(LevelPrefabs levelPrefabs, TemplatesHolderData roomTemplates) {
        _roomTemplates = roomTemplates;
        _levelPrefabs = levelPrefabs;
    }

    public void GenerateRoom(RoomNode roomNode) {
        Vector3 position = roomNode.Position;
        Transform transform = roomNode.transform;

        InstantiateTemplate(roomNode.Data.type, position, transform);

        SpawnFloors(roomNode.Data.size, transform);
        SpawnWalls(roomNode, transform);
    }

    void SpawnFloors(RoomSize roomSize, Transform parent) {
        int width = GetRoomSizeNumber(roomSize);
        int height = GetRoomSizeNumber(roomSize);

        Vector3 roomPosition = Vector3.zero;
        float offsetX = (width - 1) * _tileSize / 2f;
        float offsetZ = (height - 1) * _tileSize / 2f;

        for (int x = 0; x < width; x++) {
            for (int z = 0; z < height; z++) {
                GameObject tilePrefab = _levelPrefabs.RandomFloorPrefab();

                float posX = (x * _tileSize) - offsetX;
                float posZ = (z * _tileSize) - offsetZ;

                roomPosition.Set(posX, 0, posZ);

                Instantiate(tilePrefab, parent.position + roomPosition, Quaternion.identity, parent);
            }
        }
    }

    void SpawnWalls(RoomNode roomNode, Transform parent) {
        int roomRadius = roomNode.Data.GetRoomRadius();
        int sides = roomNode.Data.sides;

        List<Vector3> roomDirections = RoomHelpers.GetRoomDirections(sides);
        List<Vector3> connectedWalls = roomNode.GetConnectedDirections();

        float wallLength = 2 * roomRadius * Mathf.Tan(Mathf.PI / sides);

        foreach (var direction in roomDirections) {
            Vector3 position = direction * roomRadius;
            LevelPrefab wall = _levelPrefabs.RandomWall();

            if (wallHasDoor(connectedWalls, direction)) {
                InstatiateWallWithDoor(wallLength, position, parent);
            }
            else {
                Vector3 wallScale = new(1, _roomHeight / wall.dimensions.y, wallLength / wall.dimensions.z);
                InstantiatePrefab(wall.prefab, wallScale, position, parent);
            }
        }

        static bool wallHasDoor(List<Vector3> connectedWalls, Vector3 direction) {
            return connectedWalls.Exists((connectedDirection) => direction == connectedDirection);
        }
    }

    void InstatiateWallWithDoor(float wallLength, Vector3 position, Transform parent) {
        const int segments = 2;

        float wallRemainingSpace = wallLength - _doorWidth;
        float segmentSize = wallRemainingSpace / segments;
        float offsetValue = segmentSize / 2 + (_doorWidth / 2);

        LevelPrefab wall = _levelPrefabs.RandomWall();

        Vector3 offset = new(0, 0, offsetValue);
        Vector3 segmentScale = new(1, _roomHeight / wall.dimensions.y, segmentSize / wall.dimensions.z);

        for (int i = 0; i < 2; i++) {
            InstantiatePrefab(wall.prefab, segmentScale, position, parent)
                .transform.Translate(offset, Space.Self);
            offset *= -1;
        }

        LevelPrefab door = _levelPrefabs.RandomDoor();

        Vector3 doorSize = door.dimensions;
        doorSize.y = _roomHeight / door.dimensions.y;//height
        doorSize.z = _doorWidth / door.dimensions.z;//widht

        InstantiatePrefab(door.prefab, doorSize, position, parent);
    }
    GameObject InstantiatePrefab(GameObject prefab, Vector3 scale, Vector3 position, Transform parent) {
        GameObject generatedPrefab = Instantiate(prefab, parent);
        generatedPrefab.transform.localPosition = position;
        generatedPrefab.transform.LookAt(parent.position);
        generatedPrefab.transform.Rotate(_doorRotationOffset);
        generatedPrefab.transform.localScale = scale;

        return generatedPrefab;
    }

    GameObject InstantiateTemplate(RoomType type, Vector3 position, Transform parent) {
        GameObject template = _roomTemplates.GetRoomTemplate(type);
        return Instantiate(template, position, Quaternion.identity, parent);
    }

    public static int GetRoomWorldSize(RoomStats stats) {
        return GetRoomSizeNumber(stats.size) * 20;
    }

    public static int GetRoomSizeNumber(RoomSize size) => (int)(size + 1);
}