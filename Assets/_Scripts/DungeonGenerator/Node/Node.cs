using System.Collections.Generic;
using UnityEngine;

public abstract class Node : MonoBehaviour {
    [SerializeField] Node prevNode = null;
    [SerializeField] List<Node> nextNodes = new();

    public void AddNextNode(Node node) => nextNodes.Add(node);
    public void SetNextNodes(List<Node> nodes) => nextNodes = nodes;
    public void SetPrevNode(Node node) => prevNode = node;

    public virtual bool TryGetNextNodes(out List<Node> nextNodes) {
        if (this.nextNodes.Count > 0) {
            nextNodes = this.nextNodes;
            return true;
        }
        nextNodes = null;
        return false;
    }

    public virtual bool TryGetPrevNode(out Node prevNode) {
        if (this.prevNode != null) {
            prevNode = this.prevNode;
            return true;
        }
        prevNode = null;
        return false;
    }

    public static List<Node> GetNeighbours(Node node) {
        var list = new List<Node>();

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
}
