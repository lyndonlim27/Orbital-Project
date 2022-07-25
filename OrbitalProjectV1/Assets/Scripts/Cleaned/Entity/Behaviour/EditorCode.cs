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
        GameObject DecoContainer = GameObject.Find("DecorationContainer");
        if (RoomContainer != null)
        {
            DestroyImmediate(RoomContainer);
            
        }

        if (DecoContainer != null)
        {
            DestroyImmediate(DecoContainer);
        }
    }
}
