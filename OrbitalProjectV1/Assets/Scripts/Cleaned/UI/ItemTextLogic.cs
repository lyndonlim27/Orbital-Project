using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTextLogic : TextLogic
{
    private RoomManager roomManager;

    protected override void Start()
    {
        base.Start();
        
    }

    protected override void OnEnable()
    {
        if (parent != null)
        {
            EntityData entityData = parent.GetData();
            if (entityData != null)
            {
                currentword = entityData._name.ToLower();
            }

        }
        
        base.OnEnable();
        
    }
    protected override bool CheckInternalInput()
    {
        return true;
    }

    public void ResetWord()
    {
        GenerateNewWord();
    }

    protected override void GenerateNewWord()
    {
        Debug.Log("Entered?" + parent.GetData()._name.ToLower());
        this.remainingword = parent.GetData()._name.ToLower();
    }

    protected override void PerformAction()
    {
        parent.GetComponent<EntityBehaviour>().Defeated();
    }
}
