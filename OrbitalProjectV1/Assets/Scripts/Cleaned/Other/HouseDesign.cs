using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class HouseDesign : MonoBehaviour
{
    [SerializeField]
    Tilemap interiorTilemap, exteriorTilemap, nearWallTilemap;
    RoomManager currRoom;
    float dooroffset;
    DoorBehaviour door;

    protected virtual void Awake()
    {
        currRoom = GetComponentInParent<RoomManager>();
        interiorTilemap = GameObject.Find("InteriorTilemap").GetComponent<Tilemap>();
        exteriorTilemap = GameObject.Find("ExteriorTilemap").GetComponent<Tilemap>();
        nearWallTilemap = GameObject.Find("NearWallTilemap").GetComponent<Tilemap>();
        door = transform.parent.GetComponentInChildren<HouseDoorBehaviour>();
        dooroffset = 1.5f;
    }

    public void SpawnDecorations(TileBase[] decorations, bool interior, Vector2Int minArea, Vector2Int maxArea, LayerMask layerMask)
    {
        int rand = Random.Range(5, 7);
        if (decorations.Length == 0)
        {
            Debug.LogError("No exterior decorations assigned");
        }
        for (int i = 0; i < rand; i++)
        {

            var selectedDeco = decorations[Random.Range(0, decorations.Length - 1)];
            Vector3Int randomPoint = (Vector3Int) currRoom.GetRandomTilePointGivenPoints(minArea - Vector2Int.one , maxArea + Vector2Int.one, interior, layerMask);
            if (!PointNearDoor(randomPoint))
            {
                if (interior)
                {
                    PaintInteriorTiles(selectedDeco, randomPoint);
                }
                else
                {
                    PaintExteriorTiles(CheckIfNearWall(randomPoint), selectedDeco, randomPoint);
                }
            }
            
        }

    }

    private bool PointNearDoor(Vector3Int randompoint)
    {
        BoxCollider2D _col = door.GetComponent<BoxCollider2D>();
        float dist = Vector3.Distance(randompoint, new Vector3(_col.bounds.center.x,_col.bounds.min.y));
        bool insideRange = dist <= (_col.bounds.size.magnitude / 2) + dooroffset;
        if (insideRange)
        {
            return true;
        }
    
        return false;
    }

    private void PaintExteriorTiles(bool nearWall, TileBase tile, Vector3Int pos)
    {
        if (nearWall)
        {
            nearWallTilemap.SetTile(pos, tile);
        } else
        {
            exteriorTilemap.SetTile(pos, tile);
        }
    }

    private void PaintInteriorTiles(TileBase tile, Vector3Int pos)
    {
        interiorTilemap.SetTile(pos,tile);
    }

    private bool CheckIfNearWall(Vector3Int point)
    {
        return Physics2D.OverlapCircle((Vector2Int) point, 2f, LayerMask.GetMask("HouseExterior"));
    }
}
