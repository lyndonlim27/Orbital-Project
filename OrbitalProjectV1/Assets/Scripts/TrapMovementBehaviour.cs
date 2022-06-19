using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapMovementBehaviour : EntityBehaviour
{
    Vector2 originalpos;
    bool forward;
    [SerializeField] private TrapData trapData;
    private Animator animator;
    private float dist;

    public override void Defeated()
    {
        
    }

    public override EntityData GetData()
    {
        return trapData;
    }

    public override void SetEntityStats(EntityData stats)
    {
        trapData = stats as TrapData;
    }

    private void Start()
    {
        currentRoom = GetComponentInParent<RoomManager>();
        animator = GetComponent<Animator>();
        originalpos = transform.position;
        forward = transform.localScale.x == 1;
        animator.SetBool(string.Format("SawTrap{0}", Random.Range(1, 4)), true);
        inAnimation = true;
        //Debug.Log("Roomsize = " + currentRoom.GetRoomAreaBounds().size.y);
        dist = 2.5f;
    }

    private void Update()
    {
        if (Vector2.Distance(originalpos,transform.position) >= dist)
        {
            forward = !forward;
        }

        if (forward)
        {
            transform.position += transform.right * Time.deltaTime;
        }
        else
        {
            transform.position -= transform.right * Time.deltaTime;
        }
    }
}
