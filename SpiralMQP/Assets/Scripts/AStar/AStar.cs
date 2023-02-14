using System.Collections.Generic;
using UnityEngine;

public static class AStar // Make this static so that it's easier to call whenever I need to use AStar pathfinding
{
    /// <summary>
    /// Builds a path for the room, from the startGridPosition to the endGridPosition, and adds movement steps to the returned Stack. 
    /// Returns null if no path is found.
    /// </summary>
    public static Stack<Vector3> BuildPath(Room room, Vector3Int startGridPosition, Vector3Int endGridPosition)
    {
        // Adjust positions by lower bounds
        startGridPosition -= (Vector3Int)room.templateLowerBounds; ;
        endGridPosition -= (Vector3Int)room.templateLowerBounds;

        // Create open list and closed hashset
        List<Node> openNodeList = new List<Node>(); // We want to sort the list of nodes to easily retrieve the node with the lowest FCost
        HashSet<Node> closedNodeHashSet = new HashSet<Node>(); // We don't need to sort the closed nodes, but we need to check if a node is already in the list, the Contains() method can do this fast.

        // Create grid nodes for path finding
        GridNodes gridNodes = new GridNodes(room.templateUpperBounds.x - room.templateLowerBounds.x + 1, room.templateUpperBounds.y - room.templateLowerBounds.y + 1); // Adding one because we are counting grids

        // Getting start node and end node from the grid map
        Node startNode = gridNodes.GetGridNode(startGridPosition.x, startGridPosition.y);
        Node targetNode = gridNodes.GetGridNode(endGridPosition.x, endGridPosition.y);

        // This is where we actually run the astar algo
        Node endPathNode = FindShortestPath(startNode, targetNode, gridNodes, openNodeList, closedNodeHashSet, room.instantiatedRoom);

        if (endPathNode != null)
        {
            return CreatePathStack(endPathNode, room);
        }

        return null;
    }

    /// <summary>
    /// Find the shortest path - returns the end Node if a path has been found, else returns null.
    /// </summary>
    private static Node FindShortestPath(Node startNode, Node targetNode, GridNodes gridNodes, List<Node> openNodeList, HashSet<Node> closedNodeHashSet, InstantiatedRoom instantiatedRoom)
    {
        // Add start node to open list
        openNodeList.Add(startNode);

        // Loop through open node list until empty
        while (openNodeList.Count > 0)
        {
            // Sort List
            openNodeList.Sort(); // This is where the IComparable comes in handy

            // Current node = the node in the open list with the lowest FCost
            Node currentNode = openNodeList[0];
            openNodeList.RemoveAt(0);

            // If the current node = target node then finish
            if (currentNode == targetNode)
            {
                return currentNode;
            }

            // Add current node to the closed list
            closedNodeHashSet.Add(currentNode);

            // Evaluate Fcost for each neighbor of the current node
            EvaluateCurrentNodeNeighbors(currentNode, targetNode, gridNodes, openNodeList, closedNodeHashSet, instantiatedRoom);
        }

        return null; // Fail to find a path
    }


    /// <summary>
    /// Create a Stack<Vector3> containing the movement path 
    /// </summary>
    private static Stack<Vector3> CreatePathStack(Node targetNode, Room room)
    {
        Stack<Vector3> movementPathStack = new Stack<Vector3>();

        Node nextNode = targetNode;

        // Get mid point of cell
        Vector3 cellMidPoint = room.instantiatedRoom.grid.cellSize * 0.5f;
        cellMidPoint.z = 0f;

        while (nextNode != null)
        {
            // Convert grid position to world position
            Vector3 worldPosition = room.instantiatedRoom.grid.CellToWorld(new Vector3Int(nextNode.gridPosition.x + room.templateLowerBounds.x, nextNode.gridPosition.y + room.templateLowerBounds.y, 0));

            // Set the world position to the middle of the grid cell
            worldPosition += cellMidPoint;

            movementPathStack.Push(worldPosition);

            nextNode = nextNode.parentNode;
        }

        return movementPathStack;
    }

