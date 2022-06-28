using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator),typeof(Collider2D))]
public class DoorBehaviour : EntityBehaviour
{
    private Animator animator;
    public bool unlocked;
    private HashSet<RoomManager> roomManagers;
    private AudioClip unlockWoodenClip;
    private AudioClip lockWoodenClip;
    private AudioClip unlockSteelClip;
    private AudioClip unlockTutorialClip;

    private List<string> conditions;
    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        roomManagers = new HashSet<RoomManager>();
        unlockWoodenClip = Resources.Load("Sounds/Door/UnlockWooden") as AudioClip;
        lockWoodenClip = Resources.Load("Sounds/Door/LockWooden") as AudioClip;
        unlockSteelClip = Resources.Load("Sounds/Door/UnlockSteel") as AudioClip;
        unlockTutorialClip = Resources.Load("Sounds/Door/UnlockTut") as AudioClip;
        audioSource.maxDistance = 20f;
        audioSource.pitch = 0.6f;
        audioSource.volume = 0.05f;
    }

    private void Start()
    {
        unlocked = false;
    }

    private void Update()
    {
        if (unlocked)
        {

            CheckDoorUnlockedSound();
            UnlockDoor();
             
            
            //CheckDoorUnlockedSound();

        }
        else 
        {
            CheckDoorLockedSound();
            LockDoor();
            
            
            
            
        }

    }

    private void CheckDoorUnlockedSound()
    {
        if (!animator.GetBool(gameObject.name.Substring(0, 4)))
        {
            UnlockAudio();
        }
    }

    private void CheckDoorLockedSound()
    {
        if (animator.GetBool(gameObject.name.Substring(0, 4)))
        {
            LockAudio();
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
        


    public override IEnumerator FadeOut()
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

    public void LockAudio()
    {
        string doorname = this.name.Substring(0, 2);
        switch (doorname)
        {

            case ("D1"):
            case ("D3"):
                audioSource.clip = unlockSteelClip;
                audioSource.Play();
                break;
            case ("D2"):
                audioSource.clip = lockWoodenClip;
                audioSource.Play();
                break;
            case ("T1"):
                audioSource.clip = unlockTutorialClip;
                audioSource.Play();
                break;
        }
    }

    public void UnlockAudio()
    {
        string doorname = this.name.Substring(0, 2);
        switch (doorname)
        {
            case ("D1"):
            case ("D3"):
                audioSource.clip = unlockSteelClip;
                audioSource.Play();
                break;
            case ("D2"):
                audioSource.clip = unlockWoodenClip;
                audioSource.Play();
                break;
            case ("T1"):
                audioSource.clip = unlockTutorialClip;
                audioSource.Play();
                break;
        }
    }

    public HashSet<RoomManager> GetRoomManagers()
    {
        return roomManagers;
    }
}
