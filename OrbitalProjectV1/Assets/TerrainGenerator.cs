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
    private Tilemap terrainTilemap,terrainWallTilemap, groundDecoTilemap;

    [SerializeField]
    private TileBase horizwall, vertwall, luwall, ldwall, ruwall, rdwall, allardwall;

    [SerializeField]
    private TileBase terraintile, grassTile;

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
        map = GenerateMapTesting();
        PaintWalls(map);
        PaintTerrainTile(map);
        
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
        //Debug.Log(random.Next());
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

    private void MinimumSpanTree(int width, int height)
    {
        for (int i = 0; i < width; i++ )
        {
            for (int j = 0; j < height; j++ )
            {

            }
        }
    }
}


// ## do this tmr
// do i need to use this.
// 1. Start at 0,0.
// 2. do simple 8dir-dfs, find all neighbouring cells that are not 1, add into set. If cell == 1, stop.
// 3. Get a random point in the bound that is not inside this set. // instead of using a random point, can just create multiple sets like dsu. Randompoint will add computational time.
// 4. Repeat step2.
// 5. Connect the individual sets using Manhattan distance of the center of the sets.
// 6. Use djikstra to create a path between sets.
// 7. Remove all decorations/walls in the path.

public class MST
{
    //if i use two hashset, i dont have to use a separate node class.
    Dictionary<Vector2Int, int> map;
    List<Node> path;

    public MST(Dictionary<Vector2Int,int> _map)
    {
        map = _map;
        InitializeDSU();
    }

    private void InitializeDSU()
    {
        // need to sort first, dict unsorted.
        // 
        foreach(Vector2Int vec in map.Keys)
        {
            if (map[vec] == 0)
            {
                
            }
        }
    }

    public void MakeSet(Node a)
    {
        if (a.parent == null)
        {
            a.parent = a;
            a.size = 1;
            a.rank = 1;
        }
    }

    public void Union(Node a, Node b)
    {
        var aparent = findParent(a);
        var bparent = findParent(b);

        //already in the same set
        if (aparent == bparent)
        {
            return;
        } else
        {
            if (aparent.size >= bparent.size)
            {
                bparent.parent = aparent;
                aparent.size += bparent.size;
                path.Add(b);
            }
        }
    }

    public Node findParent(Node node)
    {
        if (node.parent != node)
        {
            //recursive function to find the base node.
            node.parent = findParent(node.parent);
            
        }
        return node.parent;
    }

}


public class Node
{
    //at the start all node's parent is themselves.
    //rank = 0;
    //size = 0;

    public Node parent;
    public int size;
    public int rank;
    public Vector2Int vec;


    public Node(Vector2Int _vec)
    {
        vec = _vec;
    }
}
