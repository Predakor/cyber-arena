using Helpers.Collections;
using System.Collections.Generic;
using System.Linq;
using Systems.Shared.Loggers;
using UnityEngine;


[RequireComponent(typeof(RoomPlacer), typeof(CorridorPlacer))]
[RequireComponent(typeof(RoomGenerator))]
public class LevelGenerator : MonoBehaviour
{

    #region variables
    [Header("Dependencies")]
    [SerializeField] private RoomPlacer _roomPlacer;
    [SerializeField] private BossPlacer _bossPlacer;
    [SerializeField] private CorridorPlacer _corridorPlacer;
    [SerializeField] private RoomGenerator _roomGenerator;

    [Header("Prefabs/templates")]
    [SerializeField] private FloorData _levelDataTemplate;
    [SerializeField] private TemplatesHolderData _levelDataTemplateHolder;
    [SerializeField] private GameObject _collisionChecker;

    [SerializeField] private GameObject _roomTemplate;
    [SerializeField] private GameObject _corridorTemplate;
    [SerializeField] private RoomRestrictionsSO _roomRestrictions;

    [Header("Override base template")]
    [SerializeField] private BaseFloorStats _baseStats;
    [SerializeField] private FloorModifiers _levelModifiers;
    [SerializeField] private LootRoomSettings _lootRoomSettings;
    [SerializeField] private GuardedRoomSettings _guardedRoomSettings;
    [SerializeField] private PrefabPool _levelPool;
    [SerializeField] private PlacerData _roomPlacerData;

    [Header("Generated structures")]
    [SerializeField] private List<RoomNode> _generatedNodes;
    [SerializeField] private List<RoomGenerator> _generatedRooms;
    [SerializeField] private List<CorridorGenerator> _generatedCorridors;

    #endregion

    private IGameLogger _logger;
#if UNITY_EDITOR

    [ContextMenu("Generate Floor")]
    public void CreateFloor()
    {
        KillAllChildren();
        ApplyModifiers();
        GenerateFloor();
    }

    [ContextMenu("Spawn Generated Level")]
    public void GenerateSpawnedRooms()
    {
        foreach (var corridor in _generatedCorridors)
        {
            corridor.GenerateCorridor();
        }
        foreach (RoomNode roomNode in _generatedNodes)
        {
            _roomGenerator.GenerateRoom(roomNode);
        }
    }

    [ContextMenu("KillAllCHildren")]
    public void KillAllChildren()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
        CleanLists();
    }

    [ContextMenu("Load Floor Data")]
    public void LoadData() => LoadData(_levelDataTemplate);

    [ContextMenu("Setup Generator")]
    private void LoadDependencies()
    {
        Init();
        ApplyModifiers();
        InitDependencies();
    }

