using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Animator),typeof(Collider2D), typeof(AudioSource))]
public class DoorBehaviour : EntityBehaviour
{
    public bool unlocked;
    protected Animator animator;
    private HashSet<RoomManager> roomManagers;
    private AudioClip unlockWoodenClip;
    private AudioClip lockWoodenClip;
    private AudioClip unlockSteelClip;
    private AudioClip unlockTutorialClip;

    private Tilemap terrainWallTilemap;
    private bool removed;

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

        //Collider2D col = Physics2D.OverlapCircle(transform.position, 0.01f, LayerMask.GetMask("Obstacles"));
        //if (col.transform != transform)
        //{
        //    //Destroy(this.gameObject);
        //}
    }

    protected virtual void Start()
    {
        isDead = false;
        unlocked = false;
        removed = false;

    }

    public void RemoveTerrainWalls()
    {
        bool terraingenerated = _GameManager.allTerrainsGenerated;
        if (terraingenerated && !removed)
        {
            
            removed = true;
            TerrainGenerator _tergen = TerrainGenerator.instance;
            Debug.Log("This is terraingen" + _tergen);
            if (_tergen == null)
            {
                return;
            }
            else
            {
                terrainWallTilemap = _tergen.GetTerrainWall();
                Vector3Int currentpos = Vector3Int.FloorToInt(transform.position);
                Debug.Log("This is current position" + currentpos);
                RecursiveRemovalofTerrainWalls(currentpos);
            }
        }
    }

    protected virtual void Update()
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

    protected virtual void CheckDoorUnlockedSound()
    {
        if (!animator.GetBool(gameObject.name.Substring(0, 4)))
        {
            UnlockAudio();
        }
    }

    protected virtual void CheckDoorLockedSound()
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
        return null;
    }

    public override void SetEntityStats(EntityData stats)
    {
        return;
    }

    public virtual void UnlockDoor()
    {
        animator.SetBool(gameObject.name.Substring(0, 4), true);
        GetComponent<Collider2D>().enabled = false;
        
        
    }

    public virtual void LockDoor()
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

    private void RecursiveRemovalofTerrainWalls(Vector3Int pos)
    {

        bool stop = false;
        for (int i = -1; i < 1 && i != 0; i++)
        {
            var pos1 = pos + new Vector3Int(i, 0);
            var pos2 = pos + new Vector3Int(0, i);
            if (!terrainWallTilemap.HasTile(pos1) || !terrainWallTilemap.HasTile(pos2))
            {
                stop = true;
            }
            
        }

        if (!stop)
        {
            RecursiveRemovalofTerrainWalls(pos + Vector3Int.left);
            RecursiveRemovalofTerrainWalls(pos + Vector3Int.right);
            RecursiveRemovalofTerrainWalls(pos + Vector3Int.up);
            RecursiveRemovalofTerrainWalls(pos + Vector3Int.down);
        }
        
    }
}
