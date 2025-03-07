using System.Collections.Generic;
using UnityEngine;

public class FloorGenerator : MonoBehaviour {

    [SerializeField] FloorData _floorDataScriptableObject;

    [SerializeField] BaseFloorStats _baseStats;
    [SerializeField] PrefabPool _floorPool;
    [SerializeField] FloorModifiers _floorModifiers;
    [SerializeField] LootRoomSettings _lootRoomSettings;
    [SerializeField] GuardedRoomSettings _guardedRoomSettings;
    [SerializeField] RoomConnectionSettings _roomConnectionSettings;

    [SerializeField] GameObject roomBasePrefab;
    [SerializeField] RoomData _roomDataTemplate;

    [SerializeField] List<RoomGenerator> _generatedRooms;

#if UNITY_EDITOR

    [ContextMenu("Generate Floor")]
    public void CreateFloor() {
        KillAllChildren();
        ApplyModifiers();
        GenerateFloor();
    }

    [ContextMenu("Load Floor Data")]
    public void LoadData() {
        LoadData(_floorDataScriptableObject);
    }
    [ContextMenu("Spawn all generated rooms")]
    public void GenerateSpawnedRooms() {
        if (_generatedRooms.Count == 0) {
            Debug.LogError("no rooms to generate, generate them first");
            return;
        }
        foreach (RoomGenerator room in _generatedRooms) {
            room.GenerateRoom();
        }
    }

    [ContextMenu("KillAllCHildren")]
    public void KillAllChildren() {
        for (int i = transform.childCount - 1; i >= 0; i--) {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
        _generatedRooms.Clear();
    }

#endif
    public void LoadData(FloorData data) {
        FloorGenerationData floorData = data.GetFloorData();
        _baseStats = floorData.baseStats;
        _floorPool = floorData.floorPool;
        _floorModifiers = floorData.floorModifiers;
        _lootRoomSettings = floorData.lootRoomSettings;
        _guardedRoomSettings = floorData.guardedRoomSettings;
        _roomConnectionSettings = floorData.roomConnectionSettings;

        _roomDataTemplate.LoadPrefabs(_floorPool.floorPrefabs);
    }

    public void ApplyModifiers() {
        _baseStats.difficulty = Mathf.FloorToInt(_baseStats.difficulty * _floorModifiers.difficultyModifier);
        _lootRoomSettings.lootRoomChance *= _floorModifiers.lootModifier;
        _guardedRoomSettings.guardedRoomChance *= _floorModifiers.guardModifier;

        _lootRoomSettings.Validate();
        _guardedRoomSettings.Validate();
        _roomConnectionSettings.Validate();
    }

    public void GenerateFloor() {
        GenerateRooms();
        GenerateLootRooms();
        GenerateGuardedRooms();

    }

    void GenerateRooms() {
        Vector3 currentPosition = transform.position;
        Vector3 lastRoomPosition = transform.position;

        int roomSegmentSize = 20;
        int maxDistFromCenter = 5;


        //first room
        InstantiateRoom(currentPosition, transform, _roomDataTemplate);

        for (int i = 0; i < _baseStats.numberOfRooms; i++) {
            RoomStats roomStats = RandomizeStats(_roomDataTemplate.stats);
            int roomWorldSize = RoomGenerator.GetRoomSizeNumber(roomStats.size) * roomSegmentSize;

            float minDistanceToNextRoom = 2 * roomWorldSize;

            Vector3 backwardDirection = (lastRoomPosition - currentPosition).normalized;

            //check free directionss
            List<Vector3> availablePositions = GetPossibleDirections(roomWorldSize / 2, roomStats.sides, backwardDirection, minDistanceToNextRoom);


            //pick random free direction
            for (int j = 0; j < 1; j++) {
                if (availablePositions.Count == 0) { break; }
                Vector3 randomPosition = availablePositions[Random.Range(0, availablePositions.Count)];
                lastRoomPosition = currentPosition;
                currentPosition = randomPosition;
                availablePositions.Remove(randomPosition);

                //mark room generator to create door there
            }

            _roomDataTemplate.stats = roomStats;
            InstantiateRoom(currentPosition, transform, _roomDataTemplate);

        }


        List<Vector3> GetPossibleDirections(int radius, int sides, Vector3 backwardDirection, float distance) {
            List<Vector3> availablePositions = new();

            float angleBase = 360 / sides;
            Vector3 wallCenterPosition = new();
            Vector3 center = Vector3.zero;

            for (int i = 0; i < sides; i++) {
                float angle = angleBase * i;
                int posX = (int)(radius * Mathf.Cos(Mathf.Deg2Rad * angle));
                int posZ = (int)(radius * Mathf.Sin(Mathf.Deg2Rad * angle));
                wallCenterPosition.Set(posX, 0, posZ);

                Vector3 currentDirection = (wallCenterPosition - center).normalized;

                if (Vector3.Dot(backwardDirection, currentDirection) > 0.99f) { continue; }

                Vector3 positionToCheck = currentDirection * distance + currentPosition;

                if (HasSpaceForRoom(positionToCheck, radius)) {
                    availablePositions.Add(positionToCheck);
                }
            }
            return availablePositions;
        }

        RoomStats RandomizeStats(RoomStats stats) {
            int[] sizes = new int[] { 4, 8 };
            stats.size = (RoomSize)Random.Range(1, 3);
            stats.sides = sizes[Random.Range(0, sizes.Length)];
            return stats;
        }

        bool HasSpaceForRoom(Vector3 position, float radius) {
            int roomMasks = LayerMask.GetMask("Ground");
            Collider[] detectedRooms = Physics.OverlapSphere(position, radius, roomMasks);
            bool colided = detectedRooms.Length == 0;

            return colided;
        }

        void InstantiateRoom(Vector3 position, Transform transform, RoomData data) {
            RoomGenerator generatedRoom = Instantiate(roomBasePrefab, position, Quaternion.identity, transform)
                .GetComponent<RoomGenerator>();
            generatedRoom.LoadData(data);
            _generatedRooms.Add(generatedRoom);
            generatedRoom.gameObject.name = $"Room {data.stats.size}";
        }
    }

    void GenerateLootRooms() {
        if (_lootRoomSettings.maxLootRooms > 0) {
            Debug.Log($"Generating loot rooms with chance: {_lootRoomSettings.lootRoomChance}%");
        }
    }

    void GenerateGuardedRooms() {
        if (_guardedRoomSettings.maxGuardedRooms > 0) {
            Debug.Log($"Generating guarded rooms with chance: {_guardedRoomSettings.guardedRoomChance}%");
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector3(0, 0, 0), new Vector3(20, 20, 20));
        Gizmos.DrawWireSphere(new Vector3(0, 0, 0), 20);
    }
}
