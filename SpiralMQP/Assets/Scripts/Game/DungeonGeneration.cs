using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonGeneration : MonoBehaviour
{
    [Header("Values")]
    [Range(10, 35)] [SerializeField] private int dungeonMin;
    [Range(10, 35)] [SerializeField] private int dungeonMax;
    [Header("Prefabs")]
    [SerializeField] private GameObject floorTile;
    [SerializeField] private GameObject wallTile;

    private float tileSize = 1;

    private float halfFloorSizeX;
    private float halfFloorSizeY;

    private float halfTileSize;

    void Start()
    {
        int xAxisSize = getRoomAxis() + 1;
        int yAxisSize = getRoomAxis() + 1;

        halfFloorSizeX = (float)(xAxisSize * .5f);
        halfFloorSizeY = (float)(yAxisSize * .5f);

        halfTileSize = (float)(tileSize * .5f);

        makeRoom(xAxisSize, yAxisSize);
        Debug.LogFormat("Room: ({0},{1})", xAxisSize, yAxisSize);
    }

    void initValues()
    {

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

    void spawnTileInWorld(int x, int y, int xAxisSize, int yAxisSize, GameObject tileType)
    {
        float spawnX = (x + halfTileSize) - halfFloorSizeX;
        float spawnY = (y + halfTileSize) - halfFloorSizeY;

        GameObject tile = Instantiate(tileType, new Vector2(spawnX, spawnY), Quaternion.identity, transform);
        tile.transform.localScale = Vector2.one * tileSize;
    }

    void spawnWhatTile(int x, int y, int xAxisSize, int yAxisSize)
    {
        if (x == 0 || y == 0 || x == xAxisSize - 1 || y == yAxisSize - 1)
        {
            spawnTileInWorld(x, y, xAxisSize, yAxisSize, wallTile);
        }
        else
        {
            spawnTileInWorld(x, y, xAxisSize, yAxisSize, floorTile);
        }
    }

    int getRoomAxis()
    {
        return Random.Range(dungeonMin, dungeonMax);
    }
}
