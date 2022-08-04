using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using GameManagement.RoomGenerator;

namespace GameEditorCodes
{
    [CustomEditor(typeof(AbstractDungeonGenerator), true)]
    public class RandomDungeonGeneratorEditor : Editor
    {
        AbstractDungeonGenerator generator;

        private void Awake()
        {
            generator = (AbstractDungeonGenerator)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Create Dungeon"))
            {
                generator.GenerateDungeon();
            }

        }

    }
}
