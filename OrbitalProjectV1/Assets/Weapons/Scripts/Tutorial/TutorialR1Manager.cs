using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialR1Manager : RoomManager
{
    private DialogueDetection dd;
    private UnlockableDoor door1;
    private Player player;
    protected override void Awake()
    {
        base.Awake();
     //   RoomManager.conditions.Add("cannon");
        this.maxEnemies = 0;
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

    private void RoomChecker()
    {
        dd.Fulfilled();
        Debug.Log(RoomManager.conditions);
        if (!RoomManager.conditions.Contains("cannon"))
        {
            player.PickupItem("Cannon");
            door1.UnlockDoor();
            Debug.Log("GOOSE");
        }

        if (!RoomManager.conditions.Contains("laser"))
        {
            player.PickupItem("Laser");
            door1.UnlockDoor();

        }


    }

    protected override void InitializeRoom()
    {
        base.InitializeRoom();
        //maybe wanna do other stuffs; 
    }

}
