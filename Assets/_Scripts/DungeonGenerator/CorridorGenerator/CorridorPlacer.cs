using System;
using System.Collections.Generic;
using UnityEngine;

public class CorridorPlacer : MonoBehaviour {

    [SerializeField] LevelPrefabs _levelPrefabs;
    [SerializeField] PlacerData _placerData;
    [SerializeField] GameObject _corridorTemplatePrefab;

    /// <summary>
    /// create corridor| corridor data
    /// </summary>
    public event Action<CorridorGenerator, CorridorData> OnCorridorCreated;

    public void Init(LevelPrefabs prefabs, PlacerData placerData, GameObject corridorTemplate) {
        _levelPrefabs = prefabs;
        _placerData = placerData;
        _corridorTemplatePrefab = corridorTemplate;
    }

    public void PlaceCorridors(List<RoomNode> generatedNodes) {
        foreach (var roomNode in generatedNodes) {
            if (!roomNode.TryGetPrevConnection(out var prevLink)) {
                continue;
            }

            RoomNode prevRoomNode = prevLink.room as RoomNode;

            float distance = prevLink.distance;
            Vector3 direction = -prevLink.direction;

            Vector3 doorPosition = LinkManager.GetEdgePosition(prevRoomNode, direction);
            Vector3 corridorPosition = doorPosition + (direction * (distance / 2));

            CorridorData corridorData = CreateCorridorData(distance, direction);

            CorridorGenerator corridor = PlaceCorridor(_corridorTemplatePrefab, corridorPosition);
            OnCorridorCreated?.Invoke(corridor, corridorData);
        }
    }

    CorridorData CreateCorridorData(float distance, Vector3 direction) {
        return new() {
            prefabs = _levelPrefabs,
            direction = direction,
            size = new(_placerData.minCorridorWidth, 1, distance),
            segments = new(1, 1),
        };
    }

    CorridorGenerator PlaceCorridor(GameObject corridorTemplate, Vector3 corridorPosition) {
        CorridorGenerator corridor = Instantiate(corridorTemplate, corridorPosition, Quaternion.identity, transform).GetComponent<CorridorGenerator>();
        return corridor;
    }
}
