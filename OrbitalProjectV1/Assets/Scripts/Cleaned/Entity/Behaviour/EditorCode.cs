using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Video;

[ExecuteInEditMode]
public class EditorCode : MonoBehaviour
{

    public void ClearAllRooms()
    {
        
        GameObject RoomContainer = GameObject.Find("RoomsContainer");
        if (RoomContainer != null)
        {
            DestroyImmediate(RoomContainer);
            
        }
    }
}
