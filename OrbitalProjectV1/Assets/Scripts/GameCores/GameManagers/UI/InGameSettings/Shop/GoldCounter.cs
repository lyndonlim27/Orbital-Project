using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using EntityCores;

namespace GameManagement.UIComps
{
    public class GoldCounter : MonoBehaviour
    {

        public void GoldUpdate()
        {
            Player _player = FindObjectOfType<Player>();
            TextMeshProUGUI _goldText = GetComponentInChildren<TextMeshProUGUI>();
            _goldText.text = _player.currGold.ToString();
        }
    }
}
