using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Generation/Data/Placemenet Data", fileName = "Placement Settings")]
public class PlacerData : ScriptableObject {

    [Header("Room variables")]
    [Tooltip("Total number of rooms that can be generated on this floor")]
    public int numberOfRooms = 12;
    public List<RoomSize> roomSizes;

    [Header("Layout variables")]
    public int maxDepth = 12;
    public int desiredDepth = 12;
    public int maxTries = 24;

    [Header("Corridor variables")]
    [Range(1, 20)] public float minRoomDistance = 20;
    [Range(1, 8)] public int minConnections = 1;
    [Range(1, 10)] public int minCorridorWidth = 8;

    public void OnValidate() {
        maxDepth = Math.Clamp(maxDepth, 0, numberOfRooms);
        maxTries = Math.Max(maxTries, numberOfRooms * 2);
    }
}

