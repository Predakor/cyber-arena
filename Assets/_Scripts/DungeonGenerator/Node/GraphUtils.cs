using System.Collections.Generic;

public static class GraphUtils {


    public static void BreadthFirstSearch(Node node) {

        Queue<Node> queue = new();
        queue.Enqueue(node);
        while (queue.Count > 0) {
            if (node.TryGetNextNodes(out List<Node> nextNodes)) {
                foreach (var nextNode in nextNodes) {
                    queue.Enqueue(nextNode);
                }
            }
            queue.Dequeue();
        }

    }

    public static List<Node> GetDeadEnds(Node initialNode) {
        List<Node> deadEnds = new();
        Queue<Node> queue = new();
        queue.Enqueue(initialNode);

        while (queue.Count > 0) {
            Node currentNode = queue.Dequeue();

            if (currentNode.TryGetNextNodes(out List<Node> nextNodes)) {
                foreach (var nextNode in nextNodes) {
                    queue.Enqueue(nextNode);
                }
            }
            else {
                deadEnds.Add(currentNode);
            }
        }
        return deadEnds;
    }
}
