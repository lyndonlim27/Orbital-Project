using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator),typeof(Collider2D))]
public class DoorBehaviour : EntityBehaviour
{
    private Animator animator;
    public bool unlocked;
    private HashSet<RoomManager> roomManagers;
    private List<string> conditions;
    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        roomManagers = new HashSet<RoomManager>();
        Debug.Log(animator);
    }

    private void Start()
    {
        unlocked = false;
    }

    private void Update()
    {
        if (unlocked)
        {
            UnlockDoor();
        }
        else
        {
            LockDoor();
        }

    }

    public override void Defeated()
    {
        
    }

    public override EntityData GetData()
    {
        throw new System.NotImplementedException();
    }

    public override void SetEntityStats(EntityData stats)
    {
        throw new System.NotImplementedException();
    }

    public void UnlockDoor()
    {
       
        animator.SetBool(gameObject.name.Substring(0, 4), true);
        GetComponent<Collider2D>().enabled = false;
        
        
    }

    public void LockDoor()
    {
        
        animator.SetBool(gameObject.name.Substring(0, 4), false);
        GetComponent<Collider2D>().enabled = true;
    }
        


    protected override IEnumerator FadeOut()
    {
        for (float f = 1f; f > 0 ; f -= 0.05f)
        {
            Color c = spriteRenderer.material.color;
            c.a = f;
            spriteRenderer.material.color = c;
            yield return new WaitForSeconds(0.05f);
        }
        inAnimation = false;
    }

    protected IEnumerator FadeIn()
    {
        for (float f = 0f; f < 1f; f += 0.05f)
        {
            Color c = spriteRenderer.material.color;
            c.a = f;
            spriteRenderer.material.color = c;
            yield return new WaitForSeconds(0.05f);
        }

        inAnimation = false;
    }

    public void SetRoomControllers(RoomManager room)
    {
        roomManagers.Add(room);
    }

    public HashSet<RoomManager> GetRoomManagers()
    {
        return roomManagers;
    }
}
