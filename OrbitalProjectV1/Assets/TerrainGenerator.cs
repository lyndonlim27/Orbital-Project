using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;
using System.Text;

public class TerrainGenerator : MonoBehaviour
{
    #region Variables
    
    #region InspectorFields
    [Header("Terrain Properties For Testing")]

    [SerializeField]
    private int width;

    [SerializeField]
    private int height;

    [SerializeField]
    private Tilemap terrainTilemap,terrainWallTilemap, groundDecoTilemap, wallTilemap, outerwallTilemap, kruskalVisualizer, unwalkableTilemap;

    [SerializeField]
    private TileBase horizwall, vertwall, luwall, ldwall, ruwall, rdwall, allardwall, topwall,bottomwall, leftwall, rightwall,
        lucorner,rucorner, ldcorner, rdcorner;

    [SerializeField]
    private TileBase terraintile, grassTile, kruskalTile, unwalkableTile;

    [SerializeField]
    private TileBase[] terrainInteriorDecorations, terrainExteriorDecorations;

    [SerializeField]
    [Range(0,1)]
    private float normalizer;

    [SerializeField]
    GameObject[] terrainObjects;

    /// <summary>
    /// use this instead for loading of level design.
    /// </summary>
    [Header("Terrain Design")]
    [SerializeField]
    LevelDesign levelDesignData;

    #endregion

    #region Internal Properties
    public static TerrainGenerator instance { get; private set; }
    private TilemapVisualizer tilemapVisualizer;
    private GameObject TerrainObjectHolder;
    private float frequency;
    private int offset;
    private Dictionary<List<Vector3Int>, KDTreeImpl> cachedTrees;
    private Dictionary<Vector3Int, int> map;
    public HashSet<Vector3Int> unWalkableTiles;
    #endregion

    #endregion

