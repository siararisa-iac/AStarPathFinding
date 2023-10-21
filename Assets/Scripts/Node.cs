using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IComparable
{
    public float nodeTotalCost;
    public float estimatedCost;
    public bool isObstacle;
    public Node parent;
    public Vector3 position;

    //Default Constructor
    public Node()
    {
        nodeTotalCost = 0.0f;
        estimatedCost = 1.0f;
        isObstacle = false;
        parent = null;
    }

    //Constructor with position
    public Node(Vector3 position)
    {
        nodeTotalCost = 0.0f;
        estimatedCost = 1.0f;
        isObstacle = false;
        parent = null;
        this.position = position;
    }

    public int CompareTo(object obj)
    {
        //cast the object as a Node
        Node node = (Node)obj;
        //negative value means the object comes first before this in the sorting order
        if (this.estimatedCost < node.estimatedCost)
            return -1;
        //positive value means the object will come after this in the sorting order
        if (this.estimatedCost > node.estimatedCost)
            return 1;
        return 0;
    }

    public void MarkAsObstacle()
    {
        isObstacle = true;
    }
}
