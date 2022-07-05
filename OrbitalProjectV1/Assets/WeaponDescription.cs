using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WeaponDescription : MonoBehaviour
{
    TextMeshProUGUI[] textMeshProUGUIs;
    Image image;

    private void Awake()
    {
        textMeshProUGUIs = GetComponentsInChildren<TextMeshProUGUI>();
    }

    public void SetWeaponPickUp(RangedData rangedData)
    {
        textMeshProUGUIs[0].text = rangedData.weapdescription;
        textMeshProUGUIs[0].color = rangedData.defaultcolor;

    }

    public void SetCurrWeapon(RangedData rangedData)
    {
        textMeshProUGUIs[1].text = rangedData.weapdescription;
        textMeshProUGUIs[1].color = rangedData.defaultcolor;
    }
}
