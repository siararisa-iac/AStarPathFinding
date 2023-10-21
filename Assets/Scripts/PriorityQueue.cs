using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue
{
    private List<Node> nodes;

    //Constructor
    public PriorityQueue()
    {
        nodes = new List<Node>();
    }

    //helper functions/properties
    //returns the size of the list
    public int Count
    {
        get { return nodes.Count; }
    }
    //check if list contains a node
    public bool Contains(Node node)
    {
        return nodes.Contains(node);
    }
    //returns the first node element in the list
    public Node First()
    {
        if(nodes.Count > 0)
            return nodes[0];
        return null;
    }
    //add a node in the list
    public void Push(Node node)
    {
        //add node
        nodes.Add(node);
        //sort the list
        nodes.Sort();
    }
    //remove a node in the list
    public void Remove(Node node)
    {
        nodes.Remove(node);
        nodes.Sort();
    }
}
