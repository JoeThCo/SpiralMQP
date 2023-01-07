using UnityEngine;
[System.Serializable]
public class Doorway 
{
    public Vector2Int position; // The doorway position
    public Orientation orientation; // This will be the orientation of the dungeon, which is the orientation enum value
    public GameObject doorPrefab; // This holds a door prefab where it will be closed after player passing it

    #region Header
    [Header("The Upper Left Position To Start Copying From")]
    #endregion
    public Vector2Int doorwayStartCopyPosition;

    #region Header
    [Header("The width of tiles in the doorway to copy over")]
    #endregion
    public int doorwayCopyTileWidth;

    #region Header
    [Header("The height of tiles in the doorway to copy over")]
    #endregion
    public int doorwayCopyTileHeight;

    [HideInInspector]
    public bool isConnected = false; // If this doorway has been connected
    [HideInInspector]
    public bool isUnavailable = false;
}
