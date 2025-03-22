using System.Collections.Generic;
using UnityEngine;

public abstract class Node : MonoBehaviour {
    private RoomLink<Node> prevNode = null;
    private List<RoomLink<Node>> nextNodes = new();

    public void AddNextNode(Node node) {
        nextNodes.Add(new RoomLink<Node>(node));
    }

    public void AddNextLink(RoomNode node, float distance, Vector3 direction) {
        var existingLink = nextNodes.Find((listNode) => listNode.room == node);
        if (existingLink != null) {
            existingLink.direction = direction;
            existingLink.distance = distance;
            return;
        }

        nextNodes.Add(new(node, distance, direction));
    }

    public void SetNextNodes(List<RoomLink<Node>> nodes) {
        nextNodes = nodes;
    }

    public void SetPrevNode(Node node) {
        prevNode = new RoomLink<Node>(node);
    }
    public void SetPrevLink(RoomNode node, float distance, Vector3 direction) {
        prevNode = new(node, distance, direction);
    }

    public bool TryGetNextNodes(out List<Node> foundNodes) {
        if (nextNodes.Count > 0) {
            foundNodes = new List<Node>(nextNodes.Count);
            foreach (var link in nextNodes) {
                foundNodes.Add(link.room);
            }
            return true;
        }
        foundNodes = new List<Node>(0);
        return false;
    }

    public bool TryGetNextConnections(out List<RoomLink<Node>> nextNodes) {
        if (this.nextNodes.Count > 0) {
            nextNodes = this.nextNodes;
            return true;
        }
        nextNodes = new List<RoomLink<Node>>(0);
        return false;
    }

    public bool TryGetPrevNode(out Node prevNode) {
        if (this.prevNode != null) {
            prevNode = this.prevNode.room;
            return true;
        }
        prevNode = null;
        return false;
    }

    public bool TryGetPrevConnection(out RoomLink<Node> prevNode) {
        if (this.prevNode != null) {
            prevNode = this.prevNode;
            return true;
        }
        prevNode = null;
        return false;
    }

    public static List<Node> GetNeighbours(Node node) {
        var list = new List<Node>(0);

        if (node.TryGetNextNodes(out List<Node> nodes)) {
            list.AddRange(nodes);
        }

        if (node.TryGetPrevNode(out Node prevNode)) {
            list.Add(prevNode);
        }

        return list;
    }

    public List<Node> GetNeighbours() {
        return GetNeighbours(this);
    }

    public List<RoomLink<Node>> GetConnections() {
        var list = new List<RoomLink<Node>>();

        if (TryGetNextConnections(out var nodes)) {
            list.AddRange(nodes);
        }

        if (TryGetPrevConnection(out var prevNode)) {
            list.Add(prevNode);
        }

        return list;
    }

    public List<Vector3> GetConnectedDirections() {
        var list = new List<Vector3>();

        if (TryGetNextConnections(out var nodes)) {
            foreach (var node in nodes) {
                list.Add(node.direction);
            }
        }

        if (TryGetPrevConnection(out var prevNode)) {
            list.Add(prevNode.direction);
        }

        return list;
    }
}
