using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleOnOff : MonoBehaviour
{

    public void ToggleOn()
    {
        this.gameObject.SetActive(true);
    }

    public void ToggleOff()
    {
        this.gameObject.SetActive(false);
    }
}
