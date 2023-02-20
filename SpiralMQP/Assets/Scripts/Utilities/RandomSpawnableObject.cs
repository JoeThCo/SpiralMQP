using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a helper class used for returning a random spawnable object from the spawnableObjectRatioList in SpawnableObjectsByLevel<T> for current level
/// </summary>
/// <typeparam name="T">Any spawnable object type</typeparam>
public class RandomSpawnableObject<T>
{
    // This is a struct to help arrange all the spawnable objects in a way that they can be randomly selected in the list according to each of their ratio value
    private struct chanceBoundaries
    {
        public T spawnableObject;
        public int lowBoundaryValue;
        public int highBoundaryValue;
    }

    private int ratioValueTotal = 0; // The total object values in the chanceBoundariesList
    private List<chanceBoundaries> chanceBoundariesList = new List<chanceBoundaries>();
    private List<SpawnableObjectsByLevel<T>> spawnableObjectsByLevelList; // A list that holds spawnable objects infomation for all levels

    /// <summary>
    /// Constructor
    /// </summary>
    public RandomSpawnableObject(List<SpawnableObjectsByLevel<T>> spawnableObjectsByLevelList)
    {
        this.spawnableObjectsByLevelList = spawnableObjectsByLevelList;
    }


    /// <summary>
    /// Return a random spawnable item from the current level
    /// </summary>
    public T GetItem()
    {
        // Preparation for item selection
        int upperBoundary = -1;
        ratioValueTotal = 0;
        chanceBoundariesList.Clear();
        T spawnableObject = default(T); // A default method to set up the default value for a generic type

        foreach (SpawnableObjectsByLevel<T> spawnableObjectsByLevel in spawnableObjectsByLevelList)
        {
            // Check for current level
            if (spawnableObjectsByLevel.dungeonLevel == GameManager.Instance.GetCurrentDungeonLevel())
            {
                foreach (SpawnableObjectRatio<T> spawnableObjectRatio in spawnableObjectsByLevel.spawnableObjectRatioList)
                {
                    int lowerBoundary = upperBoundary + 1; // The current lower boundary is always equal to the previous upper boundary plus 1

                    // The current upper boundary is always equal to the current lower boundary plus this spawnable object's ratio value minus 1 
                    // (Since the list starts from index 0, the minus one is necessary)
                    upperBoundary = lowerBoundary + spawnableObjectRatio.ratio - 1;

                    ratioValueTotal += spawnableObjectRatio.ratio; // Accumulate to the total count

                    // Add spawnable object to list;
                    chanceBoundariesList.Add(new chanceBoundaries() { spawnableObject = spawnableObjectRatio.dungeonObject, lowBoundaryValue = lowerBoundary, highBoundaryValue = upperBoundary });
                }
            }
        }

        // If list is empty, we simply return a generic default null
        if (chanceBoundariesList.Count == 0) return default(T);

        // Here is where all the randomness come from, by randomly selecting a value from the total ratio value
        int lookUpValue = Random.Range(0, ratioValueTotal);

        // Loop through list to get seleted random spawnable object details
        foreach (chanceBoundaries spawnChance in chanceBoundariesList)
        {
            if (lookUpValue >= spawnChance.lowBoundaryValue && lookUpValue <= spawnChance.highBoundaryValue)
            {
                spawnableObject = spawnChance.spawnableObject;
                break;
            }
        }

        return spawnableObject;
    }

}