    #region Monobehaviour
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        ClearAllTerrainObjects();
        TerrainObjectHolder = new GameObject("TerrainObjectHolder");
        frequency = 0.5f;
        unWalkableTiles = new HashSet<Vector3Int>();
    }

    private void Start()
    {
        tilemapVisualizer = TilemapVisualizer.instance;
    }
    #endregion

    #region Client-Access Methods

    #region Generation
    public void GenerateTerrain()
    {
        ClearAllTerrainObjects();
        map = GenerateMapTesting();
        PaintWalls(map);
        PaintTerrainTile(map, null);
        CreateConnections();
        
    }

    public void GenerateTerrainType2()
    {
        ClearAllTerrainObjects();
        map = GenerateMapTesting();
        Paint8DirectWalls();
        FillTerrainLand();
        CreateConnections();

    }

    public void GenerateTerrain(RoomManager currRoom)
    {
        map = GenerateMap(currRoom);
        PaintWalls(map);
        CreateConnections();
        PaintTerrainTile(map, currRoom);
        TerrainObjectHolder.transform.SetParent(currRoom.transform);
        
        
    }

    public void GenerateTerrainType2(RoomManager currRoom)
    {
        map = GenerateMap(currRoom);
        Paint8DirectWalls();
        CreateConnections();
        FillTerrainLand(currRoom);


    }

    public void GenerateTerrainType3(RoomManager currRoom)
    {
        map = GenerateEmptyMap(currRoom);
        FillTerrainLand(currRoom);
        //Paint8DirectWalls();
        //CreateConnections();
        //FillTerrainLand(currRoom);


    }

    

    #endregion

    #region LoadingData
    /// <summary>
    /// Testing from inspector.
    /// </summary>
    public void LoadLevel()
    {
        if (levelDesignData == null)
        {
            Debug.LogError("No data loaded");
        }
        else
        {
            LoadTiles(levelDesignData);
        }
    }
    #endregion

    #region Getters
    public Tilemap GetTerrainWall()
    {
        return terrainWallTilemap;
    }

    public void ClearCorridoor(Vector3Int point)
    {
        double distance = Mathf.Infinity;
        Vector3Int nearestpointtodoor = Vector3Int.back;
        foreach (KDTreeImpl tree in cachedTrees.Values)
        {

            Vector3Int nearest = tree.findNearest(point);
            double sqrdist = GetSquaredDist(nearest, point);
            if (sqrdist < distance)
            {
                distance = sqrdist;
                nearestpointtodoor = nearest;
            }
        }
        if (nearestpointtodoor != Vector3Int.back)
        {
            ClearPavement(point, nearestpointtodoor);
        }
    }
    #endregion
    #endregion

    #region Internal Methods
    #region InternalCheckers
    private void Paint8DirectWalls()
    {
        List<Vector3Int> changedTiles = new List<Vector3Int>();
        foreach (Vector3Int vec in map.Keys)
        {
            if (map[vec] == 0)
            {
                changedTiles.Add(vec);
                string neighbours = GetNeighbours(vec, map);
                string diagneighbours = GetNeighboursDiag(vec, map);
                switch (neighbours)
                {
                    default:
                        changedTiles.Remove(vec);
                        break;
                    case "0000":
                        if (diagneighbours == "1000")
                        {
                            terrainWallTilemap.SetTile(vec, ldcorner);
                        }
                        else if (diagneighbours == "0100")
                        {
                            terrainWallTilemap.SetTile(vec, rdcorner);
                        }
                        else if (diagneighbours == "0010")
                        {
                            terrainWallTilemap.SetTile(vec, lucorner);
                        }
                        else if (diagneighbours == "0001")
                        {
                            terrainWallTilemap.SetTile(vec, rucorner);
                        } else
                        {
                            changedTiles.Remove(vec);
                        }
                        break;
                    case "1001":
                        if (diagneighbours[0] == '1')
                        {
                            terrainWallTilemap.SetTile(vec, ldwall);
                        } else
                        {
                            changedTiles.Remove(vec);
                        }
                        break;
                    case "1100":
                        if (diagneighbours[1] == '1')
                        {
                            terrainWallTilemap.SetTile(vec, rdwall);
                        } else
                        {
                            changedTiles.Remove(vec);
                        }
                        break;
                    case "0011":
                        if (diagneighbours[2] == '1')
                        {
                            terrainWallTilemap.SetTile(vec, luwall);
                        } else
                        {
                            changedTiles.Remove(vec);
                        }
                        break;
                    case "0110":
                        if (diagneighbours[3] == '1')
                        {
                            terrainWallTilemap.SetTile(vec, ruwall);
                        } else
                        {
                            changedTiles.Remove(vec);
                        }
                        break;
                    //top
                    case "1000":
                        if (diagneighbours[1] == '1' && diagneighbours[0] == '1')
                        {
                            terrainWallTilemap.SetTile(vec, topwall);
                        }
                        else if (diagneighbours[1] == '1')
                        {
                            terrainWallTilemap.SetTile(vec, rdwall);
                        }
                        else if (diagneighbours[0] == '1')
                        {
                            terrainWallTilemap.SetTile(vec, ldwall);
                        }
                        else
                        {
                            terrainWallTilemap.SetTile(vec, topwall);
                        }
                        break;
                    //left
                    case "0001":
                        if (diagneighbours[0] == '1' && diagneighbours[2] == '1')
                        {
                            terrainWallTilemap.SetTile(vec, leftwall);
                        }
                        else if (diagneighbours[0] == '1')
                        {
                            terrainWallTilemap.SetTile(vec, ldwall);
                        }
                        else if (diagneighbours[2] == '1')
                        {
                            terrainWallTilemap.SetTile(vec, luwall);
                        } else
                        {
                            terrainWallTilemap.SetTile(vec, leftwall);
                        }
                        break;
                    //right
                    case "0100":
                        if (diagneighbours[1] == '1' && diagneighbours[3] == '1')
                        {
                            terrainWallTilemap.SetTile(vec, rightwall);
                        }
                        else if (diagneighbours[1] == '1')
                        {
                            terrainWallTilemap.SetTile(vec, rdwall);
                        }
                        else if (diagneighbours[3] == '1')
                        {
                            terrainWallTilemap.SetTile(vec, ruwall);
                        } else
                        {
                            terrainWallTilemap.SetTile(vec, rightwall);
                        }
                        break;
                    //bottom 
                    case "0010":
                        if (diagneighbours[2] == '1' && diagneighbours[3] == '1')
                        {
                            terrainWallTilemap.SetTile(vec, bottomwall);
                        }
                        else if (diagneighbours[2] == '1')
                        {
                            terrainWallTilemap.SetTile(vec, luwall);
                        } else if (diagneighbours[3] == '1')
                        {
                            terrainWallTilemap.SetTile(vec, ruwall);
                        } else
                        {
                            terrainWallTilemap.SetTile(vec, bottomwall);
                        }
                        break;
                    case "0111":
                        if (diagneighbours[3] == '1')
                        {
                            terrainWallTilemap.SetTile(vec, luwall);
                        } else if (diagneighbours[2] == '1')
                        {
                            terrainWallTilemap.SetTile(vec, ruwall);
                        } else
                        {
                            changedTiles.Remove(vec);
                        }
                        break;
                    case "1011":
                        if (diagneighbours[0] == '1')
                        {
                            terrainWallTilemap.SetTile(vec, rdwall);

                        } else if (diagneighbours[2] == '1')
                        {
                            terrainWallTilemap.SetTile(vec, luwall);
                        } else
                        {
                            changedTiles.Remove(vec);
                        }
                        break;
                    case "1101":
                        if (diagneighbours[0] == '1') {
                            terrainWallTilemap.SetTile(vec, ldwall);
                        } else if (diagneighbours[1] =='1')
                        {
                            terrainWallTilemap.SetTile(vec, rdwall);
                        } else
                        {
                            changedTiles.Remove(vec);
                        }
                        break;
                    case "1110":
                        if (diagneighbours[1] == '1')
                        {
                            terrainWallTilemap.SetTile(vec, rdwall);
                        } else if (diagneighbours[3] == '1')
                        {
                            terrainWallTilemap.SetTile(vec, luwall);
                        } else
                        {
                            changedTiles.Remove(vec);
                        }
                        break;

                }
            } else
            {
                unWalkableTiles.Add(vec);
                terrainWallTilemap.SetTile(vec, terraintile);
                
                
            }
        }

        foreach(Vector3Int vec in changedTiles)
        {
            map[vec] = 1;
            unWalkableTiles.Add(vec);
        }
    }

    private void PaintWalls(Dictionary<Vector3Int, int> map)
    {
        foreach (Vector3Int vec in map.Keys)
        {
            if (map[vec] == 1)
            {
                string neighbours = GetNeighbours(vec, map);
                unWalkableTiles.Add(vec);
                switch (neighbours)
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

    #endregion

    #region Painting Methods.
    private void PaintTerrainTile(Dictionary<Vector3Int, int> map, RoomManager room)
    {
        
        
        
        foreach (Vector3Int vec in map.Keys)
        {
            string neighbours = "";

            if (map[vec] == 1 && !kruskalVisualizer.HasTile(vec) && !Physics2D.OverlapCircle((Vector2Int) vec, 3, LayerMask.GetMask("Doors")))
            {
                unWalkableTiles.Add(vec);
                if (room != null)
                {
                    var bounds = room.GetSpawnAreaBound();
                    var radius = Vector3.Distance(bounds.max, bounds.center);
                    var distancefromcenter = Vector3.Distance(vec, bounds.center);
                    var normalizeddist = distancefromcenter / radius;
                    if (!terrainWallTilemap.HasTile(vec))
                    {
                        tilemapVisualizer.PaintGroundDecorations(2 * 0.2f, (Vector2Int)vec, normalizeddist);
                    }
                    
           
                }
                
                neighbours += GetNeighbours(vec, map);
                neighbours += GetNeighboursDiag(vec, map);
                if (neighbours == "11111111")
                {
                    SpawnObject(vec);
                }
                terrainTilemap.SetTile(vec,terraintile);
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

    private void FillTerrainLand()
    {
       
        foreach (Vector3Int vec in map.Keys)
        {
     
            terrainTilemap.SetTile(vec, grassTile);
        }
    }

    private void FillTerrainLand(RoomManager room)
    {
        var bounds = room.GetSpawnAreaBound();
        var radius = Vector3.Distance(bounds.max, bounds.center);
        foreach(Vector3Int vec in map.Keys)
        {
            if (map[vec] == 0 && !kruskalVisualizer.HasTile(vec) && !Physics2D.OverlapCircle((Vector2Int) vec, 3, LayerMask.GetMask("Doors")) && !Physics2D.OverlapCircle((Vector2Int) vec, 2, LayerMask.GetMask("Obstacles","HouseExterior","HouseInterior","PassableDeco")))
            {
                var distancefromcenter = Vector3.Distance(vec, bounds.center);
                var normalizeddist = distancefromcenter / radius;
                tilemapVisualizer.PaintGroundDecorations(2 * 0.2f, (Vector2Int)vec, normalizeddist);
                
            }
            terrainTilemap.SetTile(vec, grassTile);

        }

    }

    private void ClearAllTerrainObjects()
    {
        DestroyImmediate(TerrainObjectHolder);
        terrainTilemap.ClearAllTiles();
        terrainWallTilemap.ClearAllTiles();
        groundDecoTilemap.ClearAllTiles();
        kruskalVisualizer.ClearAllTiles();
        TerrainObjectHolder = new GameObject("TerrainObjectHolder");
    }
    #endregion

    #region Tilemaphelpers
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


    #endregion

    #region Generators
    private Dictionary<Vector3Int, int> GenerateMap(RoomManager currRoom)
    {
        //BoundsInt _bounds = currRoom.GetTileBounds();
        Vector3Int roomSize = (Vector3Int) currRoom.GetRoomSize();
        map = new Dictionary<Vector3Int, int>();
        
        Vector3Int minVec = Vector3Int.RoundToInt(currRoom.transform.position) - roomSize / 2;
        Vector3Int maxVec = Vector3Int.RoundToInt(currRoom.transform.position) + roomSize / 2 - Vector3Int.one;
        
        for (int i = minVec.x; i <= maxVec.x; i++)
        {
            for (int j = minVec.y; j <= maxVec.y; j++)
            {
                var currpos = new Vector3Int(i, j);
                if (wallTilemap.HasTile(currpos) || outerwallTilemap.HasTile(currpos))
                {
                    continue;
                } else
                {
                    //this only works for terraintype1
                    //doesnt really create path for type2
                    if (Physics2D.OverlapCircle((Vector2Int)currpos, 3, LayerMask.GetMask("Doors")))
                    {
                        map[currpos] = 0;
                    }
                    //else if (i == minVec.x || i == maxVec.x || j == minVec.y || j == maxVec.y)
                    //{
                    //    map[currpos] = 1;

                    else
                    {

                        map[currpos] = Mathf.RoundToInt(Mathf.PerlinNoise((i - minVec.x) * normalizer, (j - minVec.y) * normalizer));
                    }
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

    private Dictionary<Vector3Int, int> GenerateEmptyMap(RoomManager currRoom)
    {
        map = new Dictionary<Vector3Int, int>();
        Vector3Int roomSize = (Vector3Int)currRoom.GetRoomSize();

        Vector3Int minVec = Vector3Int.RoundToInt(currRoom.transform.position) - roomSize / 2;
        Vector3Int maxVec = Vector3Int.RoundToInt(currRoom.transform.position) + roomSize / 2 - Vector3Int.one;

        for (int i = minVec.x; i <= maxVec.x; i++)
        {
            for (int j = minVec.y; j <= maxVec.y; j++)
            {
                var currpos = new Vector3Int(i, j);
                map[currpos] = 0;
            }
        }

        return map;
    }

    //for debugging
    public void PaintUnwalkableTiles()
    {
        foreach(Vector3Int vec in unWalkableTiles)
        {
            unwalkableTilemap.SetTile(vec, unwalkableTile);
        }
    }

    #endregion

    #region Data Handling
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
        topwall = levelDesign.topwall;
        bottomwall = levelDesign.bottomwall;
        rightwall = levelDesign.rightwall;
        leftwall = levelDesign.leftwall;
        lucorner = levelDesign.lucorner;
        rucorner = levelDesign.rucorner;
        ldcorner = levelDesign.ldcorner;
        rdcorner = levelDesign.rdcorner;
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

    /// <summary>
    /// Load Level design for client.
    /// </summary>
    /// <param name="levelData"></param>
    public void LoadLevelData(LevelDesign levelData)
    { 
        levelDesignData = levelData;
    }

    #endregion

    #endregion

    #region Linking Terrains
    // Look For all Enclosed Spaces, find MST to every other enclosed spaces.
    // do dsu first to connect all spaces in 1 object.
    // Remove all decorations/walls in this path.

    #region Main Method
    /// <summary>
    /// Initialize segments and create pavement.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    private void CreateConnections()
    {
        cachedTrees = new Dictionary<List<Vector3Int>, KDTreeImpl>();
        List<List<Vector3Int>> segments = PartitionSegments();
        List<Edge> graph = CreateGraph(segments);
        
        // now that we have the areas segmented and sorted, we just need to do a simple MST to find the shortest path that connects all these segments.
        // choice of union : by size
        List<Edge> minconnectors = Kruskal(segments, graph);
        foreach(Edge edge in minconnectors)
        {

            //KDTree
            Tuple<Vector3Int, Vector3Int, double> shortestPair = GetShortestPair(segments[edge.src], segments[edge.dest]);
            ClearPavement(shortestPair.Item1, shortestPair.Item2);
            //MidPoint
            //ClearPavement(GetMidPoint(segments[edge.src]), GetMidPoint(segments[edge.dest]));
            
        }

    }
    #endregion

    #region Clearing Path.
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
            Collider2D col = Physics2D.OverlapPoint((Vector2Int)vec, LayerMask.GetMask("Obstacles","HouseExterior","HouseInterior","PassableDeco"));
            RemoveWall(vec);
            if (col != null && !col.CompareTag("Tiles") && !col.CompareTag("House"))
            {
                DestroyImmediate(col.gameObject);
            }

        }
    }

    private void RemoveWall(Vector3Int vec)
    {
        
        terrainWallTilemap.SetTile(vec, null);
        groundDecoTilemap.SetTile(vec, null);
        if (!terrainTilemap.HasTile(vec))
        {
            terrainTilemap.SetTile(vec, grassTile);
        }
        kruskalVisualizer.SetTile(vec, kruskalTile);
    }

    #endregion

    #region MinSpan Tree
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
    #endregion

    #region Graph Creation
    /// <summary>
    /// Partitioning segments using dfs and kdtree.
    /// </summary>
    /// <returns></returns>
    private List<List<Vector3Int>> PartitionSegments()
    {
        List<Vector3Int> keys = new List<Vector3Int>(map.Keys);
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
                Tuple<Vector3Int,Vector3Int,double> cost = GetShortestPair(sets[i], sets[j]);
                Edge edge = new Edge(i, j, cost.Item3);
                connections.Add(edge);
            }
        }

        //after sorting 
        connections.Sort();
        return connections;

    }
    #endregion

    #region Helper Methods
    /// <summary>
    /// Get Squared distance between two vectors.
    /// </summary>
    /// <param name="root"></param>
    /// <param name="point"></param>
    /// <returns></returns>
    private double GetSquaredDist(Vector3Int root, Vector3Int point)
    {

        //find squared dist
        return Mathf.Pow(point.x - root.x, 2) + Mathf.Pow(point.y - root.y, 2);
    }

    /// <summary>
    /// Get a random next position until reaching destination.
    /// </summary>
    /// <param name="curr"></param>
    /// <param name="dest"></param>
    /// <returns></returns>
    private Vector3Int GetNextPosition(Vector3Int curr, Vector3Int dest)
    {
        Vector3Int vec = Vector3Int.zero;
        IncrementVector(curr, dest, ref vec, UnityEngine.Random.value >= 0.5f);
        Vector3Int newpos = curr + vec;
        return newpos;

    }

    /// <summary>
    /// Incrementing vectors for random walk.
    /// </summary>
    /// <param name="curr"></param>
    /// <param name="dest"></param>
    /// <param name="vec"></param>
    /// <param name="horizontal"></param>
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
    /// Check validity of cell/vector
    /// </summary>
    /// <param name="visited"></param>
    /// <param name="vec"></param>
    /// <returns></returns>
    private bool Valid(HashSet<Vector3Int> visited, Vector3Int vec)
    {
        return !visited.Contains(vec) && map[vec] == 0; //&& vec.x != 0 && vec.x != width && vec.y != 0 && vec.y != height;
    }

    /// <summary>
    /// using KDTree to return shortest pair of points.
    /// </summary>
    /// <param name="segment1"></param>
    /// <param name="segment2"></param>
    /// <returns></returns>
    private Tuple<Vector3Int, Vector3Int, double> GetShortestPair(List<Vector3Int> segment1, List<Vector3Int> segment2)
    {
        Tuple<Vector3Int, Vector3Int> shortestpair = new Tuple<Vector3Int, Vector3Int>(Vector3Int.zero, Vector3Int.zero);
        double shortestdistance = Mathf.Infinity;
        //find the 4 points in the segment.
        KDTreeImpl kdTree = FindTreeFromCache(segment1, segment2);
        foreach (Vector3Int point in segment2)
        {
            Vector3Int nearestpoint = kdTree.findNearest(point);
            double dist = GetSquaredDist(nearestpoint, point);
            if (dist < shortestdistance)
            {
                shortestpair = new Tuple<Vector3Int, Vector3Int>(point, nearestpoint);
                shortestdistance = dist;
            }
        }



        return new Tuple<Vector3Int, Vector3Int, double>(shortestpair.Item1, shortestpair.Item2, shortestdistance);
    }

    /// <summary>
    /// Finding cached KDTrees.
    /// </summary>
    /// <param name="segment1"></param>
    /// <param name="segment2"></param>
    /// <returns></returns>
    private KDTreeImpl FindTreeFromCache(List<Vector3Int> segment1, List<Vector3Int> segment2)
    {
        KDTreeImpl kdTree;
        if (cachedTrees.ContainsKey(segment1))
        {
            kdTree = cachedTrees[segment1];
        }
        else if (cachedTrees.ContainsKey(segment2))
        {
            kdTree = cachedTrees[segment2];
        }
        else
        {
            kdTree = KDTreeImpl.CreateTree(2, segment1);
            cachedTrees[segment1] = kdTree;
        }

        return kdTree;
    }
    #endregion

    #endregion

    #region Unused Methods
    /// <summary>
    /// Getting the manhattandistance between two points.
    /// </summary>
    /// <param name="vector3Ints1"></param>
    /// <param name="vector3Ints2"></param>
    /// <returns></returns>
    private double GetEuclideanDistance(List<Vector3Int> vector3Ints1, List<Vector3Int> vector3Ints2)
    {
        //just gonna assume that midpoint of the list of vectors is roughly the midpoint of the segment.

        Vector3Int midPoint1 = GetMidPoint(vector3Ints1);
        Vector3Int midPoint2 = GetMidPoint(vector3Ints2);

        return Vector3Int.Distance(midPoint1, midPoint2);
    }

    /// <summary>
    /// Get Midpoint of the segment.
    /// </summary>
    /// <param name = "vector3Ints1" ></ param >
    /// < returns ></ returns >
    private Vector3Int GetMidPoint(List<Vector3Int> vector3Ints1)
    {
        int index = (int)(vector3Ints1.Count / 2);
        return vector3Ints1[Mathf.Max(index - 1, 0)];
    }

    //private void ClearPathToDoors(DoorBehaviour[] doors)
    //{

    //    HashSet<Tuple<Vector3Int, Vector3Int>> points = new HashSet<Tuple<Vector3Int, Vector3Int>>();
    //    foreach (DoorBehaviour door in doors)
    //    {
    //        double distance = Mathf.Infinity;
    //        Vector3Int nearestpointtodoor = -Vector3Int.one;
    //        Vector3Int doorpoint = Vector3Int.RoundToInt(door.transform.position);
    //        foreach (KDTreeImpl tree in cachedTrees.Values)
    //        {

    //            Vector3Int nearest = tree.findNearest(doorpoint);
    //            double sqrdist = GetSquaredDist(nearest, doorpoint);
    //            if (sqrdist < distance)
    //            {
    //                distance = sqrdist;
    //                nearestpointtodoor = nearest;
    //            }
    //        }
    //        points.Add(new Tuple<Vector3Int, Vector3Int>(nearestpointtodoor, doorpoint));
    //    }

    //    foreach (Tuple<Vector3Int, Vector3Int> path in points)
    //    {
    //        if (path.Item1 != -Vector3Int.one)
    //        {
    //            Debug.Log($"This is {path.Item1} && {path.Item2}");
    //            ClearPavement(path.Item1, path.Item2);
    //        }
    //    }
    //}

    #endregion

}

#region DataStructures

#region Edge
public class Edge : IComparable<Edge>
{
    #region Variables
    public int src, dest;
    public double weight;
    #endregion

    #region Client-Access Methods
    public Edge(int index1, int index2, double _weight)
    {
        src = index1;
        dest = index2;
        weight = _weight;
    }

    public int CompareTo(Edge other)
    {
        return this.weight.CompareTo(other.weight);
    }
    #endregion

}
#endregion

#region KDTreeImpl
public class KDTreeImpl
{
    private KDTree kdTree;

    private KDTreeImpl(int dims, List<KDNode> kDNodes)
    {
        kdTree = new KDTree(dims, kDNodes);

    }

    public static KDTreeImpl CreateTree(int v, List<Vector3Int> segment)
    {

        return new KDTreeImpl(v, CreateNodes(segment));
    }

    private static List<KDNode> CreateNodes(List<Vector3Int> segment)
    {
        List<KDNode> kdNodes = new List<KDNode>();
        foreach (Vector3Int vec in segment)
        {
            kdNodes.Add(new KDNode(vec));
        }
        return kdNodes;
    }

    public Vector3Int findNearest(Vector3Int point)
    {
        return kdTree.findNearestNeighbour(point).getPoint();
    }

    public override string ToString()
    {
        return kdTree.ToString();
    }

    #region KDNode
    public class KDNode
    {
        private Vector3Int point;
        private KDNode left;
        private KDNode right;
        
        

        public KDNode(Vector3Int _point)
        {
            point = _point;
        }

        public Vector3Int getPoint()
        {
            return point;
        }

        public void SetLeft(KDNode node)
        {
            this.left = node;
        }

        public void SetRight(KDNode node)
        {
            this.right = node;
        }

        public KDNode GetLeft()
        {
            return this.left;
        }

        public KDNode GetRight()
        {
            return this.right;
        }
    }
    #endregion

    #region KDTree
    public class KDTree
    {
        #region Variables
        int Kdims;
        private KDNode root;
        private KDNode best;
        private double sqrdist;
        private int visited;
        #endregion

        #region KDTree Helpers
        /// <summary>
        /// Constructor for KDTree
        /// </summary>
        /// <param name="_dims"></param>
        /// <param name="kDNodes"></param>
        public KDTree(int _dims, List<KDNode> kDNodes)
        {
            Kdims = _dims;
            root = makeTree(kDNodes, 0);
            best = null;
            sqrdist = Mathf.Infinity;

        }

        /// <summary>
        /// Make Tree from list of knodes, return the root node.
        /// </summary>
        /// <param name="kDNodes"></param>
        /// <param name="depth"></param>
        /// <returns></returns>
        public KDNode makeTree(List<KDNode> kDNodes, int depth)
        {
            if (kDNodes.Count <= 0)
            {
                return null;
            }

            List<KDNode> sortedNodes = new List<KDNode>();
            int sortingAxis = depth % Kdims;

            //Since this is a 2d point, we only need to check for 0 and 1;

            if (sortingAxis == 0)
            {
                sortedNodes = kDNodes.OrderBy(node1 => node1.getPoint().x).ToList();
            } else
            {
                sortedNodes = kDNodes.OrderBy(node1 => node1.getPoint().y).ToList();
            }


            KDNode currNode = sortedNodes[sortedNodes.Count / 2];

            List<KDNode> left = sortedNodes.Skip(0).Take(sortedNodes.Count / 2).ToList();
            List<KDNode> right = sortedNodes.Skip(sortedNodes.Count / 2 + 1).Take(sortedNodes.Count - 1).ToList();
            currNode.SetLeft(makeTree(left,depth + 1));
            currNode.SetRight(makeTree(right,depth + 1));

            return currNode;
        }

        /// <summary>
        /// Find the nearest node to point from KDTree
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public KDNode findNearestNeighbour(Vector3Int point)
        {
            if (root == null)
            {
                Debug.Log("No tree found");
                return null;
            }

            
            return GetNearest(root, point, 0);
            
        }

        /// <summary>
        /// Recursive function find get the nearest node to point
        /// </summary>
        /// <param name="root"></param>
        /// <param name="point"></param>
        /// <param name="depth"></param>
        /// <returns></returns>
        private KDNode GetNearest(KDNode root, Vector3Int point, int depth)
        {
            if (root == null)
            {
                return best;
            }
            double sqrd = GetSquaredDist(root, point);
            //if there is no best at all, we just use this point. else we check this dist.
            if (sqrd < sqrdist || best == null)
            {
                best = root;
            }

            double diff = 0;
            if (depth % Kdims == 0)
            {
                diff = root.getPoint().x - point.x;
            }
            else
            {
                diff = root.getPoint().y - point.y;
            }

            GetNearest(diff > 0 ? root.GetLeft() : root.GetRight(), point, (depth + 1) % Kdims);
            //if sqrtdist is already the best. return
            //else search the other side.
            if (diff * diff >= sqrdist)
            {
                return best;
            }
            else
            {
                return GetNearest(diff > 0 ? root.GetRight() : root.GetLeft(), point, (depth + 1) % Kdims);
            }


        }

        /// <summary>
        /// Getting squared distance of node and point
        /// </summary>
        /// <param name="root"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        private double GetSquaredDist(KDNode root, Vector3Int point)
        {

            //find squared dist
            return Mathf.Pow(point.x - root.getPoint().x, 2) + Mathf.Pow(point.y - root.getPoint().y, 2);
        }
        
        /// <summary>
        /// for Debugging.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            Queue<KDNode> queue = new Queue<KDNode>();
            queue.Append(root);
            int queuesize = queue.Count;
            while (queuesize > 0)
            {
                
                foreach (KDNode node in queue)
                {
                    if (node != null)
                    {
                        sb.Append(node.getPoint()).Append(", ");
                        queue.Append(node.GetLeft());
                        queue.Append(node.GetRight());
                    }
                    else
                    {
                        sb.Append("end, ");
                    }
                }
                sb.Append('\n');
            }
            return sb.ToString();
        }
        #endregion
    }
    #endregion



    #endregion
}
#endregion
