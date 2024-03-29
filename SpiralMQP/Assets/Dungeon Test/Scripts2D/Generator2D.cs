﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using Graphs;
using UnityEngine.SceneManagement;
using System;

public class Generator2D : MonoBehaviour
{
    #region Classes
    enum CellType
    {
        None,
        Room,
        Hallway,
        Wall,
        Start,
        End
    }

    class Room
    {
        public RectInt bounds;

        public Room(Vector2Int location, Vector2Int size)
        {
            bounds = new RectInt(location, size);
        }

        public Vector2Int GetCordInRoom(Random random)
        {
            int x = random.Next(bounds.min.x + 1, bounds.max.x - 1);
            int y = random.Next(bounds.min.y + 1, bounds.max.y - 1);

            return new Vector2Int(x, y);
        }

        public static bool Intersect(Room a, Room b)
        {
            return !((a.bounds.position.x >= (b.bounds.position.x + b.bounds.size.x)) || ((a.bounds.position.x + a.bounds.size.x) <= b.bounds.position.x)
                || (a.bounds.position.y >= (b.bounds.position.y + b.bounds.size.y)) || ((a.bounds.position.y + a.bounds.size.y) <= b.bounds.position.y));
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Room room = (Room)obj;

            return room.bounds.position.x == bounds.position.x && room.bounds.position.y == bounds.position.y;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    class Hallway
    {
        private List<Vector2Int> path;
        private List<Vector2Int> walls;

        public List<Vector2Int> GetPath() { return path; }
        public List<Vector2Int> GetWalls() { return walls; }

        public void AddToWallList(Vector2Int cord)
        {
            if (!walls.Contains(cord))
            {
                walls.Add(cord);
            }
        }

        public Hallway(List<Vector2Int> path)
        {
            this.path = path;
            walls = new List<Vector2Int>();
        }
    }
    #endregion

    [Header("Debug")]
    [SerializeField] bool isRandomSeed;
    [SerializeField] int seed;

    [Header("Dungeon Size")]
    [SerializeField] [Range(1f, 3f)] float tileSize = 1;

    [SerializeField] [Range(50, 250)] int size = 15;
    [SerializeField] [Range(10, 500)] int roomCount = 100;
    [SerializeField] [Range(0f, 1f)] float hallwayChance = 0.125f;

    [Header("Sizes")]
    [SerializeField] [Range(1, 10)] int roomMinSize;
    [SerializeField] [Range(5, 25)] int roomMaxSize;

    [SerializeField] [Range(1, 3)] int hallwaySize;

    [Header("Tile")]
    [SerializeField] GameObject tilePrefab;
    [SerializeField] GameObject wallPrefab;
    [Space(15)]
    [SerializeField] TilePaletteSO tilePallete;
    [SerializeField] Sprite startSprite;
    [SerializeField] Sprite endSprite;
    [Space(15)]
    [SerializeField] Sprite DebugTile;

    Random random;
    Grid2D<CellType> grid;
    Grid2D<CellType> outputGrid;
    List<Room> rooms;
    List<Hallway> hallways;
    Delaunay2D delaunay;
    HashSet<Prim.Edge> selectedEdges;
    List<Prim.Edge> mst;

    void Start()
    {
        Generate();
    }

    public void Clear()
    {
        while (transform.childCount != 0)
        {
            foreach (Transform t in transform)
            {
                DestroyImmediate(t.gameObject);
            }
        }
    }

    public void Generate()
    {
        Clear();

        if (isRandomSeed)
            random = new Random();
        else
            random = new Random(seed);

        grid = new Grid2D<CellType>(size, Vector2Int.zero);
        outputGrid = new Grid2D<CellType>(size, Vector2Int.zero);

        rooms = new List<Room>();
        hallways = new List<Hallway>();

        PlaceRooms();
        Triangulate();

        CreateHallways();
        PathfindHallways();

        SpawnWorldWithClasses();

        PlaceWallCollision();
    }
    void PlaceWallCollision()
    {
        GameObject allWalls = new GameObject();
        allWalls.name = "allWalls";
        allWalls.transform.parent = transform;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Vector2Int position = new Vector2Int(x, y);

                if (outputGrid[position] == CellType.Wall)
                {
                    foreach (Vector2Int cord in GetAdjacentNeighbors(CellType.None, position))
                    {
                        SpawnWall(cord, allWalls);
                    }
                }
            }
        }
    }

