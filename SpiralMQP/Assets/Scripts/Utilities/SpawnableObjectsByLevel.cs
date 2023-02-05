using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnableObjectsByLevel<T>
{
   public DungeonLevelSO dungeonLevel; // The dungeon level which the spawnable objects will be spawned to
   public List<SpawnableObjectRatio<T>> spawnableObjectRatioList; // A list of all spawnable objects
}
