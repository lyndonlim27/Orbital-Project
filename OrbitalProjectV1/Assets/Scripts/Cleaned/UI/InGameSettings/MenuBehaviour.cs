using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBehaviour : MonoBehaviour
{
    protected PopUpSettings popUpSettings;

    protected virtual void Start()
    {
        popUpSettings = FindObjectOfType<PopUpSettings>(true);
    }

    public virtual void Active()
    {
        this.gameObject.SetActive(true);
    }

    public virtual void Inactive()
    {
        this.gameObject.SetActive(false);
    }
}
