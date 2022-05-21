using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R1_Mgr : RoomManager
{
    
    private void Awake()
    {
        this.conditions = new List<string>();
        this.conditions.Add("booze");
        this.conditions.Add("key");
    }

    private void Update()
    {
        RoomChecker();
        CheckDialogue();
    }

    private void RoomChecker()
    {
        //check condition 1
        if (!conditions.Contains("booze"))
        {
            GameObject dd = GameObject.Find("NPC");
            
            if (dd != null)
            {
                dd.GetComponentInChildren<DialogueDetection>().Fulfilled();
                
            }
        }

        //check condition 2 and onwards;
        if (!conditions.Contains("key"))
        {
            GameObject.FindObjectOfType<UnlockableDoor>().UnlockDoor();
        }

        

    }



}
