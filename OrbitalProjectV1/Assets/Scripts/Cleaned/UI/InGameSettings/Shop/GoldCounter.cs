using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GoldCounter : MonoBehaviour
{

    public void GoldUpdate()
    {
        Player _player = FindObjectOfType<Player>();
        TextMeshProUGUI _goldText = GetComponentInChildren<TextMeshProUGUI>();
        Debug.Log(_goldText);
        Debug.Log(_player);
        _goldText.text = _player.currGold.ToString();
    }
}
