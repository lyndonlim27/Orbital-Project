using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCRoom1 : RoomManager
{

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<PopUpSettings>(true).PopUpSettingsActive();
        FindObjectOfType<SubMenu>().ControlMenuActive();
    }

    // Update is called once per frame
    void Update()
    {
        CheckDialogue();
    }

    public override void FulfillCondition(string key)
    {
        Debug.Log(key);
        conditions.Remove(key);
    }

    protected override void RoomChecker()
    {
        throw new System.NotImplementedException();
    }

    protected override void UnfulfillCondition(string key)
    {
        throw new System.NotImplementedException();
    }


}
