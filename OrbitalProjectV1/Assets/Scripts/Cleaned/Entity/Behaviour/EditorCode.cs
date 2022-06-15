using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EditorCode : MonoBehaviour
{

    private void OnEnable()
    {
        DoorBehaviour[] doors = FindObjectsOfType<DoorBehaviour>();
        foreach(DoorBehaviour door in doors)
        {
            door.tag = "Door";
        }
    }
}