    void SpawnWorldWithClasses()
    {
        Room startRoom = rooms[0];
        Room endRoom = rooms[random.Next(0, rooms.Count - 1)];
        float dist = Vector2Int.Distance(startRoom.bounds.position, endRoom.bounds.position);

        while (startRoom.Equals(endRoom) || dist < (float)(size * .5f))
        {
            endRoom = rooms[random.Next(0, rooms.Count - 1)];
            dist = Vector2Int.Distance(startRoom.bounds.position, endRoom.bounds.position);
        }

        foreach (Room room in rooms)
        {
            GameObject roomObj = new GameObject();
            roomObj.name = "Room";
            roomObj.transform.parent = transform;

            //start
            if (room.Equals(startRoom))
            {
                grid[room.GetCordInRoom(random)] = CellType.Start;
            }

            //end
            if (room.Equals(endRoom))
            {
                grid[room.GetCordInRoom(random)] = CellType.End;
            }

            foreach (Vector2Int cord in room.bounds.allPositionsWithin)
            {
                SpawnTile(cord, roomObj);
            }
        }

        foreach (Hallway hallway in hallways)
        {
            GameObject hallwayObj = new GameObject();
            hallwayObj.name = "Hallway";
            hallwayObj.transform.parent = transform;

            foreach (Vector2Int cord in hallway.GetPath())
            {
                SpawnTile(cord, hallwayObj);
            }

            foreach (Vector2Int cord in hallway.GetWalls())
            {
                SpawnTile(cord, hallwayObj);
            }
        }
    }

    void PlaceRooms()
    {
        for (int i = 0; i < roomCount; i++)
        {
            Vector2Int location = new Vector2Int(
                random.Next(0, size),
                random.Next(0, size)
            );

            Vector2Int roomSize = new Vector2Int(
                random.Next(roomMinSize, roomMaxSize + 1),
                random.Next(roomMinSize, roomMaxSize + 1)
            );

            bool add = true;
            Room newRoom = new Room(location, roomSize);
            Room buffer = new Room(location + new Vector2Int(-2, -2), roomSize + new Vector2Int(3, 3));

            foreach (var room in rooms)
            {
                if (Room.Intersect(room, buffer))
                {
                    add = false;
                    break;
                }
            }

            if (newRoom.bounds.xMin < 1 || newRoom.bounds.xMax >= size
                || newRoom.bounds.yMin < 1 || newRoom.bounds.yMax >= size)
            {
                add = false;
            }

            if (add)
            {
                rooms.Add(newRoom);

                foreach (var pos in newRoom.bounds.allPositionsWithin)
                {
                    if (pos.x == newRoom.bounds.min.x || pos.y == newRoom.bounds.min.y || pos.x == newRoom.bounds.max.x - 1 || pos.y == newRoom.bounds.max.y - 1)
                    {
                        grid[pos] = CellType.Wall;
                    }
                    else
                    {
                        grid[pos] = CellType.Room;
                    }
                }
            }
        }
    }

    void Triangulate()
    {
        List<Vertex> vertices = new List<Vertex>();

        foreach (var room in rooms)
        {
            vertices.Add(new Vertex<Room>((Vector2)room.bounds.position + ((Vector2)room.bounds.size) / 2, room));
        }

        delaunay = Delaunay2D.Triangulate(vertices);
    }

    void CreateHallways()
    {
        List<Prim.Edge> edges = new List<Prim.Edge>();

        foreach (var edge in delaunay.Edges)
        {
            edges.Add(new Prim.Edge(edge.U, edge.V));
        }

        mst = Prim.MinimumSpanningTree(edges, edges[0].U);
        selectedEdges = new HashSet<Prim.Edge>(mst);

        var remainingEdges = new HashSet<Prim.Edge>(edges);
        remainingEdges.ExceptWith(selectedEdges);

        foreach (var edge in remainingEdges)
        {
            if (random.NextDouble() < hallwayChance)
            {
                selectedEdges.Add(edge);
            }
        }
    }

    void PathfindHallways()
    {
        DungeonPathfinder2D aStar = new DungeonPathfinder2D(size);

        foreach (var edge in selectedEdges)
        {
            Room startRoom = (edge.U as Vertex<Room>).Item;
            Room endRoom = (edge.V as Vertex<Room>).Item;

            var startPosf = startRoom.bounds.center;
            var endPosf = endRoom.bounds.center;
            var startPos = new Vector2Int((int)startPosf.x, (int)startPosf.y);
            var endPos = new Vector2Int((int)endPosf.x, (int)endPosf.y);

            #region A*
            var path = aStar.FindPath(startPos, endPos, (DungeonPathfinder2D.Node a, DungeonPathfinder2D.Node b) =>
            {
                var pathCost = new DungeonPathfinder2D.PathCost();

                pathCost.cost = Vector2Int.Distance(b.Position, endPos);    //heuristic

                if (grid[b.Position] == CellType.Room)
                {
                    pathCost.cost += 10;
                }
                else if (grid[b.Position] == CellType.None)
                {
                    pathCost.cost += 5;
                }
                else if (grid[b.Position] == CellType.Hallway)
                {
                    pathCost.cost += 1;
                }

                pathCost.traversable = true;

                return pathCost;
            });

            #endregion

            if (path != null)
            {
                Hallway hallway = new Hallway(path);
                hallways.Add(hallway);

                foreach (Vector2Int cord in path)
                {
                    if (grid[cord] != CellType.None)
                    {
                        grid[cord] = CellType.Hallway;
                    }

                    foreach (Vector2Int neighbor in GetCellNeighbors(CellType.None, cord, hallwaySize))
                    {
                        if (isValid(neighbor, hallwaySize))
                        {
                            grid[neighbor] = CellType.Wall;
                            hallway.AddToWallList(neighbor);
                        }
                    }
                }
            }
        }
    }

