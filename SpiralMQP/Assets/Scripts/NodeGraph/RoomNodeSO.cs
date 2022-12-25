using System.Collections;
using System.Collections.Generic;
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
    

}
