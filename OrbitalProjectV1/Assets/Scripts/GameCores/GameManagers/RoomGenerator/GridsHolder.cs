using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridsHolder : MonoBehaviour
{
    Dictionary<string, Tilemap> allGrids;
    public static GridsHolder instance { get; private set; }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        allGrids = new Dictionary<string, Tilemap>();
        Tilemap[] tilemaps = GetComponentsInChildren<Tilemap>();
        foreach (Tilemap tilemap in tilemaps)
        {
            allGrids.Add(tilemap.name, tilemap);
        }
    }

    public Tilemap GetTilemap(string tilemapname)
    {
        return allGrids[tilemapname];
    }
}
