using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AgentMover : MonoBehaviour
{
    public Pathfinder Pathfinder;
    public GridManager gridManager;
    public float moveSpeed = 5f;

    private List<Node> currentPath;
    private int currentIndex = 0;

    public void FollowPath(List<Node> path)
    {
        currentPath = path;
        currentIndex = 0;
    }

    public void SetStartNode(Node startNode)
    {
        transform.position = NodeToWorldPosition(startNode);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPath == null || currentPath.Count == 0) return;

        if (currentIndex >= currentPath.Count)
        {
            currentPath = null;
            return;
        }

        Node targetNode = currentPath[currentIndex];
        Vector3 targetPos = NodeToWorldPosition(targetNode);

        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.05f) 
        {
            currentIndex++;
        }
    }

    private Vector3 NodeToWorldPosition(Node node)
    {
        return new Vector3(node.x * gridManager.cellSize, transform.position.y, node.y * gridManager.cellSize);
    }

}
