using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using Graphs;
using UnityEngine.SceneManagement;

public class Generator2D : MonoBehaviour
{
    enum CellType
    {
        None,
        Room,
        Hallway
    }

    class Room
    {
        public RectInt bounds;

        public Room(Vector2Int location, Vector2Int size)
        {
            bounds = new RectInt(location, size);
        }

        public static bool Intersect(Room a, Room b)
        {
            return !((a.bounds.position.x >= (b.bounds.position.x + b.bounds.size.x)) || ((a.bounds.position.x + a.bounds.size.x) <= b.bounds.position.x)
                || (a.bounds.position.y >= (b.bounds.position.y + b.bounds.size.y)) || ((a.bounds.position.y + a.bounds.size.y) <= b.bounds.position.y));
        }
    }

    [Header("Debug")]
    [SerializeField] bool isRepeatGeneration;
    [Space(10)]
    [SerializeField] bool isRandomSeed;
    [SerializeField] int seed;
    [Space(10)]
    [SerializeField] [Range(1f, 3f)] float waitTime = 2.5f;

    [Header("Dungeon Size")]
    [SerializeField] float tileSize = 1;

    [SerializeField] [Range(10, 250)] int size = 15;
    [SerializeField] [Range(10, 500)] int roomCount = 100;
    [SerializeField] [Range(0f, 1f)] float hallwayChance = 0.125f;

    [Header("Room Size")]
    [SerializeField] [Range(1, 10)] int roomMinSize;
    [SerializeField] [Range(5, 25)] int roomMaxSize;

    [Header("Tile")]
    [SerializeField] GameObject cubePrefab;

    [Header("Walls")]
    [SerializeField] Sprite xWallTile;
    [SerializeField] Sprite yWallTile;

    [SerializeField] Sprite cornerTile;

    [SerializeField] Sprite middleTile;

    Random random;
    Grid2D<CellType> grid;
    List<Room> rooms;
    Delaunay2D delaunay;
    HashSet<Prim.Edge> selectedEdges;

    void Start()
    {
        Generate();

        if (isRepeatGeneration)
        {
            StartCoroutine(RepeatGenerate());
        }
    }

    IEnumerator RepeatGenerate()
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene("Dungeon");
    }

    void Generate()
    {
        if (isRandomSeed)
        {
            random = new Random();
        }
        else
        {
            random = new Random(seed);
        }

        grid = new Grid2D<CellType>(size, Vector2Int.zero);
        rooms = new List<Room>();

        PlaceRooms();
        Triangulate();
        CreateHallways();
        PathfindHallways();
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
            Room buffer = new Room(location + new Vector2Int(-1, -1), roomSize + new Vector2Int(2, 2));

            foreach (var room in rooms)
            {
                if (Room.Intersect(room, buffer))
                {
                    add = false;
                    break;
                }
            }

            if (newRoom.bounds.xMin < 0 || newRoom.bounds.xMax >= size
                || newRoom.bounds.yMin < 0 || newRoom.bounds.yMax >= size)
            {
                add = false;
            }

            if (add)
            {
                rooms.Add(newRoom);

                foreach (var pos in newRoom.bounds.allPositionsWithin)
                {
                    grid[pos] = CellType.Room;
                }

                PlaceRoom(newRoom);
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

        List<Prim.Edge> mst = Prim.MinimumSpanningTree(edges, edges[0].U);

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
            var startRoom = (edge.U as Vertex<Room>).Item;
            var endRoom = (edge.V as Vertex<Room>).Item;

            var startPosf = startRoom.bounds.center;
            var endPosf = endRoom.bounds.center;
            var startPos = new Vector2Int((int)startPosf.x, (int)startPosf.y);
            var endPos = new Vector2Int((int)endPosf.x, (int)endPosf.y);

            GameObject hallwayParent = new GameObject();
            hallwayParent.name = "Hallway";

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

            if (path != null)
            {
                for (int i = 0; i < path.Count; i++)
                {
                    var current = path[i];

                    if (grid[current] == CellType.None)
                    {
                        grid[current] = CellType.Hallway;
                    }

                    if (i > 0)
                    {
                        var prev = path[i - 1];

                        var delta = current - prev;
                    }
                }

                //place a hallway around all empty tiles that surrond the path
                foreach (var pos in path)
                {
                    if (grid[pos] == CellType.Hallway)
                    {
                        PlaceHallway(pos, hallwayParent.transform);

                        foreach (Vector2Int cord in GetCellNeighbors(CellType.None, pos))
                        {
                            PlaceHallway(cord, hallwayParent.transform);
                        }
                    }
                }
            }
        }
    }

    void PlaceCube(Vector2Int position, Vector2Int local, Transform parentTranform, Room room = null)
    {
        Vector3 cord = new Vector3(position.x, 0, position.y) + new Vector3(local.x, 0, local.y);

        GameObject go = Instantiate(cubePrefab, cord, Quaternion.identity, parentTranform);
        go.transform.localScale = new Vector3(tileSize, 1, tileSize);

        SpriteRenderer sr = go.GetComponentInChildren<SpriteRenderer>();
        Vector2Int cords = position + local;

        if (sr)
        {
            SetSprite(sr, cords, room, grid[cords]);
        }
    }

    void PlaceRoom(Room room)
    {
        GameObject roomParent = new GameObject();
        roomParent.name = "Room";

        for (int y = 0; y < room.bounds.size.y; y++)
        {
            for (int x = 0; x < room.bounds.size.x; x++)
            {
                PlaceCube(room.bounds.position, new Vector2Int(x, y), roomParent.transform, room);
            }
        }
    }

    void PlaceHallway(Vector2Int location, Transform hallwayParent)
    {
        PlaceCube(location, Vector2Int.zero, hallwayParent);
    }

    void SetSprite(SpriteRenderer sr, Vector2Int cords, Room room, CellType cellType)
    {
        if (cellType == CellType.Room)
        {
            SetRoomSprites(sr, cords, room);
        }
        else if (cellType == CellType.Hallway)
        {
            SetHallwaySprites(sr, cords, room);
        }
    }

    List<Vector2Int> GetCellNeighbors(CellType cellType, Vector2Int input)
    {
        List<Vector2Int> output = new List<Vector2Int>();

        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                if (x != 0 || y != 0)
                {
                    Vector2Int cords = input + new Vector2Int(x, y);

                    if (grid[cords] == cellType)
                    {
                        output.Add(input + new Vector2Int(x, y));
                    }
                }
            }
        }

        return output;
    }

    void SetHallwaySprites(SpriteRenderer sr, Vector2Int cords, Room room)
    {
        int count = GetCellNeighbors(CellType.Hallway, cords).Count;

        sr.sprite = middleTile;
    }

    void SetRoomSprites(SpriteRenderer sr, Vector2Int cords, Room room)
    {
        int count = GetCellNeighbors(CellType.Room, cords).Count;

        //corner
        if (count == 3)
        {
            sr.flipX = cords.x != room.bounds.min.x;
            sr.flipY = cords.y != room.bounds.min.y;

            sr.sprite = cornerTile;
        }
        //wall
        else if (count == 5)
        {
            if (cords.x == room.bounds.min.x || cords.x == room.bounds.max.x - 1)
            {
                sr.sprite = xWallTile;
                sr.flipX = cords.x != room.bounds.min.x;
            }

            if (cords.y == room.bounds.min.y || cords.y == room.bounds.max.y - 1)
            {
                sr.sprite = yWallTile;
                sr.flipY = cords.y != room.bounds.min.y;
            }
        }
        //middle
        else if (count == 8)
        {
            sr.sprite = middleTile;
        }
    }
}
