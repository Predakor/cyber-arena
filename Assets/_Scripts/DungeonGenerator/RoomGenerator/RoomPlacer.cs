using Helpers.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomPlacer : MonoBehaviour {
    [Header("Prefabs/templates")]
    [SerializeField] GameObject _collisionChecker;
    [SerializeField] RoomRestrictionsSO _roomRestrictions;

    [Header("Generation details/beahaviour")]
    [SerializeField] int _roomCount;
    [SerializeField] float _roomSpacing;
    [SerializeField] int _maxDepth = 20;
    [SerializeField] int _desiredDepth = 10;
    [SerializeField] int _maxTries = 100;
    [SerializeField] List<RoomSize> _roomSizes = new(); //probbably use SO

    /// <summary>new room| current room|new room direction</summary>
    public event Action<RoomNode, RoomNode, Vector3> OnRoomCreated;
    public event Action<RoomNode> OnFirstRoomCreated;

    public void Init(GameObject collisionCheck, TemplatesHolderData data, PlacerData placerData) {
        _roomSpacing = placerData.minRoomDistance;
        _maxDepth = placerData.maxDepth;
        _desiredDepth = placerData.desiredDepth;
        _maxTries = placerData.maxTries;
        _roomSizes = placerData.roomSizes;
        _roomCount = placerData.numberOfRooms;

        _collisionChecker = collisionCheck;
        _roomRestrictions = data.RoomRestrictions;
    }

    [ContextMenu("Generate rooms")]
    public void GenerateRooms() => GenerateRooms(_roomCount);
    public void GenerateRooms(int roomsToGenerate) {

        _maxTries = roomsToGenerate * 2;
        _maxDepth = Mathf.Clamp(_maxDepth, _desiredDepth, roomsToGenerate);

        RoomNode firstRoom = InstantiateFirstRoom();
        GenerateRoomRecursively(firstRoom, null, roomsToGenerate - 1);
    }

    RoomNode InstantiateFirstRoom() {
        RoomStats roomStats = RandomiseStats();
        roomStats.SetType(RoomType.Normal);
        RoomNode firstRoom = InstantiateRoom(transform.position, transform, roomStats);
        OnFirstRoomCreated?.Invoke(firstRoom);
        return firstRoom;
    }

    void GenerateRoomRecursively(RoomNode previousRoom, RoomNode currentRoom,
       int remainingRooms, int tries = 0, int depth = 0) {
        if (remainingRooms <= 0 || tries > _maxTries) return;

        if (depth >= _maxDepth) {
            HandleBacktracking(previousRoom, remainingRooms, tries + 1);
            return;
        }

        if (currentRoom == null) {
            currentRoom = previousRoom;
        }

        var roomStats = RandomiseStats();

        Vector3 currentPosition = currentRoom.transform.position;
        Vector3 previousPosition = previousRoom.transform.position;

        int newRoomWorldSize = RoomGenerator.GetRoomWorldSize(roomStats);

        float prevRoomRadius = RoomGenerator.GetRoomWorldSize(currentRoom.Data);
        float roomToRoomDistance = newRoomWorldSize + prevRoomRadius;
        float minDistanceToNextRoom = roomToRoomDistance + _roomSpacing;

        Vector3 prevRoomDirection = (previousPosition - currentPosition).normalized;

        List<Vector3> avaiablePositions = RoomHelpers.GetAvaiablePositions(currentRoom, prevRoomDirection, newRoomWorldSize, minDistanceToNextRoom, _roomSpacing / 2);

        bool noAvaiablePositionsFound = avaiablePositions.Count == 0;
        if (noAvaiablePositionsFound) {
            HandleBacktracking(previousRoom, remainingRooms, tries);
            return;
        }

        Vector3 newRoomPosition = CollectionHelpers.RandomElement(avaiablePositions);
        Vector3 currentRoomDirection = (currentPosition - newRoomPosition).normalized;


        RoomStats currentStats = currentRoom.Data;
        if (!RoomHelpers.AreConnectable(currentStats, roomStats, currentRoomDirection)) {
            HandleUnconnectableRooms(currentStats, ref roomStats);
        }

        RoomNode newRoom = InstantiateRoom(newRoomPosition, transform, roomStats);
        OnRoomCreated?.Invoke(newRoom, currentRoom, currentRoomDirection * -1);

        GenerateRoomRecursively(currentRoom, newRoom, remainingRooms - 1, tries + 1, depth + 1);
    }

    RoomStats RandomiseStats() {
        return RoomHelpers.RandomizeStats(_roomSizes, _roomRestrictions);
    }

    static void HandleUnconnectableRooms(RoomStats currentRoom, ref RoomStats newRoom) {
        newRoom.sides = currentRoom.sides;
    }

    void HandleBacktracking(RoomNode prevRoom, int remainingRooms, int tries) {

        if (prevRoom.TryGetPrevNode(out Node prevPrevNode)) {
            RoomNode backtrackedNode = prevPrevNode as RoomNode;
            int depth = backtrackedNode.Depth;
            GenerateRoomRecursively(backtrackedNode, prevRoom, remainingRooms, tries + 1, depth);
            Debug.DrawLine(backtrackedNode.transform.position, prevRoom.transform.position, Color.red, 10f);
            return;
        }

        Debug.LogError("No Valid place for the room found terminating");

    }

    RoomNode InstantiateRoom(Vector3 position, Transform transform, RoomStats data) {
        float size = RoomGenerator.GetRoomWorldSize(data);

        GameObject obj = GenerateFakeRoom(position, transform);
        obj.name = $"Room {data.Size} {data.sides}gon";
        obj.GetComponent<BoxCollider>().size = new(size, 1, size);

        RoomNode node = obj.GetComponent<RoomNode>();
        node.Position = position;
        node.Data = data;
        return node;
    }

    GameObject GenerateFakeRoom(Vector3 position, Transform transform) {
        return Instantiate(_collisionChecker, position, Quaternion.identity, transform);
    }
}
public struct InitData {
    public GameObject roomPrefab;
    public RoomRestrictionsSO roomRestrictions;
}

