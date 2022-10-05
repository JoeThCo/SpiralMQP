using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonGeneration : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] bool isPlaying = true;
    [Range(10, 35)] [SerializeField] int dungeonMin;
    [Range(10, 35)] [SerializeField] int dungeonMax;
    [Header("Prefabs")]
    [SerializeField] GameObject floorTile;
    [SerializeField] GameObject wallTile;
    //[SerializeField] GameObject doorTile; after first

    float tileSize = 1;

    private float halfFloorSizeX;
    private float halfFloorSizeY;

    private float halfTileSize;

    //todo
    //make room of variable size --done
    //spawn tiles in correct spots --done
    //set the player to certain cords
    //spawn a door in

    void Start()
    {
        int xAxisSize = getRoomAxis() + 1;
        int yAxisSize = getRoomAxis() + 1;

        halfFloorSizeX = (float)(xAxisSize * .5f);
        halfFloorSizeY = (float)(yAxisSize * .5f);

        halfTileSize = (float)(tileSize * .5f);

        makeRoom(xAxisSize, yAxisSize);
        Debug.LogFormat("Room: xAxisSize {0} yAxisSize {1}", xAxisSize, yAxisSize);
    }

    public bool getIsPlaying() { return isPlaying; }

    public void setIsPlaying(bool state) { isPlaying = state; }

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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Game");
        }
    }
}
