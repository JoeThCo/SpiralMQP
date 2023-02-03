using UnityEngine;

public class GridNodes 
{
    private int width; // Width of the map grid - how many nodes we have in x direction
    private int height; // Height of the map grid - how many nodes we have in y direction

    private Node[,] gridNode; // This is a two dimensional array

    // Constructor
    public GridNodes(int width, int height)
    {
        this.width = width;
        this.height = height;

        gridNode = new Node[width, height]; // Initialize the two dimensional array size

        // Assign position for each node in the grid map
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                gridNode[x, y] = new Node(new Vector2Int(x, y));
            }
        }
    }

    /// <summary>
    /// Retrieve node based on the grid position
    /// </summary>
    public Node GetGridNode(int xPosition, int yPosition)
    {
        if (xPosition < width && yPosition < height)
        {
            return gridNode[xPosition, yPosition];
        }
        else
        {
            Debug.Log("Requested grid node is out of range");
            return null;
        }
    }

}
