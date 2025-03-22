using UnityEngine;

static public class LinkManager {
    public static Vector3 GetEdgePosition(RoomNode roomNode, Vector3 direction) {
        return roomNode.Position + (direction * roomNode.Data.GetRoomRadius());
    }

    public static void LinkNodes(RoomNode currentNode, RoomNode newNode) {
        currentNode.AddNextNode(newNode);
        newNode.SetPrevNode(currentNode);
    }

    public static void LinkRoomsAndNodes(RoomNode currentNode, RoomNode newNode, Vector3 direction) {
        Vector3 newRoomDoor = GetEdgePosition(currentNode, direction);
        Vector3 prevRoomDoor = GetEdgePosition(newNode, -direction);
        float doorDistance = (prevRoomDoor - newRoomDoor).magnitude;

        Debug.DrawLine(newRoomDoor, prevRoomDoor, Color.green, 10f);

        newNode.Depth = currentNode.Depth + 1;
        currentNode.AddNextLink(newNode, doorDistance, direction);
        newNode.SetPrevLink(currentNode, doorDistance, -direction);
    }
}
