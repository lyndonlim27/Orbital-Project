using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypeRacerTimer : MonoBehaviour
{
    private float seconds;
    
    private void Start()
    {
        seconds = 30;
    }


    private void Update()
    {
        seconds -= Time.deltaTime;
    }
}
