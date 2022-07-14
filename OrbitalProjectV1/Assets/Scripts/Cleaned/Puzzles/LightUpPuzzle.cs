using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightUpPuzzle : MonoBehaviour, Puzzle
{
    ItemWithTextData puzzleBox;
    [SerializeField] LightSwitchSystem lightSwitchSystem;
    RoomManager currRoom;
    private bool activated;

    private void Awake()
    {
        currRoom = GetComponent<RoomManager>();
        CreateLightSwitchSystem();
        CreatePuzzleBoxData();
        
        
    }

    private void CreateLightSwitchSystem()
    {
        GameObject lightSwitchSystemPrefab = Resources.Load("PuzzlePrefab/LightSwitchSystem") as GameObject;
        Debug.Log(lightSwitchSystemPrefab);
        GameObject go = Instantiate(lightSwitchSystemPrefab) as GameObject;
        go.transform.SetParent(currRoom.transform);
        lightSwitchSystem = go.GetComponent<LightSwitchSystem>();
        lightSwitchSystem.SetCurrentRoom(currRoom);
        lightSwitchSystem.RandomizeCandlesPos();


    }

    private void CreatePuzzleBoxData()
    {
        var temp = Resources.Load<ItemWithTextData>("Data/ItemWithText/Guess");
        puzzleBox = Instantiate<ItemWithTextData>(temp);
        puzzleBox.pos = currRoom.transform.position;
    }

    public void ActivatePuzzle(int seqs)
    {
        activated = true;
        currRoom.SpawnObject(puzzleBox);
        GetComponentInChildren<PuzzleBox>().SetLightSwitchSystem(lightSwitchSystem);
        lightSwitchSystem.ActivatePuzzle(seqs);
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
        return lightSwitchSystem.IsComplete();
    }

    public void Next()
    {
        lightSwitchSystem.Next();
    }
}
