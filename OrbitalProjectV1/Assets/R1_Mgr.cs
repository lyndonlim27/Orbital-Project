using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R1_Mgr : RoomManager
{
    public override void FulfillCondition(string key)
    {
        this.conditions[key] = true;
    }

    private void Awake()
    {
        this.conditions = new Dictionary<string, bool>();
        this.conditions.Add("booze", false);
        this.conditions.Add("key", false);
    }

    private void Update()
    {
        RoomChecker();
        CheckDialogue();
    }

    private void RoomChecker()
    {
        //check condition 1
        if (conditions["booze"])
        {
            GameObject.Find("NPC").GetComponentInChildren<DialogueDetection>().Fulfilled();
        }

        //check condition 2 and onwards;
        if (conditions["key"])
        {
            GameObject.FindObjectOfType<UnlockableDoor>().UnlockDoor();
        }

        

    }



}
