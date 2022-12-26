using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// A scriptable object that stores any room node data
/// Create asset menu is not necessary since all the creation will be done in the editor
/// </summary>
public class RoomNodeSO : ScriptableObject
{
    [HideInInspector] public string id; // A GUID system generated ID
    [HideInInspector] public List<string> parentRoomNodeIDList = new List<string>(); // List of all node parents
    [HideInInspector] public List<string> childRoomNodeIDList = new List<string>(); // List of all node children
    [HideInInspector] public RoomNodeGraphSO roomNodeGraph;
    public RoomNodeTypeSO roomNodeType;
    [HideInInspector] public RoomNodeTypeListSO roomNodeTypeList;

    #region Editor Code

    // Only runs in the Unity Editor
#if UNITY_EDITOR

    [HideInInspector] public Rect rect;
    [HideInInspector] public bool isLeftClickDragging = false;
    [HideInInspector] public bool isSelected = false;

    public void Initialize(Rect rect, RoomNodeGraphSO nodeGraph, RoomNodeTypeSO roomNodeType)
    {
        this.rect = rect;
        this.id = Guid.NewGuid().ToString();
        this.name = "RoomNode";
        this.roomNodeGraph = nodeGraph;
        this.roomNodeType = roomNodeType;

        // Load room node type list
        roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
    }

    public void Draw(GUIStyle nodeStyle)
    {
        // Draw Node Box Using Begin Area
        GUILayout.BeginArea(rect, nodeStyle);

        // Start Region To detect popup selection changes
        EditorGUI.BeginChangeCheck();

        // Display a popup using the RoomNodeType name values that can be selected from (default to the currently set roomNodeType)
        int selected = roomNodeTypeList.list.FindIndex(x => x == roomNodeType);

        int selection = EditorGUILayout.Popup("", selected, GetRoomNodeTypesToDisplay());

        roomNodeType = roomNodeTypeList.list[selection];

        if (EditorGUI.EndChangeCheck()) EditorUtility.SetDirty(this); // save the changes we do in the popup

        GUILayout.EndArea();

    }

    /// <summary>
    /// Return a string array with the room node types to display that can be selected
    /// </summary>
    private string[] GetRoomNodeTypesToDisplay()
    {
        string[] roomArray = new string[roomNodeTypeList.list.Count];

        for (int i = 0; i < roomNodeTypeList.list.Count; i++)
        {
            if (roomNodeTypeList.list[i].displayInNodeGraphEditor)
            {
                roomArray[i] = roomNodeTypeList.list[i].roomNodeTypeName;
            }
        }

        return roomArray;
    }

    /// <summary>
    /// Process events for the node
    /// </summary>
    public void ProcessEvents(Event currentEvent)
    {
        switch (currentEvent.type)
        {
            // Process Mouse Down Event
            case EventType.MouseDown:
                ProcessMouseDownEvent(currentEvent);
                break;
            
            // Process Mouse Up Event
            case EventType.MouseUp:
                ProcessMouseUpEvent(currentEvent);
                break;
            
            // Process Mouse Drag Event
            case EventType.MouseDrag:
                ProcessMouseDragEvent(currentEvent);
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Process mouse drag event
    /// </summary>
    private void ProcessMouseDragEvent(Event currentEvent)
    {
        // Left click drag event
        if (currentEvent.button == 0)
        {
            ProcessLeftMouseDragEvent(currentEvent);
        }
    }

    /// <summary>
    /// Process left mouse drag event
    /// </summary>
    private void ProcessLeftMouseDragEvent(Event currentEvent)
    {
        isLeftClickDragging = true;

        DragNode(currentEvent.delta); // well, this delta variable captures the relative movement of the mouse compared to the last event (just like the "difference")
        GUI.changed = true;
    }

    /// <summary>
    /// Drag the Node
    /// </summary>
    private void DragNode(Vector2 delta)
    {
        rect.position += delta; // Calculate and store the new position
        EditorUtility.SetDirty(this);
    }

    /// <summary>
    /// Process mouse up event
    /// </summary>
    private void ProcessMouseUpEvent(Event currentEvent)
    {
        // Left click up
        if (currentEvent.button == 0)
        {
            ProcessLeftClickUpEvent();
        }
    }

    /// <summary>
    /// Process left click up event
    /// </summary>
    private void ProcessLeftClickUpEvent()
    {
        if (isLeftClickDragging)
        {
            isLeftClickDragging = false;
        }
    }

    /// <summary>
    /// Process mouse down events
    /// </summary>
    private void ProcessMouseDownEvent(Event currentEvent)
    {
        // Left click down
        if (currentEvent.button == 0)
        {
            ProcessLeftClickDownEvent();
        }
        // Right click down
        if (currentEvent.button == 1)
        {
            ProcessRightClickDownEvent(currentEvent);
        }
    }

    /// <summary>
    /// Process right click down event
    /// </summary>
    private void ProcessRightClickDownEvent(Event currentEvent)
    {
        roomNodeGraph.SetNodeToDrawConnectionLineFrom(this, currentEvent.mousePosition);
    }

    /// <summary>
    /// Process left click down event
    /// </summary>
    private void ProcessLeftClickDownEvent()
    {
        Selection.activeObject = this; // When left click the node in Editor, the actual object will also be selected in the inspector

        // Toggle node selection
        isSelected = !isSelected;
    }

    /// <summary>
    /// Add childID to the node (returns true if the node has been added, false otherwise)
    /// </summary>
    public bool AddChildRoomNodeIDToRoomNode(string childID)
    {
        childRoomNodeIDList.Add(childID);
        return true;
    }

    /// <summary>
    /// Add parentID to the node (returns true if the node has been added, false otherwise)
    /// </summary>
    public bool AddParentRoomNodeIDToRoomNode(string parentID)
    {
        parentRoomNodeIDList.Add(parentID);
        return true;
    }

#endif

    #endregion

}
