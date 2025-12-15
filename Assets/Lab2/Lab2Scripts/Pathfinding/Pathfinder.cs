using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pathfinder : MonoBehaviour
{
    public GridManager gridManager;
    public AgentMover agentMover;

    [SerializeField] private Material pathMaterial;

    private Node currentNode;

    /*public void FindNodesFromGrid()
    {
        if (gridManager.StartNode == null || gridManager.GoalNode == null)
        {
            Debug.LogWarning("Start or Goal node not set");
            return;
        }

        FindPath(gridManager.StartNode, gridManager.GoalNode);
    }*/

    public List<Node> FindPath(Node startNode, Node goalNode)
    {
        for(int x = 0; x < gridManager.Width; x++)
        {
            for(int y = 0; y < gridManager.Height; y++)
            {
                Node n = gridManager.GetNode(x, y);
                n.gCost = float.PositiveInfinity;
                n.hCost = 0f;
                n.parent = null;
            }
        }
 

        List<Node> openSet = new List<Node>();
        List<Node> closedSet = new List<Node>();

        startNode.gCost = 0f;
        startNode.hCost = HeuristicCost(startNode, goalNode);

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            currentNode = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost)
                {
                    currentNode = openSet[i];
                }
            }

            if (currentNode == goalNode)
            {
                return ReconstructPath(startNode, goalNode);
            }
            else
            {
                openSet.Remove(currentNode);
                closedSet.Add(currentNode);
            }

            foreach(Node neighbour in gridManager.GetNeighbours(currentNode))
            {
                if(neighbour == null) continue;
                if(!neighbour.walkable) continue;
                if(closedSet.Contains(neighbour)) continue;

                float tentativeG = currentNode.gCost + 1f;

                if (tentativeG < neighbour.gCost)
                {
                    neighbour.parent = currentNode;
                    neighbour.gCost = tentativeG;
                    neighbour.hCost = HeuristicCost(neighbour, goalNode);
                }
                if (!openSet.Contains(neighbour))
                {
                    openSet.Add(neighbour);
                }
            }
        }
        return null;

    }
    float HeuristicCost(Node a, Node b)
    {
        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);
        return dx + dy;
    }

    public List<Node> ReconstructPath(Node startNode, Node goalNode)
    {
        List<Node> path = new List<Node>();

        currentNode = goalNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Add(startNode);
        path.Reverse();

        foreach (Node node in path)
        {
            gridManager.SetTileMaterial(node, pathMaterial);
        }

        return path;
    }

    void Update()
    {
        gridManager.StartNode = gridManager.GetNodeFromWorldPosition(agentMover.transform.position);

        if (!Keyboard.current.spaceKey.wasPressedThisFrame)
            return;

        if (gridManager.GoalNode == null)
        {
            Debug.LogWarning("Goal node not set");
            return;
        }
        
        List<Node> path = FindPath(gridManager.StartNode, gridManager.GoalNode);

        if (path == null || path.Count == 0)
        {
            Debug.LogWarning("No path found");
            return;
        }

        agentMover.SetStartNode(gridManager.StartNode);
        agentMover.FollowPath(path);
        
    }
}
