using UnityEngine;

static public class LinkManager {
    public static void LinkRooms(RoomGenerator currentRoom, RoomGenerator newRoom, Vector3 direction) {

        Vector3 newRoomDoor = GetEdgePosition(currentRoom, direction);
        Vector3 prevRoomDoor = GetEdgePosition(newRoom, -direction);
        float doorDistance = (prevRoomDoor - newRoomDoor).magnitude;

        Debug.DrawLine(newRoomDoor, prevRoomDoor, Color.blue, 10f);

        newRoom.RoomLinks.Position = newRoom.transform.position;
        newRoom.RoomLinks.Depth = currentRoom.RoomLinks.Depth + 1;
        newRoom.RoomLinks.SetPrevRoom(currentRoom.RoomLinks, doorDistance, direction * -1);
        currentRoom.RoomLinks.AddNextRoom(newRoom.RoomLinks, doorDistance, direction);
    }
    public static Vector3 GetEdgePosition(RoomGenerator room, Vector3 direction) {
        return room.transform.position + (direction * room.GetRoomRadius());
    }

    public static void LinkNodes(RoomGenerator currentRoom, RoomGenerator newRoom) {
        LinkNodes(currentRoom.RoomNode, newRoom.RoomNode);
    }

    public static void LinkNodes(RoomNode currentNode, RoomNode newNode) {
        currentNode.AddNextNode(newNode);
        newNode.SetPrevNode(currentNode);
    }

    public static void LinkRoomsAndNodes(RoomGenerator currentRoom, RoomGenerator newRoom,
        Vector3 direction) {
        LinkNodes(currentRoom, newRoom);
        LinkRooms(currentRoom, newRoom, direction);
    }
}
