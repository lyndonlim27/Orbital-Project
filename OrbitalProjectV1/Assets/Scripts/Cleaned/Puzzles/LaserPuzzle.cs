using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPuzzle : MonoBehaviour, Puzzle
{
    ItemWithTextData laserBeamData;
    ItemWithTextData mirrorData;
    ItemWithTextBehaviour laserBeam;
    GameObject target;
    RoomManager currentRoom;
    private bool activated;

    private void Awake()
    {
        CreateLaserData();
        CreateMirrorData();
        currentRoom = GetComponentInParent<RoomManager>();
        activated = false;

    }

    

    #region Creating Datas.
    /// <summary>
    /// Creating laser data.
    /// </summary>
    private void CreateLaserData()
    {
        laserBeamData = ScriptableObject.CreateInstance<ItemWithTextData>();
        laserBeamData._type = EntityData.TYPE.ITEM;
        laserBeamData.item_type = ItemWithTextData.ITEM_TYPE.LASER;
        laserBeamData.spawnAtStart = true;
        laserBeamData.scale = 1;
        laserBeamData.sprite = Resources.Load<Sprite>("Sprites/LaserTurret");
        laserBeamData.random = true;
        laserBeamData._name = "MOUNT";
        laserBeamData.description = "Move with left and right arrow keys \nSpace to shoot laser and \nESC to unmount";
        laserBeamData.minDist = 1.5f;
        
       
    }

    /// <summary>
    /// Creating Mirror data.
    /// </summary>
    private void CreateMirrorData()
    {
        mirrorData = ScriptableObject.CreateInstance<ItemWithTextData>();
        mirrorData._type = EntityData.TYPE.ITEM;
        mirrorData.item_type = ItemWithTextData.ITEM_TYPE.MIRROR;
        mirrorData.scale = 1;
        mirrorData.spawnAtStart = true;
        mirrorData.sprite = Resources.Load<Sprite>("Sprites/Mirror");
        mirrorData.random = true;
        mirrorData._name = "RESET";
        mirrorData.minDist = 1.5f;
        mirrorData.description = "";
    }

    /// <summary>
    /// Creating Target.
    /// </summary>
    private void CreatingTarget()
    {
        target = new GameObject("Target");
        SpriteRenderer renderer = target.AddComponent<SpriteRenderer>();
        renderer.sortingOrder = 1;
        renderer.sprite = Resources.Load<Sprite>("Sprites/Target");
        target.tag = "Target";
        target.layer = LayerMask.NameToLayer("Obstacles");
        target.AddComponent<BoxCollider2D>();
        target.transform.SetParent(currentRoom.transform);
        target.transform.position = currentRoom.transform.position + Vector3Int.down * 4 + Vector3Int.left * 2;
    }
    #endregion 

    public void ActivatePuzzle(int seqs)
    {
        activated = true;
        currentRoom.SpawnObject(laserBeamData);
        int rand = Random.Range(3, 5);
        while(rand-- > 0)
        {
            currentRoom.SpawnObject(mirrorData);
        }
        laserBeam = transform.Find("LaserBeam").GetComponent<ItemWithTextBehaviour>();
        Debug.Log(laserBeam);
        CreatingTarget();

    }

    public void Fulfill()
    {

    }

    public bool IsActivated()
    {
        return activated;
    }

    public bool IsComplete()
    {
        if (laserBeam != null)
        {
            return laserBeam.targetHit;
        } else
        {
            return false;
        }
    }

    public void Next()
    {
        throw new System.NotImplementedException();
    }
}
