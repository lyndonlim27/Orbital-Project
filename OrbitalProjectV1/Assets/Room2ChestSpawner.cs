using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room2ChestSpawner : MonoBehaviour
{
    private PressurePlateRoom_Mgr room;

    private void Start()
    {
        room = GetComponentInParent<PressurePlateRoom_Mgr>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("PlayerStealth"))
        {
            
            room.items.ForEach(item => item.enabled = item.GetType() != typeof(PressureSwitchBehaviour));
            room.conditions.RemoveWhere(item => item.GetType() == typeof(PressureSwitchBehaviour));
        }
    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player") || collision.CompareTag("PlayerStealth"))
    //    {
    //        room.pressureSwitchDoor.LockDoor();
    //    }
    //}

    //private IEnumerator()
    //{

    //}


}
