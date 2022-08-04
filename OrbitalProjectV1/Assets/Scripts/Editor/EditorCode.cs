using UnityEngine;

namespace GameEditorCodes
{
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
}
