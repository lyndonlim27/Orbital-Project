using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R2_Mgr : RoomManager
{
    private bool added;
    private PressureSwitchBehaviour[] pressureSwitchBehaviours;

    protected override void Awake()
    {
        roomtype = ROOMTYPE.FIGHTING_ROOM;
        base.Awake();

    }

    protected void Start()
    {
        
    }

    protected override void Update()
    {
        base.Update();
        RoomChecker();
        CheckRunningEvents();
        if (player.GetCurrentRoom() != this && player.GetCurrentRoom() != null)
        {
            items.RemoveAll((a) => a.GetType() == typeof(PressureSwitchBehaviour));
        }

    }

    protected override void RoomChecker()
    {
        
        if (activated && conditions.Count == 0 && items.Count != 0)
        {
            doors[0].unlocked = true;
        } 

        if (CheckEnemiesDead())
        {
            foreach(DoorBehaviour door in doors)
            {
                door.unlocked = true;
            }
            this.enabled = false;
        } 

    }

    protected override void CheckRunningEvents()
    {
        if (dialMgr.playing || popUpSettings.gameObject.activeInHierarchy)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }

}
