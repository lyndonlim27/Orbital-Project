using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseDoorBehaviour : BreakableDoorBehaviour
{
    protected override void Awake()
    {
        base.Awake();
        spriteRenderer.sortingOrder = 10;
        CreateDoorData();
    }

    protected override void Start()
    {
        base.Start();
        textCanvas.SetActive(true);
    }

    protected override void Update()
    {

    }

    private void CreateDoorData()
    {
        doorData = ScriptableObject.CreateInstance<DoorData>();
        doorData._name = "Enter";
        doorData._type = EntityData.TYPE.DOOR;
        doorData.sprite = spriteRenderer.sprite;
        doorData.minDist = 1.5f;
    }

    public override void Defeated()
    {
        animator.SetBool("HouseDoor",true);
        GetComponent<Collider2D>().enabled = false;
        textCanvas.SetActive(false);

    }

}
