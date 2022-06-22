using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class HybridRoom_Mgr : RoomManager
{
    // Start is called before the first frame update
    [SerializeField] private GameObject traps;

    protected override void Awake()
    {
        roomtype = ROOMTYPE.HYBRID_ROOM;
        base.Awake();
        
    }
    void Start()
    {
        
    }

    protected override void Update()
    {
        base.Update();
        RoomChecker();
        Debug.Log(conditions.Count);
        if (activated)
        {
            if (traps != null)
            {
                traps.SetActive(true);
            }
            
        }
        CheckRunningEvents();
    }

}
