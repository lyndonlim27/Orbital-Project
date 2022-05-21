using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockableDoor : MonoBehaviour
{

    private bool unlocked;
    private BoxCollider2D[] cols;
    private Quaternion initialstate;

    private void Awake()
    {
        unlocked = false;
        cols = GetComponentsInChildren<BoxCollider2D>();
        //initialstate = this.transform.rotation;
    }

    private void Update()
    {
        if (unlocked == true)
        {
            transform.rotation = Quaternion.Euler(0, 45f, 0);
        } else
        {
            transform.rotation = Quaternion.identity;
        }

        TriggerColliders();
        
    }

    public void TriggerColliders()
    {
        foreach (BoxCollider2D col in cols)
        {
            col.enabled = !unlocked;
        }
    }

    public void UnlockDoor()
    {
        unlocked = true;
        
        
    }

    public void LockDoor()
    {
        unlocked = false;


    }
}
