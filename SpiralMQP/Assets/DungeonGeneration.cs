using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGeneration : MonoBehaviour
{
    [Header("Values")]
    [Range(5, 10)] [SerializeField] int dungeonMin;
    [Range(5, 10)] [SerializeField] int dungeonMax;
    [Range(1, 5)] [SerializeField] float tileSize;
    [Header("Prefabs")]
    [SerializeField] GameObject floorTile;
    [SerializeField] GameObject wallTile;
    //[SerializeField] GameObject doorTile; after first

    //todo
    //make room of variable size --done
    //spawn tiles in correct spots --done
    //set the player to certain cords
    //spawn a door in

    void Start()
    {
        int xAxisSize = getRoomAxis() + 1;
        int yAxisSize = getRoomAxis() + 1;

        makeRoom(xAxisSize, yAxisSize);
        Debug.LogFormat("Room: xAxisSize {0} yAxisSize {1}", xAxisSize, yAxisSize);
    }

    void makeRoom(int xAxisSize, int yAxisSize)
    {
        for (int y = 0; y < yAxisSize; y++)
        {
            for (int x = 0; x < xAxisSize; x++)
            {
                spawnWhatTile(x, y, xAxisSize, yAxisSize);
            }
        }
    }

    void spawnTileInWorld(int x, int y, GameObject tileType)
    {
        GameObject tile = Instantiate(tileType, new Vector2(x, y) * tileSize, Quaternion.identity, transform);
        tile.transform.localScale = Vector2.one * tileSize;
    }

    void spawnWhatTile(int x, int y, int xAxisSize, int yAxisSize)
    {
        if (x == 0 || y == 0 || x == xAxisSize - 1 || y == yAxisSize - 1)
        {
            spawnTileInWorld(x, y, wallTile);
        }
        else
        {
            spawnTileInWorld(x, y, floorTile);
        }
    }

    int getRoomAxis()
    {
        return Random.Range(dungeonMin, dungeonMax);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
