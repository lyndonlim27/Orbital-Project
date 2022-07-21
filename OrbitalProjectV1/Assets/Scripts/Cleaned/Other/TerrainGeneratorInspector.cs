using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(TerrainGenerator), true)]
public class TerrainGeneratorInspector : Editor
{
    TerrainGenerator generator;
    private void Awake()
    {
        generator = (TerrainGenerator)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Generate Terrain"))
        {
            generator.GenerateTerrain();
        }

        if (GUILayout.Button("Generate TerrainType2"))
        {
            generator.GenerateTerrainType2();
        }

        if (GUILayout.Button("Load Level Data"))
        {
            generator.LoadLevel();
        }
    }
}
