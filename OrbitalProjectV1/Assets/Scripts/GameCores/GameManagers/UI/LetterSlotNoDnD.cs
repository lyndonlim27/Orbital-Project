using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterSlotNoDnD : MonoBehaviour
{
    public Image image;
    public int currnum;

    private void Awake()
    {
        image = transform.GetChild(0).GetComponent<Image>();
        currnum = -1;
    }

    public void Start()
    {
        image.enabled = false;
    }

    public void SetData(Sprite sprite, int num)
    {
        image.enabled = true;
        image.sprite = sprite;
        currnum = num;
    }

    public void ClearData()
    {
        if(image != null)
        {
            image.sprite = null;
            currnum = -1;
            image.enabled = false;
        }
        
    }
}

