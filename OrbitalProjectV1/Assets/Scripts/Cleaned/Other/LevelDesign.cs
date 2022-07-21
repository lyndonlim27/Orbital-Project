using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class LevelDesign : ScriptableObject
{
    //didnt really use, just for self awareness in inspector mode.
    public enum ROOM_DESIGN
    {
        FARM,
        PLANTATION,
        FOREST,
        FKNIGHT_ROOM,
        WATERMAGE_ROOM,
        BLADEKEEPER_ROOM,
        GROUNDMONK_ROOM,
        WINDGUY_ROOM,
    }
    public ROOM_DESIGN room_design;
    public TileBase floorTile,grassTile;
    [Header("Wall Tiles")]
    public TileBase luwall, ldwall, ruwall, rdwall, vertwall, horizwall, allardwall, topwall, bottomwall, leftwall, rightwall;
    public TileBase lucorner, rucorner, ldcorner, rdcorner;
    public GameObject[] terrainObjects;
    public TileBase[] intterrainDecorations;
    public TileBase[] extterrainDecorations;
    public float frequency;
    public int offset;

    [Range(0,1)]
    public float normalizer;
}