    /// <summary>
    /// Evaluate neighbor nodes
    /// </summary>
    private static void EvaluateCurrentNodeNeighbors(Node currentNode, Node targetNode, GridNodes gridNodes, List<Node> openNodeList, HashSet<Node> closedNodeHashSet, InstantiatedRoom instantiatedRoom)
    {
        Vector2Int currentNodeGridPosition = currentNode.gridPosition;

        Node validNeighborNode;

        // Loop through all directions - a 3x3 area
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                // If this is the current node, skip
                if (i == 0 && j == 0) continue;

                // Get neighbor 
                validNeighborNode = GetValidNodeNeighbor(currentNodeGridPosition.x + i, currentNodeGridPosition.y + j, gridNodes, closedNodeHashSet, instantiatedRoom);

                if (validNeighborNode != null)
                {
                    // Calculate new gcost for neighbor
                    int newCostToNeighbor;

                    // Get the movement penalty
                    // Unwalkable paths have a value of 0. Default movement penalty is set in Settings and applies to other grid squares.
                    int movementPenaltyForGridSpace = instantiatedRoom.aStarMovementPenalty[validNeighborNode.gridPosition.x, validNeighborNode.gridPosition.y];

                    // Update the gcost
                    newCostToNeighbor = currentNode.gCost + GetDistance(currentNode, validNeighborNode) + movementPenaltyForGridSpace;

                    bool isValidNeighborNodeInOpenList = openNodeList.Contains(validNeighborNode);

                    if (newCostToNeighbor < validNeighborNode.gCost || !isValidNeighborNodeInOpenList)
                    {
                        // Update all costs and set the parent node of the neighbor node to the current node
                        validNeighborNode.gCost = newCostToNeighbor;
                        validNeighborNode.hCost = GetDistance(validNeighborNode, targetNode);
                        validNeighborNode.parentNode = currentNode;

                        // If the neighbor node is not in the open list, then add it
                        if (!isValidNeighborNodeInOpenList)
                        {
                            openNodeList.Add(validNeighborNode);
                        }
                    }
                }
            }
        }
    }


    /// <summary>
    /// Returns the distance int between nodeA and nodeB
    /// </summary>
    private static int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridPosition.x - nodeB.gridPosition.x);
        int dstY = Mathf.Abs(nodeA.gridPosition.y - nodeB.gridPosition.y);

        // The shorter one must be the diagonal passing node - try to draw it if you don't trust me
        if (dstX > dstY) return 14 * dstY + 10 * (dstX - dstY);  // 10 used instead of 1, and 14 is a pythagoras approximation SQRT(10*10 + 10*10) - to avoid using floats
            
        return 14 * dstX + 10 * (dstY - dstX);
    }

    /// <summary>
    /// Evaluate a neighbor node at neighborNodeXPosition, neighborNodeYPosition, using the specified gridNodes, closedNodeHashSet, and instantiated room.
    /// Returns null if the node isn't valid
    /// </summary>
    private static Node GetValidNodeNeighbor(int neighborNodeXPosition, int neighborNodeYPosition, GridNodes gridNodes, HashSet<Node> closedNodeHashSet, InstantiatedRoom instantiatedRoom)
    {
        // If neighbor node position is beyond grid then return null
        if (neighborNodeXPosition >= instantiatedRoom.room.templateUpperBounds.x - instantiatedRoom.room.templateLowerBounds.x 
            || neighborNodeXPosition < 0 
            || neighborNodeYPosition >= instantiatedRoom.room.templateUpperBounds.y - instantiatedRoom.room.templateLowerBounds.y 
            || neighborNodeYPosition < 0)
        {
            return null;
        }

        // Get neighbor node
        Node neighborNode = gridNodes.GetGridNode(neighborNodeXPosition, neighborNodeYPosition);

        // Check for obstacle at that position
        int movementPenaltyForGridSpace = instantiatedRoom.aStarMovementPenalty[neighborNodeXPosition, neighborNodeYPosition];

        // Check for moveable obstacle at that position
        int itemObstacleForGridSpace = instantiatedRoom.aStarItemObstacles[neighborNodeXPosition, neighborNodeYPosition];

        // If neighbor is an obstacle or neighbor is in the closed list then skip
        if (movementPenaltyForGridSpace == 0 || itemObstacleForGridSpace == 0 || closedNodeHashSet.Contains(neighborNode))
        {
            return null;
        }
        else
        {
            return neighborNode;
        }

    }
}