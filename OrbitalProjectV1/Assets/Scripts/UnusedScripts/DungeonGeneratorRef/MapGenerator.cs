using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] int seed;

    [Header("Tiles")]
    Tilemap tilemap;
    [SerializeField] TileBase[] tileBases;


    [Header("Dimensions")]
    [SerializeField] int width;
    [SerializeField] int height;

    [Header("Smoothing")]
    [SerializeField] float noiseFactor;

    private void Awake()
    {
        
    }


    /**
     * Rendering of Map
     */
    public static void RenderMap(int[,] map, Tilemap tilemap, TileBase tile)
    {
        //Clear the map (ensures we dont overlap)
        tilemap.ClearAllTiles();
        //Loop through the width of the map
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            //Loop through the height of the map
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                // 1 = tile, 0 = no tile
                if (map[x, y] == 1)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), tile);
                }
            }
        }
    }
}
