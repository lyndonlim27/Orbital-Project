using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R1_Mgr : RoomManager
{
    private DialogueDetection dd;
    private UnlockableDoor door;
    protected override void Awake()
    {
        base.Awake();
        RoomManager.conditions.Add("key");
        dd = GameObject.Find("NPC").GetComponentInChildren<DialogueDetection>();
        door = GameObject.FindObjectOfType<UnlockableDoor>();

    }

    private void Start()
    {
          
    }

    private void Update()
    {
        if (!this.activated)
        {
            return;
        }
        
        RoomChecker();
        CheckDialogue();
    }

    private void RoomChecker()
    {
        //check condition 1

        if (!RoomManager.conditions.Contains("booze"))
        {
            
            if (dd != null)
            {
                dd.Fulfilled();
                
            }
        }

        //check condition 2 and onwards;
        if (!RoomManager.conditions.Contains("key"))
        {
            door.UnlockDoor();
        }

        

    }

    protected override void InitializeRoom()
    {
        base.InitializeRoom();
        //maybe wanna do other stuffs; 
    }

}


