using Helpers.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(RoomPlacer), typeof(CorridorPlacer))]
public class LevelGenerator : MonoBehaviour {

    #region variables
    [Header("Dependencies")]
    [SerializeField] RoomPlacer _roomPlacer;
    [SerializeField] CorridorPlacer _corridorPlacer;

    [Header("Prefabs/templates")]
    [SerializeField] FloorData _levelDataTemplate;
    [SerializeField] GameObject _roomTemplate;
    [SerializeField] RoomData _roomDataTemplate;
    [SerializeField] RoomRestrictionsSO _roomRestrictions;
    [SerializeField] GameObject _corridorTemplate;

    [Header("Override base template")]
    [SerializeField] BaseFloorStats _baseStats;
    [SerializeField] FloorModifiers _levelModifiers;
    [SerializeField] LootRoomSettings _lootRoomSettings;
    [SerializeField] GuardedRoomSettings _guardedRoomSettings;
    [SerializeField] PrefabPool _levelPool;
    [SerializeField] PlacerData _roomPlacerData;

    [Header("Generated structures")]
    [SerializeField] List<RoomNode> _generatedNodes;
    [SerializeField] List<RoomGenerator> _generatedRooms;
    [SerializeField] List<CorridorGenerator> _generatedCorridors;

    #endregion

#if UNITY_EDITOR

    [ContextMenu("Generate Floor")]
    public void CreateFloor() {
        KillAllChildren();
        ApplyModifiers();
        GenerateFloor();
    }
    [ContextMenu("Load Floor Data")]
    public void LoadData() {
        LoadData(_levelDataTemplate);
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

    [ContextMenu("Spawn corridors")]
    public void SpawnAllCorridors() {
        foreach (var corridor in _generatedCorridors) {
            corridor.GenerateCorridor();
        }
    }

    [ContextMenu("KillAllCHildren")]
    public void KillAllChildren() {
        for (int i = transform.childCount - 1; i >= 0; i--) {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
        _generatedRooms.Clear();
    }

    [ContextMenu("Load dependencies")]
    void LoadDependencies() { SetupDependencies(); }

#endif
    public void LoadData(FloorData data) {
        FloorGenerationData floorData = data.GetFloorData();
        _baseStats = floorData.baseStats;
        _levelPool = floorData.floorPool;
        _levelModifiers = floorData.floorModifiers;
        _lootRoomSettings = floorData.lootRoomSettings;
        _guardedRoomSettings = floorData.guardedRoomSettings;
        _roomPlacerData = floorData.roomPlacerData;
        _roomDataTemplate.LoadPrefabs(_levelPool.levelPrefabs);
    }
    public void ApplyModifiers() {
        _baseStats.difficulty = Mathf.FloorToInt(_baseStats.difficulty * _levelModifiers.difficultyModifier);
        _lootRoomSettings.lootRoomChance *= _levelModifiers.lootModifier;
        _guardedRoomSettings.guardedRoomChance *= _levelModifiers.guardModifier;

        _lootRoomSettings.Validate();
        _guardedRoomSettings.Validate();
    }

    public void SetupDependencies() {
        _roomPlacer.Init(
            new() {
                roomPrefab = _roomTemplate,
                roomDataTemplate = _roomDataTemplate,
                roomRestrictions = _roomRestrictions

            },
            _roomPlacerData);
        _roomPlacer.OnFirstRoomCreated += OnFirstRoomHandler;
        _roomPlacer.OnRoomCreated += HandleRoomCreation;
        _corridorPlacer.LoadData(_roomDataTemplate, _roomPlacerData);
    }

    public void GenerateFloor() {
        _generatedRooms.Clear();
        _generatedNodes.Clear();
        _generatedCorridors.Clear();

        _roomPlacer.GenerateRooms(_roomPlacerData.numberOfRooms);
        GenerateLootRooms();
        GenerateGuardedRooms();
        _generatedCorridors = _corridorPlacer.PlaceCorridors(_generatedRooms, _corridorTemplate);
    }

    void OnFirstRoomHandler(RoomGenerator obj) => _generatedRooms.Add(obj);
    void HandleRoomCreation(RoomGenerator newRoom, RoomGenerator currentRoom, Vector3 direction) {
        LinkManager.LinkRoomsAndNodes(currentRoom, newRoom, direction);
        _generatedRooms.Add(newRoom);
    }

    void GenerateLootRooms() {
        int maxLootRoms = _lootRoomSettings.maxLootRooms;
        int gurantedLootRooms = _lootRoomSettings.guaranteedLootRooms;
        int generatedLootRooms = 0;

        for (int i = 0; i < gurantedLootRooms; i++) {
            RoomGenerator lootRoom = CollectionHelpers.RandomElement(_generatedRooms, 1);
            CreateLootRoom(lootRoom);
        }

        bool lootRoomLimitSurpased = generatedLootRooms >= _lootRoomSettings.maxLootRooms;
        if (lootRoomLimitSurpased) {
            return;
        }

        float lootRoomChance = _lootRoomSettings.lootRoomChance;
        for (int i = _generatedRooms.Count - 1; i > 0; i--) {
            if (generatedLootRooms >= maxLootRoms) {
                return;
            }

            bool succesfulRoll = Random.Range(0, 101) > lootRoomChance;
            if (succesfulRoll) {
                CreateLootRoom(_generatedRooms[i]);
            };
        }


        void CreateLootRoom(RoomGenerator lootRoom) {
            RoomStats newStats = lootRoom.RoomStats;
            newStats.hasTreasure = true;
            lootRoom.RoomStats = newStats;
            generatedLootRooms++;
        }
    }

    void GenerateGuardedRooms() {
        float hostileRoomChance = _guardedRoomSettings.guardedRoomChance;
        for (int i = 1; i < _generatedRooms.Count; i++) {


            float roll = Random.Range(0, 101);
            bool isGuarded = roll > hostileRoomChance;

            if (isGuarded) {
                RoomGenerator room = _generatedRooms[i];
                RoomStats newStats = room.RoomStats;
                newStats.IsGuarded = isGuarded;
                room.RoomStats = newStats;
            }
        }
    }
}
