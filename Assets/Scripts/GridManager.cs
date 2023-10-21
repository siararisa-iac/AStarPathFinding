using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    //Singleton implementation
    private static GridManager _Instance = null;
    public static GridManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                //Check if ther is existing GridManager
                _Instance = FindObjectOfType(typeof(GridManager)) as GridManager;
                //If no existing GridManager
                if (_Instance == null)
                    Debug.LogError("You do not have a GridManager.");
            }
            return _Instance;       
        }
    }

    public int numOfRows;
    public int numOfColumns;
    public float gridCellSize;
    public bool showGrid = true;
    public bool showObstacleBlocks = true;
    private Vector3 origin;
    private GameObject[] obstacles;
    public Node[,] nodes;
    
    public Vector3 Origin
    {
        get { return origin; }
    }

    private void Awake()
    {
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        CalculateObstacles();
    }

    //Set the position of and mark all the obstacles in the grid
    private void CalculateObstacles()
    {
        //Set up the nodes 2D array
        nodes = new Node[numOfColumns, numOfRows];
        int index = 0;

        //Calculate the position of each node and assign them in the 2d array
        for(int i=0; i < numOfColumns; i++)
        {
            for(int j=0; j < numOfRows; j++)
            {
                Vector3 cellPos = GetGridCellCenter(index);
                Node node = new Node(cellPos);
                nodes[i, j] = node;
                index++;
            }
        }

        //mark the obstacles in the map
        if (obstacles != null && obstacles.Length > 0)
        {
            foreach(GameObject data in obstacles)
            {
                int indexCell = GetGridIndex(data.transform.position);
                int col = GetColumn(indexCell);
                int row = GetRow(indexCell);
                nodes[row, col].MarkAsObstacle();
            }
        }
    }

    //Return the position of the grid cell in world coordinates
    public Vector3 GetGridCellCenter(int index)
    {
        Vector3 cellPosition = GetGridCellPosition(index);
        cellPosition.x += (gridCellSize / 2.0f);
        cellPosition.z += (gridCellSize / 2.0f);
        return cellPosition;
    }

    public Vector3 GetGridCellPosition(int index)
    {
        int row = GetRow(index);
        int col = GetColumn(index);
        float xPosInGrid = col * gridCellSize;
        float zPosInGrid = row * gridCellSize;
        return Origin + new Vector3(xPosInGrid, 0.0f, zPosInGrid);
    }

    //Returns the grid cell index in the grid based on the given position
    public int GetGridIndex(Vector3 position)
    {
        if (!IsInBounds(position))
            return -1;
        position -= Origin;
        int col = (int)(position.x / gridCellSize);
        int row = (int)(position.z / gridCellSize);
        return (row * numOfColumns + col);
    }

    public bool IsInBounds(Vector3 position)
    {
        float width = numOfColumns * gridCellSize;
        float height = numOfRows * gridCellSize;
        return (position.x >= Origin.x && position.x <= Origin.x + width &&
            position.x <= Origin.z + height && position.z >= Origin.z);
    }

    public int GetColumn(int index)
    {
        return index % numOfColumns;
    }

    public int GetRow(int index)
    {
        return index / numOfRows;
    }


    //Retrieve the neighboring nodes of a particular node
    public void GetNeighbors(Node node, ref List<Node> neighbors)
    {
        Vector3 neighborPos = node.position;
        int neighborIndex = GetGridIndex(neighborPos);
        int row = GetRow(neighborIndex);
        int col = GetColumn(neighborIndex);

        neighbors = new List<Node>();
        //Bottom
        int leftNodeRow = row - 1;
        int leftNodeColumn = col;
        AssignNeighbor(leftNodeRow, leftNodeColumn, ref neighbors);
        //Top
        leftNodeRow = row + 1;
        leftNodeColumn = col;
        AssignNeighbor(leftNodeRow, leftNodeColumn, ref neighbors);
        //Left 
        leftNodeRow = row;
        leftNodeColumn = col + 1;
        AssignNeighbor(leftNodeRow, leftNodeColumn, ref neighbors);
        //Right
        leftNodeRow = row;
        leftNodeColumn = col - 1;
        AssignNeighbor(leftNodeRow, leftNodeColumn, ref neighbors);
    }

    //Check whether the node is an obstacle. If not, we add the neighbor node to the neighborsList
    public void AssignNeighbor(int row, int column, ref List<Node> neighbors)
    {
        if(row != -1 && column != -1 && row < numOfRows && column < numOfColumns)
        {
            Node nodeToAdd = nodes[row, column];
            if (!nodeToAdd.isObstacle)
                neighbors.Add(nodeToAdd);
        }
    }

    //For drawing the Grid
    private void OnDrawGizmos()
    {
        if (showGrid)
        {
            DebugDrawGrid(transform.position, numOfRows, numOfColumns, gridCellSize, Color.blue);
        }
        if (showObstacleBlocks)
        {
            Vector3 cellsize = new Vector3(gridCellSize, 1.0f, gridCellSize);
            if(obstacles != null && obstacles.Length > 0)
            {
                foreach (GameObject data in obstacles)
                    Gizmos.DrawCube(GetGridCellCenter(GetGridIndex(data.transform.position)), cellsize);
            }
        }
    }

    void DebugDrawGrid(Vector3 origin, int numRows, int numCols, float cellSize, Color color)
    {
        float width = (numCols * cellSize);
        float height = numOfRows * cellSize;
        for(int i = 0; i < numRows + 1; i++)
        {
            Vector3 startPos = origin + i * cellSize * Vector3.forward;
            Vector3 endPos = startPos + width * Vector3.right;
            Debug.DrawLine(startPos, endPos, color);
        }
        for (int i = 0; i < numCols + 1; i++)
        {
            Vector3 startPos = origin + i * cellSize * Vector3.right;
            Vector3 endPos = startPos + width * Vector3.forward;
            Debug.DrawLine(startPos, endPos, color);
        }
    }
}
