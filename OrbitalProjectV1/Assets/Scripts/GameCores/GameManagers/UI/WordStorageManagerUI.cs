using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using EntityDataMgt;
using GameManagement.UIComps;

namespace GameManagement
{
    public class WordStorageManagerUI : MenuBehaviour
    {
        private Pointer pointer;
        public static WordStorageManagerUI instance;
        public static Dictionary<LetterSlotUI, ConsumableItemData> inventoryslots;


        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }

            inventoryslots = new Dictionary<LetterSlotUI, ConsumableItemData>();
            LetterSlotUI[] slots = FindObjectsOfType<LetterSlotUI>(true);
            foreach (LetterSlotUI slot in slots)
            {
                inventoryslots[slot] = null;
            }

        }

        private void Update()
        {
            //Debug.Log(NotFull());
            //Debug.Log(inventoryslots.Values.ToList().Count);
        }

        public void AddItem(ConsumableItemData data)
        {
            foreach (LetterSlotUI letterSlot in inventoryslots.Keys)
            {
                if (inventoryslots[letterSlot] == null)
                {
                    UpdateItem(letterSlot, data);
                    return;
                }
            }

        }

        public bool NotFull()
        {
            return inventoryslots.ContainsValue(null);
        }

        public void UpdateItem(LetterSlotUI slot, ConsumableItemData data)
        {
            inventoryslots[slot] = data;
            slot.SetData(data);
        }

        public void SwitchItemSlots(LetterSlotUI slot1, LetterSlotUI slot2)
        {
            ConsumableItemData data1 = inventoryslots[slot1];
            ConsumableItemData data2 = inventoryslots[slot2];

            UpdateItem(slot1, data2);
            UpdateItem(slot2, data1);
        }

    }
}