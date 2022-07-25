using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractDungeonGenerator : MonoBehaviour
{
    [SerializeField]
    protected TilemapVisualizer tilemapVisualizer = null;
    [SerializeField]
    protected Vector2Int startPosition = Vector2Int.zero;

    [Header("Random Seed")]
    public int currentSeed = -1;

    public void GenerateDungeon()
    {
        tilemapVisualizer.Clear();
        TerrainGenerator.instance.SetSeed(currentSeed);
        RoomDesign.instance.SetSeed(currentSeed);
        TerrainGenerator.instance.ClearAllTerrainObjects();
        TerrainGenerator.instance.ClearUnwalkableTiles();
        RunProceduralGeneration();
    }

    protected abstract void RunProceduralGeneration();

        
}
