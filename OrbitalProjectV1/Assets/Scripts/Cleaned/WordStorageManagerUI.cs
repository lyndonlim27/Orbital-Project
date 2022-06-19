using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        if (Input.GetKeyDown(KeyCode.P))
        {
            ConsumableItemData cons = ScriptableObject.CreateInstance<ConsumableItemData>();
            cons.sprite = Resources.Load<Sprite>("Sprites/corpse");
            AddItem(cons);
        }
        //Debug.Log(NotFull());
        //Debug.Log(inventoryslots.Values.ToList().Count);
    }

    public void AddItem(ConsumableItemData data)
    {
        Debug.Log("This is itemdata" + data);
        foreach(LetterSlotUI letterSlot in inventoryslots.Keys)
        {
            Debug.Log(letterSlot);
            Debug.Log(data.sprite);
            if (inventoryslots[letterSlot] == null)
            {
                Debug.Log("What?");
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