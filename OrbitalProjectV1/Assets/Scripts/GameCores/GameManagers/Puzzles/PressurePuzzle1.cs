using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using EntityCores;
using EntityDataMgt;
using GameManagement;

namespace PuzzleCores
{
    public class PressurePuzzle1 : MonoBehaviour, Puzzle
    {

        List<SwitchData> pressureSwitchDatas;
        List<ItemWithTextData> pushableDatas;
        List<PressureSwitchBehaviour> pressureSwitchBehaviours;
        List<ItemWithTextBehaviour> pushableObjects;
        RoomManager currRoom;
        bool activated;
        int rand;


        public void ActivatePuzzle(int seqs)
        {
            activated = true;
            currRoom.SpawnObjects(pressureSwitchDatas.ToArray());
            currRoom.SpawnObjects(pushableDatas.ToArray());
            pressureSwitchBehaviours = GetComponentsInChildren<PressureSwitchBehaviour>().ToList();
            pushableObjects = GetComponentsInChildren<ItemWithTextBehaviour>().ToList();

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
            return activated && pressureSwitchBehaviours.All(x => x.IsOn());
        }

        public void Next()
        {
            throw new System.NotImplementedException();
        }

        // Start is called before the first frame update

        private void Awake()
        {
            currRoom = GetComponent<RoomManager>();
            rand = Random.Range(3, 5);
            pushableDatas = new List<ItemWithTextData>();
            pressureSwitchBehaviours = new List<PressureSwitchBehaviour>();
            pressureSwitchDatas = new List<SwitchData>();
            pushableObjects = new List<ItemWithTextBehaviour>();


        }

        public void CreateSwitchDatas()
        {
            var pressureSwitchData = Resources.Load("Data/PressurePlates/PSwitchL3") as SwitchData;

            for (int i = 0; i < rand; i++)
            {
                var clone = Instantiate(pressureSwitchData);
                clone.random = false;
                clone.pos = currRoom.GetRandomObjectPoint();
                pressureSwitchDatas.Add(clone);
            }
        }

        public void CreatePushableData()
        {
            var pushableData = Resources.Load("Data/ItemWithText/PushableObjects") as ItemWithTextData;
            var allSprites = pushableData.itemSprites;
            Sprite selectedsprite = allSprites[Random.Range(0, allSprites.Length - 1)];
            for (int i = 0; i < rand; i++)
            {
                var clone = Instantiate(pushableData);
                clone.sprite = selectedsprite;
                clone.random = false;
                clone.pos = currRoom.GetRandomObjectPoint();
                pushableDatas.Add(clone);
            }
        }
    }
}
