using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Callbacks;
using UnityEditor;
using System;

/// <summary>
/// Check out Unity Script Reference if you are confused about any of the functions or classes
/// </summary>
public class RoomNodeGraphEditor : EditorWindow
{
    // Define a customized GUI style
    private GUIStyle roomNodeStyle;

    private static RoomNodeGraphSO currentRoomNodeGraph;
    private RoomNodeTypeListSO roomNodeTypeList;
    private RoomNodeSO currentRoomNode = null; // Current selected room node SO

    // Node layout values
    private const float nodeWidth = 160f;
    private const float nodeHeight = 75f;
    private const int nodePadding = 25;
    private const int nodeBorder = 12;

    // Connecting line values
    private const float connectingLineWidth = 3f;
    private const float connectingLineArrowSize = 6f;

    // Add a menu item named "Room Node Graph Editor" 
    // And define a PATH that we can call the menuitem 
    [MenuItem("Room Node Graph Editor", menuItem = "Window/Dungeon Editor/Room Node Graph Editor")] // FYI: Not necessary, but it's cool, thus we keep it

    //  Only static functions can use the MenuItem attribute.
    private static void OpenWindow()
    {
        // A built in function in "EditorWindow" class
        // Returns the first EditorWindow of type t which is currently on the screen.
        // "Room Node Graph Editor" is the title of the window
        GetWindow<RoomNodeGraphEditor>("Room Node Graph Editor");
    }

    private void OnEnable()
    {
        // Define node layout style
        roomNodeStyle = new GUIStyle();
        roomNodeStyle.normal.background = EditorGUIUtility.Load("node5") as Texture2D;
        roomNodeStyle.normal.textColor = Color.white;
        roomNodeStyle.padding = new RectOffset(nodePadding, nodePadding, nodePadding, nodePadding);
        roomNodeStyle.border = new RectOffset(nodeBorder, nodeBorder, nodeBorder, nodeBorder);

        // Load Room Node Types
        roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
    }


    /// <summary>
    /// Open the room node graph editor window if a room node graph scriptable object asset is double clicked in the inspector
    /// </summary>
    [OnOpenAsset(0)] // Need the namespace UnityEditor.Callbacks
    public static bool OnDoubleClickAsset(int instanceID, int line)
    {
        RoomNodeGraphSO roomNodeGraph = EditorUtility.InstanceIDToObject(instanceID) as RoomNodeGraphSO;

        if (roomNodeGraph != null)
        {
            OpenWindow();
            currentRoomNodeGraph = roomNodeGraph;
            return true;
        }
        return false;
    }


    /// <summary>
    /// Draw Editor GUI
    /// </summary>
    private void OnGUI()
    {
        // If a scriptable object of type RoomNodeGraphSO has been selected then process
        // IMPORTANT: The order of function execution matters
        if (currentRoomNodeGraph != null)
        {
            // Draw line if being dragged
            DrawDraggedLine();

            // Process Events - one event at a time
            ProcessEvents(Event.current);

            // Draw Connections Between Room Nodes
            DrawRoomConnections();

            // Draw Room Nodes
            DrawRoomNodes();
        }

        if (GUI.changed) Repaint();
    }


