using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(_GameManager), true)]
public class GameManagersGenerator : Editor
{
    _GameManager generator;
    // Start is called before the first frame update
    private void Awake()
    {
        generator = (_GameManager)target;
    }

    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Generate Managers"))
        {
            generator.GenerateManagers();
        }

        if (GUILayout.Button("Spawn Player"))
        {
            generator.SpawnPlayer();
        }

        if (GUILayout.Button("SetPlayerPosition"))
        {
            //generator.SetPlayerPosition();
        }

    }
}
