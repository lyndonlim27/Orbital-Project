using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using EntityDataMgt;

/// <summary>
/// This is the UI Comp for Weapon Description.
/// </summary>
namespace GameManagement.UIComps
{
    public class WeaponDescription : MonoBehaviour
    {
        #region Variables
        TextMeshProUGUI[] textMeshProUGUIs;
        Image image;
        #endregion

        #region MonoBehaviour
        private void Awake()
        {
            textMeshProUGUIs = GetComponentsInChildren<TextMeshProUGUI>();
        }
        #endregion

        #region Client-Assess Methods
        public void SetWeaponPickUp(RangedData rangedData)
        {
            textMeshProUGUIs[0].text = rangedData.weapdescription;
            textMeshProUGUIs[0].color = rangedData.defaultcolor;

        }

        public void SetCurrWeapon(RangedData rangedData)
        {
            if (rangedData == null)
            {
                textMeshProUGUIs[1].text = "";
                textMeshProUGUIs[1].color = Color.white;
                return;
            }
            textMeshProUGUIs[1].text = rangedData.weapdescription;
            textMeshProUGUIs[1].color = rangedData.defaultcolor;
        }
        #endregion
    }
}
