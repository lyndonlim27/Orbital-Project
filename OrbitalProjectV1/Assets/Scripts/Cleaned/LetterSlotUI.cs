using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LetterSlotUI : MonoBehaviour, IDropHandler
{
    private DragAndDrop dragAndDrop;
    private RectTransform rectTransform;
    //private CanvasGroup canvasGroup;
    private WordStorageManagerUI wordStorage;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        dragAndDrop = GetComponentInChildren<DragAndDrop>();
       

    }

    private void Start()
    {
        wordStorage = WordStorageManagerUI.instance;
        ClearSlot();
    }

    public void SetData(ConsumableItemData newletterData)
    {
        if (newletterData == null)
        {
            dragAndDrop.ClearImage();
        } else
        {
            dragAndDrop.SetImage(newletterData.sprite);
        }
    }

    // Clear the slot
    public void ClearSlot()
    {
        dragAndDrop.ClearImage();
        

    }

    public void OnDrop(PointerEventData eventData)
    {
        DragAndDrop dnd = eventData.pointerDrag.GetComponent<DragAndDrop>();
        LetterSlotUI currslot = dnd.currSlot;
        wordStorage.SwitchItemSlots(currslot, this);
        dnd.ResetPosition();


    }

    

}
