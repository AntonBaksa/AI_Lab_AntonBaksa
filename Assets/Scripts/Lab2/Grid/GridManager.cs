using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;


public class Node
{
    public int x;
    public int y;

    public bool walkable;
    public GameObject tile;

    public float gCost;
    public float hCost;
    public Node paret;

    public float fCost => gCost + hCost;

    public Node(int x, int y, bool walkable, GameObject tile)
    {
        this.x = x;
        this.y = y;
        this.walkable = walkable;
        this.tile = tile;
        /*this.gCost = gCost;
        this.hCost = hCost;
        this.paret = paret;*/
    }
}

public class GridManager : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public float cellSize = 1f;

    public GameObject tilePrefab;
    public Material walkableMaterial;
    public Material wallMaterial;

    private Node[,] nodes;

    private Dictionary<GameObject, Node> tileToNode = new Dictionary<GameObject, Node>();

    private void Awake()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        nodes = new Node[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 worldPos = new Vector3(x * cellSize, 0f, y * cellSize);
                GameObject tileGO = Instantiate(tilePrefab, worldPos, Quaternion.identity, this.transform);
                tileGO.name = $"Tile_{x}_{y}";

                Node node = new Node(x, y, true, tileGO);
                nodes[x, y] = node;
                tileToNode[tileGO] = node;

                SetTileMaterial(node, walkableMaterial);
            }
        }
    }

    public Node GetNode(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            return null;
        }
        return nodes[x, y];
    }

    public void SetWalkable(Node node, bool walkable)
    {
        node.walkable = walkable;
        SetTileMaterial(node, walkable ? walkableMaterial : wallMaterial);
    }

    private void SetTileMaterial(Node node , Material mat)
    {
        var render = node.tile.GetComponent<MeshRenderer>();
        if(render != null)
        {
            render.material = mat;
        }
    }

    public IEnumerator<Node> GetNeighbours(Node node, bool allowDiagonals = false)
    {
        int x = node.x; int y = node.y;

        yield return GetNode(x + 1, y);
        yield return GetNode(x - 1, y);
        yield return GetNode(x, y + 1);
        yield return GetNode(x, y - 1);

        if (allowDiagonals)
        {
            yield return GetNode(x + 1, y + 1);
            yield return GetNode(x - 1, y + 1);
            yield return GetNode(x + 1, y - 1);
            yield return GetNode(x - 1, y - 1);
        }

    }

    public Node GetNodeFromWorldPosition(Vector3 worldPos)
    {
        int x = Mathf.RoundToInt(worldPos.x / cellSize);
        int y = Mathf.RoundToInt(worldPos.y / cellSize);
        return GetNode(x, y);
    }

    public Node GetNodeFromTile(GameObject tileGO) 
    {
        if (tileToNode.TryGetValue(tileGO, out Node node))
        {
            return node;
        }
        return null;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
