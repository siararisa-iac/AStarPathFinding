using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    public static PriorityQueue closedList, openList;

    private static float HeuristicEstimateCost(Node curNode, Node goalNode)
    {
        //find the directional vector towards the goalNode
        Vector3 vecCost = curNode.position - goalNode.position;
        //the magnitude of the directional vector gives the distance from the current node to the goal node
        return vecCost.magnitude;
    }

    public static List<Node> FindPath(Node start, Node goal)
    {
        //Initialize the open and closed list
        openList = new PriorityQueue();
        closedList = new PriorityQueue();

        //openlist will start with the start node
        openList.Push(start);
        start.nodeTotalCost = 0.0f;
        start.estimatedCost = HeuristicEstimateCost(start, goal);

        Node currentNode = null;

        while(openList.Count != 0)
        {
            //pick the first node in the openlist and keep it as the current node
            //take note that the openlist is always sorted everytime a node is being added
            //therefor, the first node is always the node with the least cost
            currentNode = openList.First();

            //Check if the current node is the goal node
            if(currentNode.position == goal.position)
            {
                //exit the loop and calculate and build the path
                return CalculatePath(currentNode);
            }

            //Create a list that will store the neighboring nodes
            List<Node> neighbors = new List<Node>();
            GridManager.Instance.GetNeighbors(currentNode, ref neighbors);


            //Check each node in the neighbors
            for (int i = 0; i < neighbors.Count; i++)
            {
                Node neighborNode = neighbors[i];
                //Check if the neighborNode is already in the closedlist
                if (!closedList.Contains(neighborNode))
                {
                    //calculate the cost value
                    float cost = HeuristicEstimateCost(currentNode, neighborNode);
                    float totalCost = currentNode.nodeTotalCost + cost;
                    float neighborEstCost = HeuristicEstimateCost(neighborNode, goal);
                    //update the node properties with the new cost
                    neighborNode.nodeTotalCost = totalCost;
                    neighborNode.parent = currentNode;
                    neighborNode.estimatedCost = totalCost + neighborEstCost;

                    //put the neighbor node in the openlist
                    if (!openList.Contains(neighborNode))
                        openList.Push(neighborNode);
                }
            }

            //Push the current node to the closed list
            closedList.Push(currentNode);
            //Remove it from the open list
            openList.Remove(currentNode);
        }

        if(currentNode.position != goal.position)
        {
            Debug.LogError("goal not found");
            return null;
        }

        return CalculatePath(currentNode);
    }

    //Trace through each node's parent node and build the list
    private static List<Node> CalculatePath(Node node)
    {
        List<Node> list = new List<Node>();
        while(node != null)
        {
            list.Add(node);
            node = node.parent;
        }
        list.Reverse();
        return list;
    }
}
