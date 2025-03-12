using Helpers.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RoomHelpers {
    static Dictionary<int, List<Vector3>> _angleCache = new();

    public static List<Vector3> GetRoomDirections(int sides) {
        if (sides == 0) { return null; }

        if (_angleCache.ContainsKey(sides)) return _angleCache[sides];

        List<Vector3> directions = new(sides);
        Vector3 direction = Vector3.zero;

        int rotationBase = 360 / sides;
        for (int i = 0; i < sides; i++) {
            float angle = rotationBase * i;
            float posX = Mathf.Cos(Mathf.Deg2Rad * angle);
            float posZ = Mathf.Sin(Mathf.Deg2Rad * angle);

            direction.Set(posX, 0, posZ);
            directions.Add(direction.normalized);
        }

        _angleCache.Add(sides, directions);
        return directions;
    }

    public static List<Vector3> GetUnoccupiedPositions(List<Vector3> positionsToCheck, float radius) {
        List<Vector3> availablePositions = new();
        foreach (var position in positionsToCheck) {
            if (HasSpaceForRoom(position, radius)) {
                availablePositions.Add(position);
            }
        }
        return availablePositions;
    }

    public static bool HasSpaceForRoom(Vector3 position, float radius) {
        int roomMasks = LayerMask.GetMask("Ground");
        Collider[] detectedRooms = Physics.OverlapSphere(position, radius, roomMasks);
        return detectedRooms.Length == 0;
    }

    public static bool HasSpaceForRoom(Vector3 position, Vector3 boxCordinates) {
        int roomMasks = LayerMask.GetMask("Ground");
        Collider[] detectedRooms = Physics.OverlapBox(position, boxCordinates, Quaternion.identity, roomMasks);
        return detectedRooms.Length == 0;
    }

    public static RoomStats RandomizeStats(RoomStats stats) {
        RoomSize[] defaultSizes = new[] { RoomSize.Small, RoomSize.Medium, RoomSize.Large };
        int[] defaultSides = new[] { 4, 8 };
        return RandomizeStats(stats, defaultSizes, defaultSides);
    }

    public static RoomStats RandomizeStats(RoomStats stats, RoomSize[] roomSizes, int[] roomSides) {
        stats.size = (RoomSize)Random.Range(0, roomSizes.Length);
        stats.sides = roomSides[Random.Range(0, roomSides.Length)];
        return stats;
    }

    public static RoomStats RandomizeStats(RoomStats stats, List<RoomSize> sizes, RoomRestrictionsSO restrictions) {
        RoomStats randomStats = stats;
        randomStats.size = CollectionHelpers.RandomElement(sizes);
        List<int> allowedSides = restrictions.GetAllowedSides(randomStats.size);
        randomStats.sides = CollectionHelpers.RandomElement(allowedSides);
        return randomStats;
    }

    public static bool AreRoomsConnectable(RoomStats currentStats, RoomStats newStats, Vector3 prevRoomDirection) {
        if (currentStats.sides == newStats.sides) {
            return true;
        }
        List<Vector3> newRoomDirections = GetRoomDirections(newStats.sides);
        return newRoomDirections.Exists((direction) => direction == prevRoomDirection);
    }

    public static List<Vector3> GetAvaiablePositions(RoomGenerator currentRoom, Vector3 prevRoomDirection, int roomWorldSize, float minDistanceToNextRoom, float minSpacing) {
        List<Vector3> directions = GetRoomDirections(currentRoom.RoomStats.sides);

        //transform directions to points and fillter backwardDirection out
        List<Vector3> positions = new(directions.Count - 1);
        foreach (var direction in directions) {
            if (direction != prevRoomDirection) {
                positions.Add(direction * minDistanceToNextRoom + currentRoom.transform.position);
            }
        }

        //check which directions we can fit a room
        float sizeToCheck = roomWorldSize + minSpacing / 2;
        List<Vector3> avaiablePositions = GetUnoccupiedPositions(positions, sizeToCheck);
        return avaiablePositions;
    }
}
