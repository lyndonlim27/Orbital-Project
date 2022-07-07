using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPuzzle : MonoBehaviour, Puzzle
{
    ItemWithTextData laserBeamData;
    ItemWithTextData mirrorData;
    RoomManager currentRoom;
    private bool activated;

    private void Awake()
    {
        CreateLaserData();
        currentRoom = GetComponentInParent<RoomManager>();
        activated = false;
    }

    private void CreateLaserData()
    {
        laserBeamData = ScriptableObject.CreateInstance<ItemWithTextData>();
        laserBeamData.item_type = ItemWithTextData.ITEM_TYPE.LASER;
        laserBeamData.spawnAtStart = true;
        laserBeamData.scale = 1;
        laserBeamData.random = true;
        mirrorData = ScriptableObject.CreateInstance<ItemWithTextData>();
        mirrorData.item_type = ItemWithTextData.ITEM_TYPE.MIRROR;


        
        
    }

    public void ActivatePuzzle(int seqs)
    {
        activated = true;
        currentRoom.SpawnObject(laserBeamData);
        int rand = Random.Range(3, 5);
        while(rand-- > 0)
        {
            currentRoom.SpawnObject(mirrorData);
        }
        
    }

    public void Fulfill()
    {
        throw new System.NotImplementedException();
    }

    public bool IsActivated()
    {
        return activated;
    }

    public bool IsComplete()
    {
        return false;
    }

    public void Next()
    {
        throw new System.NotImplementedException();
    }
}
