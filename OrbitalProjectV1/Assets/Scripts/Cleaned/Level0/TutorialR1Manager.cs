using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialR1Manager : RoomManager
{
    private DialogueDetection dd;
    private UnlockableDoor door1;
    protected override void Awake()
    {
        base.Awake();
        dd = GameObject.Find("NPC").GetComponentInChildren<DialogueDetection>();
        door1 = GameObject.Find("UnlockableDoor1").GetComponent<UnlockableDoor>();
        player = GameObject.Find("Player").GetComponent<Player>();

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

    protected override void RoomChecker()
    {
        dd.Fulfilled();
        Debug.Log(conditions);
        if (conditions.Contains("cannon"))
        {
            player.PickupItem("Cannon");
            door1.UnlockDoor();
            Debug.Log("GOOSE");
        }

        if (conditions.Contains("laser"))
        {
            player.PickupItem("Laser");
            door1.UnlockDoor();

        }


    }



    protected override void FulfillCondition(string key)
    {
        throw new System.NotImplementedException();
    }

    protected override void UnfulfillCondition(string key)
    {
        throw new System.NotImplementedException();
    }


}