    /// <summary>
    /// Draw connections in the graph window between room nodes
    /// </summary>
    private void DrawRoomConnections()
    {
        // Loop through all room nodes and draw line between each parent and child pair for all nodes
        foreach (RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList)
        {
            if (roomNode.childRoomNodeIDList.Count > 0) // Check if the child list is empty
            {
                // Loop through child room nodes and draw line between each parent and child pair
                foreach (string childRoomNodeID in roomNode.childRoomNodeIDList)
                {
                    // Get child room node from dictionary
                    if (currentRoomNodeGraph.roomNodeDictionary.ContainsKey(childRoomNodeID))
                    {
                        DrawConnectionLine(roomNode, currentRoomNodeGraph.roomNodeDictionary[childRoomNodeID]); // Draw line between the parent node and child node

                        GUI.changed = true;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Draw connection line between the parent room node and child room node
    /// </summary>
    private void DrawConnectionLine(RoomNodeSO parentRoomNode, RoomNodeSO childRoomNode)
    {
        // Get line start and end position
        Vector2 startPosition = parentRoomNode.rect.center;
        Vector2 endPosition = childRoomNode.rect.center;

        // Calculate mid point
        Vector2 midPosition = (endPosition + startPosition) / 2f;

        // Vector from start to end position of line
        Vector2 direction = endPosition - startPosition;

        // Calculate normalized perpendicular positions from the mid point
        Vector2 arrowTailPoint1 = midPosition - new Vector2(-direction.y, direction.x).normalized * connectingLineArrowSize;
        Vector2 arrowTailPoint2 = midPosition + new Vector2(-direction.y, direction.x).normalized * connectingLineArrowSize;

        // Calculate mid point offset position for arrow head
        Vector2 arrowHeadPoint = midPosition + direction.normalized * connectingLineArrowSize;

        // Draw Arrow
        Handles.DrawBezier(arrowHeadPoint, arrowTailPoint1, arrowHeadPoint, arrowTailPoint1, Color.yellow, null, connectingLineWidth);
        Handles.DrawBezier(arrowHeadPoint, arrowTailPoint2, arrowHeadPoint, arrowTailPoint2, Color.yellow, null, connectingLineWidth);

        // Draw line
        Handles.DrawBezier(startPosition, endPosition, startPosition, endPosition, Color.yellow, null, connectingLineWidth);

        GUI.changed = true;
    }

    /// <summary>
    /// Draw line from node to line position
    /// </summary>
    private void DrawDraggedLine()
    {
        if (currentRoomNodeGraph.linePosition != Vector2.zero) // If the endpoint has been updated
        {
            // Draw line from node to line position
            // This fucntion draws texured bezier line through start and end points with the given tangents
            Handles.DrawBezier(currentRoomNodeGraph.roomNodeToDrawLineFrom.rect.center, currentRoomNodeGraph.linePosition, 
                currentRoomNodeGraph.roomNodeToDrawLineFrom.rect.center, currentRoomNodeGraph.linePosition, Color.yellow, null, connectingLineWidth);
        }
    }

    /// <summary>
    /// Draw room nodes in the graph window
    /// </summary>
    private void DrawRoomNodes()
    {
        // Loop through all room nodes and draw them
        foreach (RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList)
        {
          roomNode.Draw(roomNodeStyle);
        }

        GUI.changed = true; // Mark the change to true
    }

    private void ProcessEvents(Event currentEvent)
    {
        // Get room node that mouse is over if it's null or not curerntly being dragged
        if (currentRoomNode == null || currentRoomNode.isLeftClickDragging == false)
        {
            currentRoomNode = IsMouseOverRoomNode(currentEvent);
        }

        // If mouse isn't over a room node or we are currently dragging a line from the room node then process graph events
        if (currentRoomNode == null || currentRoomNodeGraph.roomNodeToDrawLineFrom != null)
        {
            ProcessRoomNodeGraphEvents(currentEvent);
        }

        // Else process room node events
        else
        {
            // Process room node events
            currentRoomNode.ProcessEvents(currentEvent);
        }

    }

    /// <summary>
    /// Check to see the mouse is over a room node - if so then return the room node else return null
    /// </summary>
    private RoomNodeSO IsMouseOverRoomNode(Event currentEvent)
    {
        for (int i = currentRoomNodeGraph.roomNodeList.Count - 1; i >= 0; i--)
        {
            if (currentRoomNodeGraph.roomNodeList[i].rect.Contains(currentEvent.mousePosition)) // Check to see the mouse is over a room node
            {
                return currentRoomNodeGraph.roomNodeList[i];
            }
        }

        return null;
    }

    /// <summary>
    /// Process Room Node Graph Events
    /// </summary>
    private void ProcessRoomNodeGraphEvents(Event currentEvent)
    {
        switch (currentEvent.type)
        {
            case EventType.MouseDown:
                ProcessMouseDownEvent(currentEvent);
                break;
            
            case EventType.MouseUp:
                ProcessMouseUpEvent(currentEvent);
                break;
            
            case EventType.MouseDrag:
                ProcessMouseDragEvent(currentEvent);
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Process mouse up events
    /// </summary>
    private void ProcessMouseUpEvent(Event currentEvent)
    {
        // if releasing the right mouse button and currently dragging a line
        if (currentEvent.button == 1 && currentRoomNodeGraph.roomNodeToDrawLineFrom != null)
        {
            // Check if over a room node
            RoomNodeSO roomNode = IsMouseOverRoomNode(currentEvent);

            if (roomNode != null)
            {
                // Set it as a child of the parent room node if it can be added
                if (currentRoomNodeGraph.roomNodeToDrawLineFrom.AddChildRoomNodeIDToRoomNode(roomNode.id))
                {
                    // Set parent ID in child room node
                    roomNode.AddParentRoomNodeIDToRoomNode(currentRoomNodeGraph.roomNodeToDrawLineFrom.id);
                }
            }

            ClearLineDrag();
        }
    }

    /// <summary>
    /// Clear line drag from a room node
    /// </summary>
    private void ClearLineDrag()
    {
        currentRoomNodeGraph.roomNodeToDrawLineFrom = null; // This is the same as telling the Room Node Graph Editor: " Hey! I am not dragging anymore! "
        currentRoomNodeGraph.linePosition = Vector2.zero;
        GUI.changed = true;
    }

    /// <summary>
    /// Process mouse drag event
    /// </summary>
    private void ProcessMouseDragEvent(Event currentEvent)
    {
        // Process right click drag event - draw line
        if (currentEvent.button == 1)
        {
            ProcessRightMouseDragEvent(currentEvent);
        }
    }

    /// <summary>
    /// Process right click drag event - draw line
    /// </summary>
    private void ProcessRightMouseDragEvent(Event currentEvent)
    {
        if (currentRoomNodeGraph.roomNodeToDrawLineFrom != null)
        {
            DragConnectingLine(currentEvent.delta);
            GUI.changed = true;
        }
    }

    /// <summary>
    /// Drag connecting line from room node
    /// </summary>
    private void DragConnectingLine(Vector2 delta)
    {
        currentRoomNodeGraph.linePosition += delta; // Keep updating the new end point
    }

    /// <summary>
    /// Process mouse down events on the room node graph (not over a node)
    /// </summary>
    private void ProcessMouseDownEvent(Event currentEvent)
    {
        // Process right click mouse down on graph event (show context menu) 
        if (currentEvent.button == 1) // 0: left mouse click  1: right mouse click
        {
            ShowContextMenu(currentEvent.mousePosition);
        }
    }

     /// <summary>
     /// Show the context menu
     /// </summary>
    private void ShowContextMenu(Vector2 mousePosition)
    {
        GenericMenu menu = new GenericMenu(); // GenericMenu lets you create custom context menus and dropdown menus
        menu.AddItem(new GUIContent("Create Room Node"), false, CreateRoomNode, mousePosition);
        menu.ShowAsContext();
    }

     /// <summary>
     /// Create a room node at the mouse position
     /// </summary>
    private void CreateRoomNode(object mousePositionObject)
    {
        CreateRoomNode(mousePositionObject, roomNodeTypeList.list.Find(x => x.isNone)); // Using predicate and we are only interested in the unassigned room node
    }

     /// <summary>
     /// Create a room node at the mouse position - overloaded to also pass in RoomNodeTypeSO
     /// </summary>
    private void CreateRoomNode(object mousePositionObject, RoomNodeTypeSO roomNodeType)
    {
        Vector2 mousePosition = (Vector2)mousePositionObject;

        // Create room node SO asset
        RoomNodeSO roomNode = ScriptableObject.CreateInstance<RoomNodeSO>();

        // Add room node to current room node graph room node list
        currentRoomNodeGraph.roomNodeList.Add(roomNode);

        // Set room node values
        roomNode.Initialize(new Rect(mousePosition, new Vector2(nodeWidth, nodeHeight)), currentRoomNodeGraph, roomNodeType);

        // Add room node to room node graph SO asset database
        AssetDatabase.AddObjectToAsset(roomNode, currentRoomNodeGraph);
        AssetDatabase.SaveAssets();

        // Refresh graph node dictionary
        currentRoomNodeGraph.OnValidate();
    }
}
