using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCRoom1 : RoomManager
{
    private TypingTestTL _tl;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<PopUpSettings>(true).PopUpSettingsActive();
        FindObjectOfType<SubMenu>().ControlMenuActive();
        _tl = GameObject.FindObjectOfType<TypingTestTL>(true);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        CheckRunningEvents();
        RoomChecker();

    }

    public override void FulfillCondition(string key)
    {
        conditions.Remove(key);
        _tl.transform.parent.parent.gameObject.SetActive(true);

    }

    protected override void RoomChecker()
    {

        if (conditions.Count == 0 && !_tl.gameObject.activeInHierarchy)
        {
            foreach (GameObject door in doors)
            {
                door.GetComponent<Animator>().SetBool("Open", true);
                door.GetComponent<Collider2D>().enabled = false;
            }
        }
    }

    public override void UnfulfillCondition(string key)
    {
        throw new System.NotImplementedException();
    }

    IEnumerator CountDown()
    {
        yield return new WaitForSecondsRealtime(1f);


    }

}
