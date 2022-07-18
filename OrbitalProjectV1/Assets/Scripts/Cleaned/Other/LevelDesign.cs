using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class LevelDesign : ScriptableObject
{
    public enum ROOM_DESIGN
    {
        FARM,
        SWAMP,
        PLANTATION,
        FOREST,
        MARSH,
    }
    public ROOM_DESIGN room_design;
    public TileBase floorTile,grassTile;
    [Header("Wall Tiles")]
    public TileBase luwall, ldwall, ruwall, rdwall, vertwall, horizwall, allardwall;
    public GameObject[] terrainObjects;
    public TileBase[] intterrainDecorations;
    public TileBase[] extterrainDecorations;
    public float frequency;
    public int offset;

    [Range(0,1)]
    public float normalizer;
}
