using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomNodeType", menuName = "Scriptable Objects/Dungeon/Room Node Type")]
public class RoomNodeTypeSO : ScriptableObject
{
    public string roomNodeTypeName; // Name of the room type, such as: Corridor, Entrance, Boss, Small Room, Large Room and etc.
    public bool displayInNodeGraphEditor = true; // Only flag the RoomNodeTypes that should be visible in the editor
    public bool isCorridor; // One Type should be a corridor
    public bool isCorridorNS; // One Type should be a corridor North-South
    public bool isCorridorEW; //One Type should be a corridor East-West
    public bool isEntrance; // One Type should be an entrance
    public bool isBossRoom; // One Type should be a boss room
    public bool isNone; // One Type should be none (unassigned)

    #region Editor Code

    // Compiler directive: only runs in the unity editor
#if UNITY_EDITOR
    /// <summary>
    /// Editor-only function that Unity calls when the script is loaded or a value changes in the inspector
    /// </summary>
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(roomNodeTypeName), roomNodeTypeName);
    }
#endif

    #endregion
}
