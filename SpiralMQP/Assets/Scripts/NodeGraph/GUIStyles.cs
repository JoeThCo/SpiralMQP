using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// A class that holds all GUI styles for all room node types
/// </summary>
public class GUIStyles
{
    // Entrance 
    public GUIStyle entranceNodeStyle = new();
    public GUIStyle entranceNodeSelectedStyle = new();

    // Any non-boss room
    public GUIStyle roomNodeStyle = new();
    public GUIStyle roomNodeSelectedStyle = new();

    // Boss room
    public GUIStyle bossRoomNodeStyle = new();
    public GUIStyle bossRoomNodeSelectedStyle = new();

    // Corridor
    public GUIStyle corridorNodeStyle = new();
    public GUIStyle corridorNodeSelectedStyle = new();
    public readonly float corridorNodeWidth = 110f;
    public readonly float corridorNodeHeight = 65f;

    // Node layout values
    private const int NodePadding = 20; // Spacing inside the GUI element
    private const int NodeBorder = 12; // Spacing outside the GUI element

    public void Initialize()
    {
        SetupEntranceNodeStyle();
        SetupRoomNodeStyle();
        SetupBossRoomNodeStyle();
        SetupCorridorNodeStyle();

        void SetupEntranceNodeStyle()
        {
            entranceNodeStyle = new GUIStyle();
            #if UNITY_EDITOR 
            entranceNodeStyle.normal.background = EditorGUIUtility.Load("node3") as Texture2D;
            #endif
            entranceNodeStyle.normal.textColor = Color.white;
            entranceNodeStyle.padding = new RectOffset(NodePadding, NodePadding, NodePadding, NodePadding);
            entranceNodeStyle.border = new RectOffset(NodeBorder, NodeBorder, NodeBorder, NodeBorder);

            entranceNodeSelectedStyle = new GUIStyle();
            #if UNITY_EDITOR 
            entranceNodeSelectedStyle.normal.background = EditorGUIUtility.Load("node3 on") as Texture2D;
            #endif
            entranceNodeSelectedStyle.normal.textColor = Color.white;
            entranceNodeSelectedStyle.padding = entranceNodeStyle.padding;
            entranceNodeSelectedStyle.border = entranceNodeStyle.border;
        }

        void SetupRoomNodeStyle()
        {
            roomNodeStyle = new GUIStyle();
            #if UNITY_EDITOR 
            roomNodeStyle.normal.background = EditorGUIUtility.Load("node5") as Texture2D;
            #endif
            roomNodeStyle.normal.textColor = Color.white;
            roomNodeStyle.padding = new RectOffset(NodePadding, NodePadding, NodePadding, NodePadding);
            roomNodeStyle.border = new RectOffset(NodeBorder, NodeBorder, NodeBorder, NodeBorder);

            roomNodeSelectedStyle = new GUIStyle();
            #if UNITY_EDITOR 
            roomNodeSelectedStyle.normal.background = EditorGUIUtility.Load("node5 on") as Texture2D;
            #endif
            roomNodeSelectedStyle.normal.textColor = Color.white;
            roomNodeSelectedStyle.padding = roomNodeStyle.padding;
            roomNodeSelectedStyle.border = roomNodeStyle.border;
        }

        void SetupBossRoomNodeStyle()
        {
            bossRoomNodeStyle = new GUIStyle();
            #if UNITY_EDITOR 
            bossRoomNodeStyle.normal.background = EditorGUIUtility.Load("node6") as Texture2D;
            #endif
            bossRoomNodeStyle.normal.textColor = Color.black;
            bossRoomNodeStyle.padding = new RectOffset(NodePadding, NodePadding, NodePadding, NodePadding);
            bossRoomNodeStyle.border = new RectOffset(NodeBorder, NodeBorder, NodeBorder, NodeBorder);

            bossRoomNodeSelectedStyle = new GUIStyle();
            #if UNITY_EDITOR 
            bossRoomNodeSelectedStyle.normal.background = EditorGUIUtility.Load("node6 on") as Texture2D;
            #endif
            bossRoomNodeSelectedStyle.normal.textColor = Color.black;
            bossRoomNodeSelectedStyle.padding = bossRoomNodeStyle.padding;
            bossRoomNodeSelectedStyle.border = bossRoomNodeStyle.border;
        }

        void SetupCorridorNodeStyle()
        {
            corridorNodeStyle = new GUIStyle();
            #if UNITY_EDITOR 
            corridorNodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            #endif
            corridorNodeStyle.normal.textColor = Color.white;
            corridorNodeStyle.padding = new RectOffset(NodePadding, NodePadding, NodePadding, NodePadding);
            corridorNodeStyle.border = new RectOffset(NodeBorder, NodeBorder, NodeBorder, NodeBorder);

            corridorNodeSelectedStyle = new GUIStyle();
            #if UNITY_EDITOR 
            corridorNodeSelectedStyle.normal.background = EditorGUIUtility.Load("node0 on") as Texture2D;
            #endif
            corridorNodeSelectedStyle.normal.textColor = Color.white;
            corridorNodeSelectedStyle.padding = corridorNodeStyle.padding;
            corridorNodeSelectedStyle.border = corridorNodeStyle.border;
        }

    }
}


