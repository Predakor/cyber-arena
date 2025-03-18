using Helpers.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[RequireComponent(typeof(RoomPlacer), typeof(CorridorPlacer))]
public class LevelGenerator : MonoBehaviour {

    #region variables
    [Header("Dependencies")]
    [SerializeField] RoomPlacer _roomPlacer;
    [SerializeField] CorridorPlacer _corridorPlacer;
    [SerializeField] BossPlacer _bossPlacer;

    [Header("Prefabs/templates")]
    [SerializeField] FloorData _levelDataTemplate;
    [SerializeField] TemplatesHolderData _levelDataTemplateHolder;

    [SerializeField] GameObject _roomTemplate;
    [SerializeField] RoomData _roomDataTemplate;
    [SerializeField] GameObject _corridorTemplate;
    [SerializeField] RoomRestrictionsSO _roomRestrictions;

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

    [ContextMenu("Spawn Generated Level")]
    public void GenerateSpawnedRooms() {
        foreach (var corridor in _generatedCorridors) {
            corridor.GenerateCorridor();
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
        CleanLists();
    }

    [ContextMenu("Load Floor Data")]
    public void LoadData() => LoadData(_levelDataTemplate);

    [ContextMenu("Setup Generator")]
    void LoadDependencies() {
        Init();
        ApplyModifiers();
        SetupDependencies();
    }

#endif

    public void Init(Level level = Level.Neon_City) {
        string templatePath = "LevelData/BaseTemplates/Level_Template_Holder";
        var templates = Resources.Load<TemplatesHolderData>(templatePath);

        if (templates == null) {
            string templatesName = templatePath.Split('/').Last();
            Debug.LogError("templates not found" + " " + templatesName);
        }

        _roomRestrictions = templates.RoomRestrictions;
        _corridorTemplate = templates.CorridorTemplatePrefab;
        _roomDataTemplate = templates.RoomDataTemplate;
        _roomTemplate = templates.GetRoomTemplate();
        _levelDataTemplateHolder = templates;
        Resources.UnloadAsset(templates);

        string levelPath = $"LevelData/{level}";
        var levelData = Resources.Load(levelPath);
        if (levelData == null) {
            Debug.Log($"No data found for {level} level");
        }
        //binding magic
        Resources.UnloadAsset(levelData);
    }

    public void SetupDependencies() {
        InitData roomPlacerData = new() {
            roomPrefab = _roomTemplate,
            roomDataTemplate = _roomDataTemplate,
            roomRestrictions = _roomRestrictions
        };

        CleanEventHandler();

        _roomPlacer.Init(_levelDataTemplateHolder, _roomPlacerData);
        _corridorPlacer.Init(_levelPool.levelPrefabs, _roomPlacerData, _corridorTemplate);
        _bossPlacer.Init(_levelPool.enemiesPool);

        _roomPlacer.OnFirstRoomCreated += OnFirstRoomHandler;
        _roomPlacer.OnRoomCreated += HandleRoomCreation;
        _corridorPlacer.OnCorridorCreated += HandleCorridorCreation;
    }

    public void LoadData(FloorData data) {
        FloorGenerationData floorData = data.GetFloorData();
        _baseStats = floorData.baseStats;
        _levelPool = floorData.floorPool;
        _levelModifiers = floorData.floorModifiers;
        _lootRoomSettings = floorData.lootRoomSettings;
        _guardedRoomSettings = floorData.guardedRoomSettings;
        _roomPlacerData = floorData.roomPlacerData;
    }
    public void ApplyModifiers() {
        _baseStats.difficulty = Mathf.FloorToInt(_baseStats.difficulty * _levelModifiers.difficultyModifier);
        _lootRoomSettings.lootRoomChance *= _levelModifiers.lootModifier;
        _guardedRoomSettings.guardedRoomChance *= _levelModifiers.guardModifier;

        _lootRoomSettings.Validate();
        _guardedRoomSettings.Validate();
    }

    public void GenerateFloor() {
        CleanLists();

        _roomPlacer.GenerateRooms(_roomPlacerData.numberOfRooms);
        GenerateLootRooms();
        GenerateGuardedRooms();
        GenerateBossRoom();


        _corridorPlacer.PlaceCorridors(_generatedRooms);
    }

    void GenerateBossRoom() {
        _bossPlacer.GetBossLocation(_generatedNodes);
    }

    void OnFirstRoomHandler(RoomGenerator room) {
        //place player or mark as first idk
        _generatedNodes.Add(room.RoomNode);
        _generatedRooms.Add(room);
    }

    void HandleRoomCreation(RoomGenerator newRoom, RoomGenerator currentRoom, Vector3 direction) {
        LinkManager.LinkRoomsAndNodes(currentRoom, newRoom, direction);
        _generatedNodes.Add(newRoom.RoomNode);
        _generatedRooms.Add(newRoom);
    }

    void HandleCorridorCreation(CorridorGenerator corridor, CorridorData corridorData) {
        corridor.LoadData(corridorData);
        _generatedCorridors.Add(corridor);
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
                newStats.isGuarded = isGuarded;
                room.RoomStats = newStats;
            }
        }
    }

    void CleanEventHandler() {
        _roomPlacer.OnFirstRoomCreated -= OnFirstRoomHandler;
        _roomPlacer.OnRoomCreated -= HandleRoomCreation;
        _corridorPlacer.OnCorridorCreated -= HandleCorridorCreation;
    }

    void CleanLists() {
        _generatedRooms.Clear();
        _generatedNodes.Clear();
        _generatedCorridors.Clear();

    }
}


public enum Level {
    Neon_City,
    Beaver_City,
}