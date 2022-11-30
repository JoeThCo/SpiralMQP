using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DungeonTileSO")]
public class TilePaletteSO : ScriptableObject
{
    public Sprite middleTile;
    [Space(10)]
    public Sprite xWallTile;
    public Sprite yWallTile;
    [Space(10)]
    public Sprite cornerTile;
}
