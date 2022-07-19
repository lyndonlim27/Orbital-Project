using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;

public class TerrainGenerator : MonoBehaviour
{
    public static TerrainGenerator instance { get; private set; }

    [Header("Terrain Properties For Testing")]

    [SerializeField]
    private int width;

    [SerializeField]
    private int height;

    [SerializeField]
    private Tilemap terrainTilemap,terrainWallTilemap, groundDecoTilemap, kruskalVisualizer;

    [SerializeField]
    private TileBase horizwall, vertwall, luwall, ldwall, ruwall, rdwall, allardwall;

    [SerializeField]
    private TileBase terraintile, grassTile, kruskalTile;

    [SerializeField]
    private TileBase[] terrainInteriorDecorations, terrainExteriorDecorations;

    [SerializeField]
    [Range(0,1)]
    private float normalizer;

    [SerializeField]
    GameObject[] terrainObjects;
    

    private GameObject TerrainObjectHolder;
    private float frequency;
    private int seed;
    private int offset;
    private System.Random random;

    /// <summary>
    /// use this instead for loading of level design.
    /// </summary>
    [Header("Terrain Design")]
    [SerializeField]
    LevelDesign levelDesignData; 

    private Dictionary<Vector3Int, int> map;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
        TerrainObjectHolder = new GameObject("TerrainObjectHolder");
        //default
        frequency = 0.5f;
        //seed = 10;
        //random = new System.Random(seed);
        LoadLevel();
    }

    public void GenerateTerrain()
    {
        ClearAllTerrainObjects();
        terrainTilemap.ClearAllTiles();
        terrainWallTilemap.ClearAllTiles();
        groundDecoTilemap.ClearAllTiles();
        kruskalVisualizer.ClearAllTiles();
        map = GenerateMapTesting();
        PaintWalls(map);
        PaintTerrainTile(map);
        CreateConnections(width,height);
        
    }

    public void GenerateTerrain(RoomManager currRoom)
    {
        ClearAllTerrainObjects();
        terrainTilemap.ClearAllTiles();
        terrainWallTilemap.ClearAllTiles();
        groundDecoTilemap.ClearAllTiles();
        map = GenerateMap(currRoom);
        PaintWalls(map);
        PaintTerrainTile(map);
        TerrainObjectHolder.transform.SetParent(currRoom.transform);
    }

    private void ClearAllTerrainObjects()
    {
        DestroyImmediate(TerrainObjectHolder);
        TerrainObjectHolder = new GameObject("TerrainObjectHolder");
    }

    private void PaintTerrainTile(Dictionary<Vector3Int, int> map)
    {
        foreach(Vector3Int vec in map.Keys)
        {
            string neighbours = "";
            if (map[vec] == 1)
            {

                neighbours += GetNeighbours(vec, map);
                neighbours += GetNeighboursDiag(vec, map);
                if (neighbours == "11111111")
                {
                    SpawnObject(vec);
                }
                terrainTilemap.SetTile(vec,terraintile);

                //if (NotAtBoundsTesting(vec))

                if (!terrainWallTilemap.HasTile(vec))
                {
                    PlaceDecorationCon(terrainInteriorDecorations, vec);
                }
                
            } else
            {
                terrainTilemap.SetTile(vec, grassTile);
                if (!terrainWallTilemap.HasTile(vec))
                {
                    PlaceDecoration(terrainExteriorDecorations, vec);
                }
            }
        }
    }

    private void PlaceDecorationCon(TileBase[] decorations,Vector3Int pos)
    {
        if (decorations == null || decorations.All(x => x == null))
        {
            return;
        }
        bool place = Mathf.PerlinNoise(pos.x * normalizer, pos.y * normalizer) >= frequency;
        if (place)
        {
            groundDecoTilemap.SetTile(pos, decorations[UnityEngine.Random.Range(0, decorations.Length)]);
        }
    }

    private void PlaceDecoration(TileBase[] decorations, Vector3Int pos)
    {
        if (decorations == null || decorations.All(x => x == null))
        {
            return;
        }
        bool place = UnityEngine.Random.Range(0, 100) >= offset + frequency * 100;
        if (place)
        {
            groundDecoTilemap.SetTile(pos, decorations[UnityEngine.Random.Range(0, decorations.Length)]);
        }
    }

    private void SpawnObject(Vector3Int vec)
    {
        if (terrainObjects == null || terrainObjects.All(x => x == null))
        {
            return;
        }
            //set object transform parent to room later.

        bool SpawnObject = UnityEngine.Random.Range(0, 100) >= frequency * 100;
        if (SpawnObject)
        {
            Instantiate(terrainObjects[UnityEngine.Random.Range(0, terrainObjects.Length)], vec + new Vector3(1,1), Quaternion.identity, TerrainObjectHolder.transform);
        }
            
    }

    private void PaintWalls(Dictionary<Vector3Int, int> map)
    {
        foreach(Vector3Int vec in map.Keys)
        {
            if (map[vec] == 1)
            {
                string neighbours = GetNeighbours(vec,map);

                switch(neighbours)
                {
                    default:
                        break;
                    case "1111":
                        CheckDiagonals(vec, map);
                        break;
                    case "1100":
                        terrainWallTilemap.SetTile(vec, luwall);
                        break;
                    case "1010":
                    case "1110":
                    case "1011":
                        terrainWallTilemap.SetTile(vec, vertwall);
                        break;
                    case "0101":
                    case "1101":
                    case "0111":
                        terrainWallTilemap.SetTile(vec, horizwall);
                        break;
                    case "1001":
                        terrainWallTilemap.SetTile(vec, ruwall);
                        break;
                    case "0110":
                        terrainWallTilemap.SetTile(vec, ldwall);
                        break;
                    case "0011":
                        terrainWallTilemap.SetTile(vec, rdwall);
                        break;
                }
            }
        }
    }

    private void CheckDiagonals(Vector3Int vec, Dictionary<Vector3Int, int> map)
    {
        string neighbours = GetNeighboursDiag(vec, map);

        switch (neighbours)
        {
            default:
                break;
            case "1110":
                terrainWallTilemap.SetTile(vec, ldwall);
                break;
            case "1011":
                terrainWallTilemap.SetTile(vec, luwall);
                break;
            case "0111":
                terrainWallTilemap.SetTile(vec, ruwall);
                break;
            case "1101":
                terrainWallTilemap.SetTile(vec, rdwall);
                break;
            case "0011":
            case "1100":
                terrainWallTilemap.SetTile(vec, horizwall);
                break;
            case "1010":
            case "0101":
                terrainWallTilemap.SetTile(vec, vertwall);
                break;
            case "1001":
            case "0110":
                terrainWallTilemap.SetTile(vec, allardwall);
                break;
        }
    }

    private string GetNeighboursDiag(Vector3Int vec, Dictionary<Vector3Int, int> map)
    {
        int defaultValue = 0;
        int topleft = map.TryGetValue(vec + new Vector3Int(-1,1), out int upv) ? upv : defaultValue;
        int topright = map.TryGetValue(vec + new Vector3Int(1, 1), out int downv) ? downv : defaultValue;
        int bottomleft = map.TryGetValue(vec + new Vector3Int(-1, -1), out int leftv) ? leftv : defaultValue;
        int bottomright = map.TryGetValue(vec + new Vector3Int(1, -1), out int rightv) ? rightv : defaultValue;

        return $"{topleft}{topright}{bottomleft}{bottomright}";
    }

    private string GetNeighbours(Vector3Int vec, Dictionary<Vector3Int,int> map)
    {
        int defaultValue = 0;
        int up = map.TryGetValue(vec + Vector3Int.up, out int upv) ? upv : defaultValue;
        int down = map.TryGetValue(vec + Vector3Int.down, out int downv) ? downv : defaultValue;
        int left = map.TryGetValue(vec + Vector3Int.left, out int leftv) ? leftv : defaultValue;
        int right = map.TryGetValue(vec + Vector3Int.right, out int rightv) ? rightv : defaultValue;

        return $"{up}{right}{down}{left}";
    }

    private Dictionary<Vector3Int, int> GenerateMap(RoomManager currRoom)
    {
        Bounds _bounds = currRoom.GetRoomAreaBounds();
        map = new Dictionary<Vector3Int, int>();
        Vector3Int minVec = Vector3Int.RoundToInt(_bounds.min);
        Vector3Int maxVec = Vector3Int.RoundToInt(_bounds.max);
        for (int i = minVec.x; i <= maxVec.x; i++)
        {
            for (int j = minVec.y; j <= maxVec.y; j++)
            {
                if (i == minVec.x || i == maxVec.x || j == minVec.y || j == maxVec.y)
                {
                    map[new Vector3Int(i, j)] = 1;
                } else
                {
                    
                    map[new Vector3Int(i,j)] = Mathf.RoundToInt(Mathf.PerlinNoise((i - minVec.x) * normalizer, (j - minVec.y) * normalizer));
                }

            }
        }

        return map;
    }

    private Dictionary<Vector3Int, int> GenerateMapTesting()
    {
        map = new Dictionary<Vector3Int, int>();
        for (int i = 0; i <= width; i++)
        {
            for (int j = 0; j <= height; j++)
            {
                if (i == 0|| i == width || j == 0 || j == height)
                {
                    map[new Vector3Int(i, j)] = 1;
                }
                else
                {
                    map[new Vector3Int(i, j)] = Mathf.RoundToInt(Mathf.PerlinNoise(i * normalizer,j * normalizer));
                }

            }
        }

        return map;
    }

    /// <summary>
    /// Client access method  to load level design.
    /// </summary>
    public void LoadLevel()
    {
        if (levelDesignData == null)
        {
            Debug.LogError("No data loaded");
        } else
        {
            LoadTiles(levelDesignData);
        }
    }

    /// <summary>
    /// To Load all relevant tiles using data.
    /// </summary>
    /// <param name="levelDesign"></param>
    private void LoadTiles(LevelDesign levelDesign)
    {
        horizwall = levelDesign.horizwall;
        vertwall = levelDesign.vertwall;
        luwall = levelDesign.luwall;
        ldwall = levelDesign.ldwall;
        ruwall = levelDesign.ruwall;
        rdwall = levelDesign.rdwall;
        allardwall = levelDesign.allardwall;
        terraintile = levelDesign.floorTile;
        grassTile = levelDesign.grassTile;
        terrainObjects = levelDesign.terrainObjects;
        terrainInteriorDecorations = levelDesign.intterrainDecorations;
        terrainExteriorDecorations = levelDesign.extterrainDecorations;
        frequency = levelDesign.frequency;
        offset = levelDesign.offset;
        normalizer = levelDesign.normalizer;
    }




    // Look For all Enclosed Spaces, find MST to every other enclosed spaces.
    // do dsu first to connect all spaces in 1 object.
    // Remove all decorations/walls in this path.

    /// <summary>
    /// Initialize segments and create pavement.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    private void CreateConnections(int width, int height)
    {
        List<List<Vector3Int>> segments = PartitionSegments();
        List<Edge> graph = CreateGraph(segments);

        // now that we have the areas segmented and sorted, we just need to do a simple MST to find the shortest path that connects all these segments.
        // choice of union : by size
        List<Edge> minconnectors = Kruskal(segments, graph);
        foreach(Edge edge in minconnectors)
        {
            ClearPavement(GetMidPoint(segments[edge.src]), GetMidPoint(segments[edge.dest]));
        }

    }

    /// <summary>
    /// Clear Pavement using RandomWalk
    /// </summary>
    /// <param name="curr"></param>
    /// <param name="dest"></param>
    private void ClearPavement(Vector3Int curr, Vector3Int dest)
    {
        List<Vector3Int> pathing = new List<Vector3Int>();
        while (curr != dest)
        {
            pathing.Add(curr);
            curr = GetNextPosition(curr,dest);
        }


        foreach (Vector3Int vec in pathing)
        {
            Collider2D col = Physics2D.OverlapPoint((Vector2Int)vec);
            RemoveWall(vec);
            if (col != null)
            {
                DestroyImmediate(col.gameObject);
            }

        }
    }

    private void RemoveWall(Vector3Int vec)
    {
        
        terrainWallTilemap.SetTile(vec, null);
        terrainTilemap.SetTile(vec, grassTile);
        kruskalVisualizer.SetTile(vec, kruskalTile);
    }

    private Vector3Int GetNextPosition(Vector3Int curr, Vector3Int dest)
    {
        Vector3Int vec = Vector3Int.zero;
        IncrementVector(curr, dest, ref vec, UnityEngine.Random.value >= 0.5f);
        Vector3Int newpos = curr + vec;
        return newpos;

    }

    private void IncrementVector(Vector3Int curr, Vector3Int dest, ref Vector3Int vec, bool horizontal)
    {
        int lastval = horizontal ? dest.x : dest.y;
        int startval = horizontal ? curr.x : curr.y;
        if (lastval > startval)
        {
            vec += horizontal ? Vector3Int.right : Vector3Int.up;
        }
        else if (lastval < startval)
        {
            vec += horizontal ? Vector3Int.left : Vector3Int.down;
        }
    }

    /// <summary>
    /// Finding minimum number of connections to connect all segments.
    /// </summary>
    /// <param name="segments"></param>
    /// <param name="graph"></param>
    /// <returns></returns>
    private List<Edge> Kruskal(List<List<Vector3Int>> segments, List<Edge> graph)
    {
        int[] parent = new int[segments.Count];
        int[] size = new int[segments.Count];
        MakeSets(segments, parent, size);
        List<Edge> minconnections = new List<Edge>();
        foreach (Edge edge in graph)
        {
            Union(parent, size, edge, minconnections);
        }

        return minconnections;
    }

    /// <summary>
    /// Find root parent of current subset.
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="subset"></param>
    /// <returns></returns>
    private int Find(int[] parent, int subset)
    {
        if (parent[subset] == subset)
        {
            return subset;
        }
        return Find(parent, parent[subset]);
    }

    /// <summary>
    /// Union two subsets.
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="size"></param>
    /// <param name="edge"></param>
    /// <param name="minconnections"></param>
    private void Union(int[] parent, int[] size, Edge edge, List<Edge> minconnections)
    {
        var p1 = Find(parent,edge.src);
        var p2 = Find(parent, edge.dest);
        if (p1 != p2)
        {
            if (size[p1] >= size[p2])
            {
                parent[p2] = p1;
            } else
            {
                parent[p1] = p2;
            }
            minconnections.Add(edge);
        }
        
    }

    /// <summary>
    /// Create subset.
    /// </summary>
    /// <param name="segments"></param>
    /// <param name="parent"></param>
    /// <param name="size"></param>
    private void MakeSets(List<List<Vector3Int>> segments, int[] parent, int[] size)
    {
        for (int i = 0; i < segments.Count; i++)
        {
            parent[i] = i;
            size[i] = 1;
        }
    }

    /// <summary>
    /// Partitioning segments using dfs.
    /// </summary>
    /// <returns></returns>
    private List<List<Vector3Int>> PartitionSegments()
    {
        List<Vector3Int> keys = new List<Vector3Int>(map.Keys);
        //should i sort them? then getting the center will just be /2, but this adds complexity. Does it even matter. /2 will always be approximately center.
        HashSet<Vector3Int> visited = new HashSet<Vector3Int>();
        List<List<Vector3Int>> sets = new List<List<Vector3Int>>();
        foreach (Vector3Int vec in keys)
        {
            if (Valid(visited, vec))
            {
                var curr = vec;
                List<Vector3Int> neighbours = new List<Vector3Int>();
                AddAllConnectedCells(curr, neighbours, visited);
                sets.Add(neighbours);
            }

        }

        return sets;
    }

    private bool Valid(HashSet<Vector3Int> visited, Vector3Int vec)
    {
        return !visited.Contains(vec) && map[vec] == 0; //&& vec.x != 0 && vec.x != width && vec.y != 0 && vec.y != height;
    }

    /// <summary>
    /// Finding all neighbouring cells.
    /// </summary>
    /// <param name="curr"></param>
    /// <param name="neighbours"></param>
    /// <param name="visited"></param>
    private void AddAllConnectedCells(Vector3Int curr, List<Vector3Int> neighbours, HashSet<Vector3Int> visited)
    {
        if (map[curr] == 1)
        {
            return;
        }
        else
        {
            neighbours.Add(curr);
            visited.Add(curr);
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {

                    var nextposition = curr + new Vector3Int(i, j);
                    if (map.ContainsKey(nextposition) && !neighbours.Contains(nextposition) && Valid(visited,nextposition))
                    {
                        AddAllConnectedCells(nextposition, neighbours, visited);
                    }

                }
            }
        }
              
    }

    /// <summary>
    /// Creating graph of edges with euclidean distance as cost between segments.
    /// </summary>
    /// <param name="sets"></param>
    /// <returns></returns>
    private List<Edge> CreateGraph(List<List<Vector3Int>> sets)
    {
        List<Edge> connections = new List<Edge>();
        //Create at least one edges for all vertices using their euclidean distance as the cost.
        for (int i = 0; i < sets.Count; i++)
        {
            for (int j = i; j < sets.Count; j++)
            {
                float cost = GetEuclideanDistance(sets[i], sets[j]);
                Edge edge = new Edge(i, j, cost);
                connections.Add(edge);
            }
        }

        //after sorting 
        connections.Sort();
        return connections;

    }

    /// <summary>
    /// Getting the manhattandistance between two points.
    /// </summary>
    /// <param name="vector3Ints1"></param>
    /// <param name="vector3Ints2"></param>
    /// <returns></returns>
    private float GetEuclideanDistance(List<Vector3Int> vector3Ints1, List<Vector3Int> vector3Ints2)
    {
        //just gonna assume that midpoint of the list of vectors is roughly the midpoint of the segment.
        Vector3Int midPoint1 = GetMidPoint(vector3Ints1);
        Vector3Int midPoint2 = GetMidPoint(vector3Ints2);
        return Vector3Int.Distance(midPoint1, midPoint2);

    }

    /// <summary>
    /// Get Midpoint of the segment.
    /// </summary>
    /// <param name="vector3Ints1"></param>
    /// <returns></returns>
    private Vector3Int GetMidPoint(List<Vector3Int> vector3Ints1)
    {
        int index = (int)(vector3Ints1.Count / 2);
        return vector3Ints1[Mathf.Max(index - 1, 0)];
    }

    
}

public class Edge : IComparable<Edge>
{
    public int src, dest;
    public float weight;

    public Edge(int index1, int index2, float _weight)
    {
        src = index1;
        dest = index2;
        weight = _weight;
    }

    public int CompareTo(Edge other)
    {
        return (int) (this.weight - other.weight);
    }
}

