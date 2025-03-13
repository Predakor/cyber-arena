using System.Collections.Generic;
using UnityEngine;

public class CorridorPlacer : MonoBehaviour {

    [SerializeField] RoomData _roomDataTemplate;
    [SerializeField] RoomConnectionSettings _roomConnectionSettings;

    [SerializeField] List<CorridorGenerator> _generatedCorridors;

    public void LoadData(RoomData roomData, RoomConnectionSettings connectionSettings) {
        _roomDataTemplate = roomData;
        _roomConnectionSettings = connectionSettings;
    }

    public List<CorridorGenerator> PlaceCorridors(
        List<RoomGenerator> generatedRooms, GameObject corridorTemplate) {
        _generatedCorridors.Clear();

        foreach (var room in generatedRooms) {
            var prevRoom = room.RoomLinks.GetPrevRoom();
            if (!prevRoom.Value.room) {
                continue;
            }

            float distance = prevRoom.Value.distance;
            Vector3 direction = prevRoom.Value.direction;
            Vector3 doorPosition = LinkManager.GetEdgePosition(room, direction);
            Vector3 corridorPosition = doorPosition + (direction * (distance / 2));

            CorridorData corridorData = new() {
                prefabs = _roomDataTemplate.prefabs,
                direction = direction,
                size = new(_roomConnectionSettings.minCorridorWidth, 1, distance),
                segments = new(1, 1),
            };

            PlaceCorridor(corridorTemplate, corridorPosition, corridorData);
        }

        return _generatedCorridors;
    }

    private void PlaceCorridor(GameObject corridorTemplate, Vector3 corridorPosition, CorridorData corridorData) {
        CorridorGenerator corridor = Instantiate(corridorTemplate, corridorPosition, Quaternion.identity, transform).GetComponent<CorridorGenerator>();
        corridor.LoadData(corridorData);
        _generatedCorridors.Add(corridor);
    }
}
