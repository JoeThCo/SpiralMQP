using System;
using UnityEngine;

/// <summary>
/// IComparable is an interface in the Unity engine that provides a method to sort objects in a collection. 
/// The method CompareTo is used to determine the order of the objects in a collection. 
/// Classes that implement IComparable must define their own sorting logic by implementing the CompareTo method. 
/// This allows objects to be sorted based on a specific criteria such as the object's name, value, or any other attribute. 
/// IComparable is used to sort arrays and other collections of objects in Unity, and can be useful in situations where a custom sort order is needed.
/// </summary>
public class Node : IComparable<Node>
{
    public Vector2Int gridPosition; // Where is this node in the grid map - This will be used to calculate the distance in the game
    public int gCost = 0; // Distance from current node to starting node
    public int hCost = 0; // Distance from current node to finishing node
    public Node parentNode;

    // Constructor
    public Node(Vector2Int gridPosition)
    {
        this.gridPosition = gridPosition;

        parentNode = null;
    }

    // FCost = GCost + HCost
    public int FCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public int CompareTo(Node nodeToCompare)
    {
        // Compare will be < 0 if this instance Fcost is less than nodeToCompare.FCost
        // Compare will be > 0 if this instance Fcost is greater than nodeToCompare.FCost
        // Compare will be == 0 if the FCost values are the same

        int compare = FCost.CompareTo(nodeToCompare.FCost);

        // Tie breaker
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }

        return compare;
    }
}
