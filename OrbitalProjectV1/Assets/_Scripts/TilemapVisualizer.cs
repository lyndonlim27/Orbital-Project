using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField]
    private Tilemap floorTilemap, innerwallTilemap, outerwallTilemap, waterTilemap, dockTilemap, groundTilemap, decorativeTilemap, treeBottomTilemap, treeTop1Tilemap, treeTop2Tilemap, treeTop3Tilemap, groundDecoTilemap;

    [Header("Map Tiles")]
    [SerializeField]
    private TileBase floorTile, wallTop, wallSideRight, wallSiderLeft, wallBottom, wallFull, 
        wallInnerCornerDownLeft, wallInnerCornerDownRight, 
        wallDiagonalCornerDownRight, wallDiagonalCornerDownLeft, wallDiagonalCornerUpRight, wallDiagonalCornerUpLeft;

    [SerializeField]
    private TileBase[] multifloorTiles;

    [Header("Dock tiles")]
    [SerializeField]
    private TileBase horizontalplank, verticalplank, ldcornerplank, rdcornerplank,lucornerplank,rucornerplank;

    [Header("Decorative Tiles")]
    [SerializeField]
    [Range(1, 100)]
    private int decorationCount;

    [SerializeField]
    private List<TileBase> waterdecoratives;
    [SerializeField]
    private List<TileBase> landdecoratives;

    [Header("Groundtiles")]
    [SerializeField]
    private List<TileBase> groundTiles;

    [Header("Other decorative objects")]
    [SerializeField]
    private Sprite ship;

    [SerializeField]
    private GameObject ship1, ship2, ship3;

    [SerializeField]
    private TileBase[] treeBottoms, treeTops;


    [Header("Structures")]
    [SerializeField]
    private List<GameObject> frontstructures, sidestructures;



    private Transform decorationContainer;

    //public void ActivateTilemapColliders()
    //{
    //    innerwallTilemap.gameObject.AddComponent<TilemapCollider2D>();
    //    outerwallTilemap.gameObject.AddComponent<TilemapCollider2D>();

    //}

    //public void DeactivateTilemapColliders()
    //{
    //    DestroyImmediate(innerwallTilemap.GetComponent<TilemapCollider2D>());
    //    DestroyImmediate(outerwallTilemap.GetComponent<TilemapCollider2D>());
    //}

    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions, float groundOffset, List<BoundsInt> rooms, HashSet<Vector2Int> corridoorVectors)
    {

        PaintGroundAndFloorTiles(floorPositions, floorTilemap, multifloorTiles[UnityEngine.Random.Range(0,multifloorTiles.Length)], groundOffset, rooms, corridoorVectors);
    }

    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach (var position in positions)
        {

            PaintSingleTile(tilemap, tile, position, false);
        }
    }

    private void PaintGroundAndFloorTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile, float offSet, List<BoundsInt> rooms, HashSet<Vector2Int> corridoors)
    {
        foreach (var position in positions)
        {
            foreach (BoundsInt bounds in rooms)
            {
                //Debug.Log("room" + bounds);
                if (insideRoom(position, bounds) && !corridoors.Contains(position))
                {
                    var distancefromcenter = Vector3.Distance((Vector3Int)position, bounds.center);
                    var radius = Vector3.Distance(bounds.max, bounds.center);
                    var normalizeddist = distancefromcenter / radius;
                    //Debug.Log("normalized distance = " + normalizeddist);
                    PaintGround(offSet, position, normalizeddist);
                    PaintGroundDecorations(offSet * 2f, position, normalizeddist);

                }

            }
            PaintSingleTile(tilemap, tile, position, false);

        }
        
    }

    private void PaintGround(float groundOffset, Vector2Int position, float normalizeddist)
    {
        if (waterdecoratives.Count == 0)
        {
            Debug.LogError("No waterdecoratives added");
        }
        if (UnityEngine.Random.value + groundOffset >= normalizeddist)
        {
            PaintSingleTile(groundTilemap, groundTiles[UnityEngine.Random.Range(0, groundTiles.Count)], position, false);
        }
    }


    private void PaintGroundDecorations(float decoOffset, Vector2Int position, float normalizeddist)
    {

        if (Physics2D.OverlapCircle(position, 0.01f, LayerMask.GetMask("Doors")))
        {
            return;
        }

        

        if (landdecoratives.Count == 0)
        {
            Debug.LogError("No landdecoratives added");
        }

        if (UnityEngine.Random.value + decoOffset <= normalizeddist)
        {
            
            int toPutTreeOrNot = UnityEngine.Random.Range(0, 10);
            
            

            if (toPutTreeOrNot <= 2 && toPutTreeOrNot <= 6)
            {

                PaintSingleTile(groundDecoTilemap, landdecoratives[UnityEngine.Random.Range(0, landdecoratives.Count)], position, false);
            }
            else if (toPutTreeOrNot >= 6)
            {
                int rand = UnityEngine.Random.Range(0, 3);
                Tilemap selectedlayer;
                switch (rand)
                {
                    default:
                    case 1:
                        selectedlayer = treeTop1Tilemap;
                        break;
                    case 2:
                        selectedlayer = treeTop2Tilemap;
                        break;
                    case 3:
                        selectedlayer = treeTop3Tilemap;
                        break;
                }
                PaintSingleTile(treeBottomTilemap, treeBottoms[UnityEngine.Random.Range(0,treeBottoms.Length)], position, false);
                PaintSingleTile(selectedlayer, treeTops[UnityEngine.Random.Range(0, treeBottoms.Length)], position + Vector2Int.up, false);
            }

        }




    }


    private bool insideRoom(Vector2Int vec, BoundsInt room)
    {
        return room.xMin + 3f <= vec.x && vec.x <= room.xMax - 3.5f && room.yMin + 3f <= vec.y && vec.y <= room.yMax - 3f;
    }

    

    public void PaintCorridoorTiles(IEnumerable<Vector2Int> corridoor)
    {
        foreach (var position in corridoor)
        {

            PaintSingleTile(groundTilemap, groundTiles[UnityEngine.Random.Range(0, groundTiles.Count)], position, false);
        }
    }

    internal void PaintSingleBasicWall(Vector2Int position, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        bool outer = false;
        TileBase tile = null;
        if (WallTypesHelper.wallTop.Contains(typeAsInt))
        {
            tile = wallTop;
        }else if (WallTypesHelper.wallSideRight.Contains(typeAsInt))
        {
            tile = wallSideRight;
        }
        else if (WallTypesHelper.wallSideLeft.Contains(typeAsInt))
        {
            tile = wallSiderLeft;
        }
        else if (WallTypesHelper.wallBottm.Contains(typeAsInt))
        {
            outer = true;
            tile = wallBottom;
        }
        else if (WallTypesHelper.wallFull.Contains(typeAsInt))
        {
            tile = wallFull;
        }

        if (tile!=null)
        {
            if (outer)
            {
                PaintSingleTile(outerwallTilemap, tile, position, outer);
            } else
            {
                PaintSingleTile(innerwallTilemap, tile, position, outer);
            }
        }
            
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position, bool outer)
    {
        if (outer)
        {
            tilemap.GetComponent<TilemapRenderer>().sortingOrder = 2;
        }
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile);
    }

    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        innerwallTilemap.ClearAllTiles();
        outerwallTilemap.ClearAllTiles();
        dockTilemap.ClearAllTiles();
        groundTilemap.ClearAllTiles();
        decorativeTilemap.ClearAllTiles();
        treeBottomTilemap.ClearAllTiles();
        treeTop1Tilemap.ClearAllTiles();
        treeTop2Tilemap.ClearAllTiles();
        treeTop3Tilemap.ClearAllTiles();
        groundDecoTilemap.ClearAllTiles();
        FindObjectOfType<EditorCode>().ClearAllRooms();
    }


    internal void PaintSingleCornerWall(Vector2Int position, string binaryType)
    {
        int typeASInt = Convert.ToInt32(binaryType, 2);
        bool outer = false;
        TileBase tile = null;

        if (WallTypesHelper.wallInnerCornerDownLeft.Contains(typeASInt))
        {
            outer = true;
            tile = wallInnerCornerDownLeft;
        }
        else if (WallTypesHelper.wallInnerCornerDownRight.Contains(typeASInt))
        {
            outer = true;
            tile = wallInnerCornerDownRight;
        }
        else if (WallTypesHelper.wallDiagonalCornerDownLeft.Contains(typeASInt))
        {
            
            tile = wallDiagonalCornerDownLeft;
        }
        else if (WallTypesHelper.wallDiagonalCornerDownRight.Contains(typeASInt))
        {
            
            tile = wallDiagonalCornerDownRight;
        }
        else if (WallTypesHelper.wallDiagonalCornerUpRight.Contains(typeASInt))
        {
            tile = wallDiagonalCornerUpRight;
        }
        else if (WallTypesHelper.wallDiagonalCornerUpLeft.Contains(typeASInt))
        {
            tile = wallDiagonalCornerUpLeft;
        }
        else if (WallTypesHelper.wallFullEightDirections.Contains(typeASInt))
        {
            tile = wallFull;
        }
        else if (WallTypesHelper.wallBottmEightDirections.Contains(typeASInt))
        {
            outer = true;
            tile = wallBottom;
        }

        if (tile != null)

            if (outer)
            {
                PaintSingleTile(outerwallTilemap, tile, position, outer);
            } else
            {
                PaintSingleTile(innerwallTilemap, tile, position, outer);
            }
            
    }

    public void PaintDecorations()
    {
        List<Vector2Int> paintableTiles = new List<Vector2Int>();
        var paintable = innerwallTilemap.cellBounds.allPositionsWithin;
        foreach (var pos in paintable)
        {
            if (!(outerwallTilemap.HasTile(pos) || innerwallTilemap.HasTile(pos) || floorTilemap.HasTile(pos)))
            {
                paintableTiles.Add(new Vector2Int(pos.x, pos.y));
            }

        }
        decorationContainer = new GameObject("DecorationContainer").transform;
        PaintDockTiles(ref paintableTiles);
        PaintDecorativeTiles(ref paintableTiles);
        //SpawnSpriteDecoration(ref paintableTiles, ship, UnityEngine.Random.Range(10,25), decorationContainer);
        SpawnGameObjectDecoration(ref paintableTiles, 3f, ship1, UnityEngine.Random.Range(10, 25), decorationContainer);
        SpawnGameObjectDecoration(ref paintableTiles, 3f, ship2, UnityEngine.Random.Range(10, 25), decorationContainer);
        SpawnGameObjectDecoration(ref paintableTiles, 5f, ship3, UnityEngine.Random.Range(10, 25), decorationContainer);
        

    }

    private void SpawnSpriteDecoration(ref List<Vector2Int> paintableTiles, Sprite sprite, int offset, Transform decorationContainer)
    {
        int numofSpawns = (int)(waterTilemap.size.x / offset);

        Vector2Int vec = Vector2Int.zero;
        while (numofSpawns-- > 0)
        {
            int maxretries = 100;
            do
            {
                vec = paintableTiles[UnityEngine.Random.Range(0, paintableTiles.Count)];
                Debug.Log(vec);
            } while (Physics2D.OverlapCircleAll(vec, sprite.bounds.size.x).Length > 0 && maxretries-- > 0);

            if (vec != Vector2Int.zero && maxretries > 0)
            {
                GameObject shipgo = new GameObject("Ship");
                shipgo.layer = LayerMask.NameToLayer("Obstacles");
                shipgo.transform.position = new Vector2(vec.x, vec.y);
                SpriteRenderer spr_ = shipgo.AddComponent<SpriteRenderer>();
                spr_.sprite = ship;
                shipgo.transform.SetParent(decorationContainer);
                paintableTiles.Remove(vec);
            }

        }
    }

    private void SpawnGameObjectDecoration(ref List<Vector2Int> paintableTiles, float size, GameObject decorationObj, int offset, Transform decorationContainer)
    {
        int numofships = (int)(waterTilemap.size.x / offset);

        Vector2Int vec = Vector2Int.zero;
        while (numofships-- > 0)
        {
            int maxretries = 100;
            do
            {
                vec = paintableTiles[UnityEngine.Random.Range(0, paintableTiles.Count)];
            } while (NoDecorationInVicinity(vec,(int) size) && maxretries-- > 0); /*Physics2D.OverlapCircleAll(vec, size).Length > 0*/

            if (vec != Vector2Int.zero && maxretries > 0)
            {
                GameObject deco = Instantiate(decorationObj);
                deco.layer = LayerMask.NameToLayer("Obstacles");
                deco.transform.position = new Vector2(vec.x, vec.y);
                deco.transform.SetParent(decorationContainer);
                //deco.transform.rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360));
                paintableTiles.Remove(vec);
            }

        }

    }

    private bool NoDecorationInVicinity(Vector2Int vec, int size)
    {
        for (int i = -size; i < size; i ++)
        {
            for (int j = -size; j <size; j++)
            {
                if (decorativeTilemap.HasTile(new Vector3Int(vec.x + i , vec.y + j))) {
                    return false;
                }
            }
        }
        return true;
    }

    private void SpawnSingleGameObjectDecoration(Vector2Int position, float size, GameObject decorationObj, Transform decorationContainer)
    {
        if (!Physics2D.OverlapPoint(position))
        {
            GameObject deco = Instantiate(decorationObj);
            deco.layer = LayerMask.NameToLayer("Obstacles");
            deco.transform.position = new Vector2(position.x, position.y);
            deco.transform.SetParent(decorationContainer);
        }
       

    }

    public void PaintDecorativeTiles(ref List<Vector2Int> paintableTiles)
    {
        int decorativePossible = Mathf.Min(paintableTiles.Count, decorationCount);
        while (decorativePossible-- > 0)
        {
            var pos = paintableTiles[UnityEngine.Random.Range(0, paintableTiles.Count)];
            paintableTiles.Remove(pos);
            PaintSingleTile(decorativeTilemap, waterdecoratives[UnityEngine.Random.Range(0, waterdecoratives.Count)], pos, false);
        }
    }


    public void PaintDockTiles(ref List<Vector2Int> paintableTiles)
    {
        int iterations = 25;
        int walklength = 25;
        for (int i = 0; i < iterations; i++)
        {
            Vector2Int startPos = paintableTiles[UnityEngine.Random.Range(0, paintableTiles.Count)];
            RunSimpleRandomWalk(startPos, paintableTiles, walklength);
            
        }

    }

    private void RunSimpleRandomWalk(Vector2Int startPos, List<Vector2Int> randomPositions, int walklength)
    {

        string prevdirection = "";
        Dictionary<Vector2Int, TileBase> dockPath = new Dictionary<Vector2Int, TileBase>();
        GameObject selectedStructure = (prevdirection == "left" || prevdirection == "right") ? sidestructures[UnityEngine.Random.Range(0, sidestructures.Count)] : frontstructures[UnityEngine.Random.Range(0, frontstructures.Count)];
        var _col = selectedStructure.GetComponent<Collider2D>();
        while (walklength-- > 0)
        {
            Vector2Int left = (startPos += Vector2Int.left);
            Vector2Int right = (startPos += Vector2Int.right);
            Vector2Int up = (startPos += Vector2Int.up);
            Vector2Int down = (startPos += Vector2Int.down);
            randomPositions.Remove(startPos);
            HashSet<Vector2Int> allDirs = new HashSet<Vector2Int>() { left, right, up, down };
            allDirs.IntersectWith(randomPositions);
            if (allDirs.Count == 0)
            {
                if (dockPath.Count >= 10)
                {
                    foreach (Vector2Int vec in dockPath.Keys)
                    {
                        PaintSingleTile(dockTilemap, dockPath[vec], vec, false);
                    }

                    SpawnStructureOnDir(startPos, prevdirection, ref selectedStructure, ref _col);
                }
                return;
            }
            else
            {
                Vector2Int selectedPos = allDirs.ElementAt(UnityEngine.Random.Range(0, allDirs.Count));
                
                if (prevdirection == "")
                {
                    SpawnSingleGameObjectDecoration(startPos, _col.bounds.size.magnitude, selectedStructure, decorationContainer);
                }

                else if (selectedPos == left && prevdirection == "left" || selectedPos == right && prevdirection == "right")
                {
                    dockPath[startPos] = horizontalplank;

                }
                else if (selectedPos == left && prevdirection == "up" || selectedPos == down && prevdirection == "right")
                {
                    dockPath[startPos] = rdcornerplank;

                }
                else if (selectedPos == left && prevdirection == "down" || selectedPos == up && prevdirection == "right")
                {
                    dockPath[startPos] = rucornerplank;

                }
                else if (selectedPos == right && prevdirection == "up" || selectedPos == down && prevdirection == "left")
                {

                    dockPath[startPos] = ldcornerplank;

                }
                else if (selectedPos == right && prevdirection == "down" || selectedPos == up && prevdirection == "left")
                {
                    dockPath[startPos] = lucornerplank;

                }
                else if (selectedPos == down && prevdirection == "down" || selectedPos == up && prevdirection == "up")
                {
                    dockPath[startPos] = verticalplank;
                }
                prevdirection = selectedPos == left ? "left" : selectedPos == right ? "right" : selectedPos == up ? "up" : "down";
                startPos = selectedPos;

            }

        }

        foreach (Vector2Int vec in dockPath.Keys)
        {
            PaintSingleTile(dockTilemap, dockPath[vec], vec, false);
        }

        SpawnStructureOnDir(startPos, prevdirection, ref selectedStructure, ref _col);

    }

    private void SpawnStructureOnDir(Vector2Int startPos, string prevdirection, ref GameObject selectedStructure, ref Collider2D _col)
    {
        if (prevdirection == "left" || prevdirection == "right")
        {
            selectedStructure = sidestructures[UnityEngine.Random.Range(0, sidestructures.Count)];
            _col = selectedStructure.GetComponent<Collider2D>();
        }
        SpawnSingleGameObjectDecoration(startPos, _col.bounds.size.magnitude, selectedStructure, decorationContainer);
    }

}
