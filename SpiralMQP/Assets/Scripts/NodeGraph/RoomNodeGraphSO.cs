using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomNodeGraph", menuName = "Scriptable Objects/Dungeon/Room Node Graph")]
public class RoomNodeGraphSO : ScriptableObject
{
    [HideInInspector] public RoomNodeTypeListSO roomNodeTypeList;
    [HideInInspector] public List<RoomNodeSO> roomNodeList = new List<RoomNodeSO>();
    [HideInInspector] public Dictionary<string, RoomNodeSO> roomNodeDictionary = new Dictionary<string, RoomNodeSO>(); // The key in the dictionary is a string type GUID

    private void Awake() 
    {
        LoadRoomNodeDictionary();
    }

    /// <summary>
    /// Load the room node dictionary from the room node list
    /// </summary>
    private void LoadRoomNodeDictionary()
    {
        roomNodeDictionary.Clear();

        // Populate dictionary
        foreach (RoomNodeSO node in roomNodeList)
        {
            roomNodeDictionary[node.id] = node;
        }
    }

    /// <summary>
    /// Get room node by room node ID
    /// </summary>
    public RoomNodeSO GetRoomNode(string roomNodeID)
    {
        if (roomNodeDictionary.TryGetValue(roomNodeID, out RoomNodeSO roomNode))
        {
            return roomNode;
        }
        return null;
    }



    #region Editor Code

    // Compiler directive: only runs in the unity editor
#if UNITY_EDITOR

    [HideInInspector] public RoomNodeSO roomNodeToDrawLineFrom = null; // Starting point for the line
    [HideInInspector] public Vector2 linePosition; // End point for the line

    // Repopulate node dictionary every time a change is made in the editor
    public void OnValidate() 
    {
        LoadRoomNodeDictionary();
    }

    /// <summary>
    /// Set Line info
    /// </summary>
    public void SetNodeToDrawConnectionLineFrom(RoomNodeSO node, Vector2 position)
    {
        roomNodeToDrawLineFrom = node;
        linePosition = position;
    }

#endif

    #endregion
}
