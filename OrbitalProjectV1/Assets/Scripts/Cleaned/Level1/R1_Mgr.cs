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
        CheckRunningEvents();
    }

    protected override void RoomChecker()
    {
        //check condition 1

        if (!conditions.Contains("booze"))
        {
            
            if (dd != null)
            {
                dd.Fulfilled();
                
            }
        }

        //check condition 2 and onwards;
        if (!conditions.Contains("key"))
        {
         //   door.UnlockDoor();
        }

        

    }


    public override void FulfillCondition(string key)
    {
        throw new System.NotImplementedException();
    }

    protected override void UnfulfillCondition(string key)
    {
        throw new System.NotImplementedException();
    }
}