    void SpawnTile(Vector2Int cords, GameObject parent)
    {
        Vector3 worlds = new Vector3(cords.x, cords.y, 0);

        GameObject go = Instantiate(tilePrefab, worlds * tileSize, Quaternion.identity, parent.transform);
        go.transform.localScale = new Vector3(tileSize, tileSize, 1);
        go.name = cords.ToString();

        SetSprite(go.GetComponentInChildren<SpriteRenderer>(), cords);
    }

    void SpawnWall(Vector2Int cords, GameObject parent)
    {
        Vector3 worlds = new Vector3(cords.x, cords.y, 0);

        GameObject go = Instantiate(wallPrefab, worlds * tileSize, Quaternion.identity, parent.transform);
        go.transform.localScale = new Vector3(tileSize, tileSize, 1);
        go.name = "Wall" + cords.ToString();
    }

    void SetSprite(SpriteRenderer sr, Vector2Int cords)
    {
        CellType currentCell = grid[cords];

        if (currentCell == CellType.Room)
        {
            sr.sprite = tilePallete.middleTile;
            outputGrid[cords] = CellType.Room;
        }
        else if (currentCell == CellType.Hallway)
        {
            sr.sprite = tilePallete.middleTile;
            outputGrid[cords] = CellType.Hallway;
        }
        else if (currentCell == CellType.Wall)
        {
            SetWallSprite(sr, cords);
        }
        else if (currentCell == CellType.Start)
        {
            sr.sprite = startSprite;
            sr.color = Color.green;

            outputGrid[cords] = CellType.Start;
        }
        else if (currentCell == CellType.End)
        {
            sr.sprite = endSprite;
            sr.color = Color.red;

            outputGrid[cords] = CellType.End;
        }
    }

    void SetWallSprite(SpriteRenderer sr, Vector2Int cords)
    {
        CellType above = CellType.None;
        CellType below = CellType.None;
        CellType right = CellType.None;
        CellType left = CellType.None;

        if (isValid(cords + Vector2Int.up))
            above = grid[cords + Vector2Int.up];

        if (isValid(cords + Vector2Int.down))
            below = grid[cords + Vector2Int.down];

        if (isValid(cords + Vector2Int.right))
            right = grid[cords + Vector2Int.right];

        if (isValid(cords + Vector2Int.left))
            left = grid[cords + Vector2Int.left];

        if (above == CellType.Wall && below == CellType.Wall && right != CellType.Wall && left != CellType.Wall)
        {
            sr.sprite = tilePallete.xWallTile;
            sr.flipX = right == CellType.None || left != CellType.None;

            outputGrid[cords] = CellType.Wall;
        }
        else if (left == CellType.Wall && right == CellType.Wall && above != CellType.Wall && below != CellType.Wall)
        {
            sr.sprite = tilePallete.yWallTile;
            sr.flipY = above == CellType.None || below != CellType.None;

            outputGrid[cords] = CellType.Wall;
        }
        else
        {
            int noneCells = GetAdjacentNeighbors(CellType.None, cords).Count;

            if (noneCells == 0)
            {
                sr.sprite = tilePallete.middleTile;

                outputGrid[cords] = CellType.Room;
            }
            else
            {
                sr.sprite = tilePallete.cornerTile;

                sr.flipY = above == CellType.None || below != CellType.None;
                sr.flipX = right == CellType.None || left != CellType.None;

                outputGrid[cords] = CellType.Wall;
            }
        }
    }

    bool isValid(Vector2Int cords)
    {
        return cords.x >= 0 && cords.x < size && cords.y >= 0 && cords.y < size;
    }

    bool isValid(Vector2Int cords, int min)
    {
        return cords.x >= min && cords.x < size - min && cords.y >= min && cords.y < size - min;
    }

    List<Vector2Int> GetAdjacentNeighbors(CellType cellType, Vector2Int cords)
    {
        List<Vector2Int> output = new List<Vector2Int>();

        if (isValid(cords + Vector2Int.up) && grid[cords + Vector2Int.up] == cellType)
        {
            output.Add(cords + Vector2Int.up);
        }

        if (isValid(cords + Vector2Int.down) && grid[cords + Vector2Int.down] == cellType)
        {
            output.Add(cords + Vector2Int.down);
        }

        if (isValid(cords + Vector2Int.right) && grid[cords + Vector2Int.right] == cellType)
        {
            output.Add(cords + Vector2Int.right);
        }

        if (isValid(cords + Vector2Int.left) && grid[cords + Vector2Int.left] == cellType)
        {
            output.Add(cords + Vector2Int.left);
        }

        return output;
    }
    List<Vector2Int> GetCellNeighbors(CellType cellType, Vector2Int input, int size)
    {
        List<Vector2Int> output = new List<Vector2Int>();

        for (int y = -size; y <= size; y++)
        {
            for (int x = -size; x <= size; x++)
            {
                if (x != 0 || y != 0)
                {
                    Vector2Int cords = input + new Vector2Int(x, y);

                    if (isValid(cords) && grid[cords] == cellType)
                    {
                        output.Add(input + new Vector2Int(x, y));
                    }
                }
            }
        }
        return output;
    }
}
