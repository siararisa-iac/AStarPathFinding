using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathFinding : MonoBehaviour
{
    private GameObject startCube, endCube;
    public Node startNode { get; set; }
    public Node endNode { get; set; }
    public List<Node> path;

    private float elapsedTime = 0;
    private float intervalTime = 1.0f;

    private void Start()
    {
        startCube = GameObject.FindGameObjectWithTag("Start");
        endCube = GameObject.FindGameObjectWithTag("End");
        path = new List<Node>();
        FindPath();
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if(elapsedTime >= intervalTime)
        {
            elapsedTime = 0;
            FindPath();
        }
    }

    private void FindPath()
    {
        //get the nodes based on the position of the gameobjects in the world
        startNode = new Node(GridManager.Instance.GetGridCellCenter(
            GridManager.Instance.GetGridIndex(startCube.transform.position)));
        endNode = new Node(GridManager.Instance.GetGridCellCenter(
            GridManager.Instance.GetGridIndex(endCube.transform.position)));
        path = AStar.FindPath(startNode, endNode);
    }

    //Visualize the path found
    private void OnDrawGizmos()
    {
        if (path == null)
            return;
        if(path.Count > 0)
        {
            int index = 1;
            foreach(Node node in path)
            {
                if(index < path.Count)
                {
                    Node nextNode = path[index];
                    Debug.DrawLine(node.position, nextNode.position, Color.red);
                    index++;
                }
            }
        }
    }
}
