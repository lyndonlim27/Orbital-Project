using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTextLogic : TextLogic
{
    private RoomManager roomManager;

    protected override bool CheckInternalInput()
    {
        return true;
    }

    protected override void GenerateNewWord()
    {
        remainingword = currentword;
    }
}
