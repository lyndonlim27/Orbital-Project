using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheEnd : MonoBehaviour
{
    [SerializeField] private LoadMainScene _loadMainMenu;

    private void OnTriggerEnter2D()
    {
        _loadMainMenu.LoadScene();
    }
}