#endif

    private void Awake()
    {
        _logger = GameLogger.GetOrAdd<LevelGenerator>();
    }
    public void Init(Level level = Level.Neon_City)
    {
        string templatePath = "LevelData/BaseTemplates/Level_Template_Holder";
        var templates = Resources.Load<TemplatesHolderData>(templatePath);

        if (templates == null)
        {
            string templatesName = templatePath.Split('/').Last();
            _logger.Error($"templates not found {templatesName}");
        }

        _roomRestrictions = templates.RoomRestrictions;
        _corridorTemplate = templates.CorridorTemplatePrefab;
        _roomTemplate = templates.GetRoomTemplate();
        _levelDataTemplateHolder = templates;
        Resources.UnloadAsset(templates);

        string levelPath = $"LevelData/{level}";
        var levelData = Resources.Load(levelPath);
        if (levelData == null)
        {
            _logger.Error($"No data found for {level} level");
        }
        Resources.UnloadAsset(levelData);
    }

    public void InitDependencies()
    {
        InitData roomPlacerData = new()
        {
            roomPrefab = _roomTemplate,
            roomRestrictions = _roomRestrictions
        };

        CleanEventHandler();

        _roomPlacer.Init(_collisionChecker, _levelDataTemplateHolder, _roomPlacerData);
        _corridorPlacer.Init(_levelPool.levelPrefabs, _roomPlacerData, _corridorTemplate);
        _bossPlacer.Init(_levelPool.enemiesPool);
        _roomGenerator.Init(_levelPool.levelPrefabs, _levelDataTemplateHolder);

        _roomPlacer.OnFirstRoomCreated += FirstRoomNodeHandler;
        _roomPlacer.OnRoomCreated += HandleRoomNodeCreation;
        _corridorPlacer.OnCorridorCreated += HandleCorridorCreation;
    }

    public void LoadData(FloorData data)
    {
        FloorGenerationData floorData = data.GetFloorData();
        _baseStats = floorData.baseStats;
        _levelPool = floorData.floorPool;
        _levelModifiers = floorData.floorModifiers;
        _lootRoomSettings = floorData.lootRoomSettings;
        _guardedRoomSettings = floorData.guardedRoomSettings;
        _roomPlacerData = floorData.roomPlacerData;
    }
    public void ApplyModifiers()
    {
        _baseStats.difficulty = Mathf.FloorToInt(_baseStats.difficulty * _levelModifiers.difficultyModifier);
        _lootRoomSettings.lootRoomChance *= _levelModifiers.lootModifier;
        _guardedRoomSettings.guardedRoomChance *= _levelModifiers.guardModifier;

        _lootRoomSettings.Validate();
        _guardedRoomSettings.Validate();
    }

    public void GenerateFloor()
    {
        CleanLists();

        _roomPlacer.GenerateRooms(_roomPlacerData.numberOfRooms);
        GenerateGuardedRooms();
        GenerateLootRooms();
        GenerateBossRoom();
        //generate placed rooms
        _corridorPlacer.PlaceCorridors(_generatedNodes);

    }

    private void GenerateBossRoom()
    {
        RoomNode bossNode = _bossPlacer.GetBossLocation(_generatedNodes);
        bossNode.Data.SetType(RoomType.Boss);
    }

    private void FirstRoomNodeHandler(RoomNode roomNode)
    {
        _generatedNodes.Add(roomNode);
    }

    private void HandleRoomNodeCreation(RoomNode newRoom, RoomNode currentRoom, Vector3 direction)
    {
        LinkManager.LinkRoomsAndNodes(currentRoom, newRoom, direction);
        _generatedNodes.Add(newRoom);
    }

    private void HandleCorridorCreation(CorridorGenerator corridor, CorridorData corridorData)
    {
        corridor.LoadData(corridorData);
        _generatedCorridors.Add(corridor);
    }

    private void GenerateLootRooms()
    {
        int maxLootRoms = _lootRoomSettings.maxLootRooms;
        int gurantedLootRooms = _lootRoomSettings.guaranteedLootRooms;
        int generatedLootRooms = 0;

        for (int i = 0; i < gurantedLootRooms; i++)
        {
            RoomNode lootRoom = _generatedNodes.GetRandom(1);
            CreateLootRoom(lootRoom);
        }

        bool lootRoomLimitSurpased = generatedLootRooms >= _lootRoomSettings.maxLootRooms;
        if (lootRoomLimitSurpased)
        {
            return;
        }

        float lootRoomChance = _lootRoomSettings.lootRoomChance;
        for (int i = _generatedNodes.Count - 1; i > 0; i--)
        {
            if (generatedLootRooms >= maxLootRoms)
            {
                return;
            }

            bool succesfulRoll = Random.Range(0, 101) > lootRoomChance;
            if (succesfulRoll)
            {
                CreateLootRoom(_generatedNodes[i]);
            }
        }

        void CreateLootRoom(RoomNode lootNode)
        {
            lootNode.Data.SetType(RoomType.Loot);
            generatedLootRooms++;
        }
    }

    private void GenerateGuardedRooms()
    {
        float hostileRoomChance = _guardedRoomSettings.guardedRoomChance;
        for (int i = 1; i < _generatedNodes.Count; i++)
        {

            float roll = Random.Range(0, 101);
            bool isGuarded = roll > hostileRoomChance;

            if (isGuarded)
            {
                RoomNode node = _generatedNodes[i];
                node.Data.SetType(RoomType.Guarded);
            }
        }
    }

    private void CleanEventHandler()
    {
        _roomPlacer.OnFirstRoomCreated -= FirstRoomNodeHandler;
        _roomPlacer.OnRoomCreated -= HandleRoomNodeCreation;
        _corridorPlacer.OnCorridorCreated -= HandleCorridorCreation;
    }

    private void CleanLists()
    {
        _generatedRooms.Clear();
        _generatedNodes.Clear();
        _generatedCorridors.Clear();

    }
}


public enum Level
{
    Neon_City,
    Beaver_City,
}