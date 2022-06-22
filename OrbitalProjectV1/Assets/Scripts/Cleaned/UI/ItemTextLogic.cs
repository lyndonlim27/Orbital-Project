using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTextLogic : TextLogic
{
    private RoomManager roomManager;

    protected override void Start()
    {
        base.Start();
        currentword = parent.GetData()._name.ToLower();
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
        this.remainingword = parent.GetData()._name.ToLower();
    }

    protected override void PerformAction()
    {
        parent.GetComponent<ItemWithTextBehaviour>().Defeated();
    }
}
