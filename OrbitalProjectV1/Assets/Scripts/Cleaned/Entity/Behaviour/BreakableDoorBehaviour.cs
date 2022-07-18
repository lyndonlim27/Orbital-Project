using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableDoorBehaviour : DoorBehaviour
{
    protected GameObject textCanvas;
    protected WeaponDescription weaponDataDisplay;
    [SerializeField] protected DoorData doorData;

    protected override void Awake()
    {
        base.Awake();   
        textCanvas = GetComponentInChildren<TextLogic>().gameObject;
        weaponDataDisplay = GetComponentInChildren<WeaponDescription>(true);
        weaponDataDisplay.gameObject.SetActive(false);
        

    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GetComponent<BoxCollider2D>().size = spriteRenderer.sprite.bounds.size;
    }

    protected override void Start()
    {
        base.Start();
        LockDoor();

    }

    public override void LockDoor()
    {
        textCanvas.SetActive(false);
    }

    public override void UnlockDoor()
    {
        textCanvas.SetActive(true);
    }

    protected override void Update()
    {
        if (unlocked)
        {
            UnlockDoor();
        } 
    }


    public override void Defeated()
    {
        Destroy(this.gameObject);
    }

    public override EntityData GetData()
    {
        return doorData;
    }

    public override void SetEntityStats(EntityData stats)
    {
        DoorData doorStats = (DoorData)stats;
        if (doorStats == null)
        {
            Debug.Log("Stats is null");
        } else
        {
            doorData = doorStats;
        }
    }
}

