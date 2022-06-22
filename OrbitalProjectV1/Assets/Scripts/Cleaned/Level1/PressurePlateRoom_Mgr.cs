using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateRoom_Mgr : RoomManager
{   
    public DoorBehaviour pressureSwitchDoor;
    private bool spawned;

    protected override void Awake()
    {
        roomtype = ROOMTYPE.FIGHTING_ROOM;
        base.Awake();

    }

    protected void Start()
    {
        spawned = false;
        
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
           
            
            if (pressureitems.Count != 0 && pressureitems.TrueForAll(item => item.IsOn()))
            {
                Debug.Log("entered");
                float duration = pressureitems[0].data.duration;
                StartCoroutine(OpenDoorsTemp(duration));
            }

            Debug.Log(conditions.Count);
            foreach(string s in conditions)
            {
                Debug.Log(s);
            }
            if (conditions.Count == 0 && player.GetCurrentRoom() != this && !spawned)
            {
                spawnlater.ForEach(spawn =>
                {
                    EntityData ed = Instantiate(spawn);
                    ed.spawnAtStart = true;
                    SpawnObject(ed);
                });
                //SpawnObjects(spawnlater.ToArray());
                pressureSwitchDoor.unlocked = true;
                spawned = true;
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
        pressureSwitchDoor.unlocked = conditions.Count == 0;
    }

}
