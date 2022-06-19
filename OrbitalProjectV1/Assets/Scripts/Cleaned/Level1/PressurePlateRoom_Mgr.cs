using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateRoom_Mgr : RoomManager
{   
    public DoorBehaviour pressureSwitchDoor;

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

    }

    protected override void RoomChecker()
    {
        if (activated)
        {
            Debug.Log(pressureitems.Count);
            
            if (pressureitems.Count != 0 && pressureitems.TrueForAll(item => item.IsOn()))
            {
                Debug.Log("entered");
                float duration = pressureitems[0].data.duration;
                StartCoroutine(OpenDoorsTemp(duration));
            }

            if (conditions.Count == 0 && player.GetCurrentRoom() != this)
            {
                pressureSwitchDoor.unlocked = true;
            }
        }
        
        if (CanProceed())
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

   private IEnumerator OpenDoorsTemp(float duration)
    {
        pressureSwitchDoor.unlocked = true;
        yield return duration;
        pressureSwitchDoor.unlocked = false;
    }

}
