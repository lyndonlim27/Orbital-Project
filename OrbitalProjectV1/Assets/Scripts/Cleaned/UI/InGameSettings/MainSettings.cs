using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSettings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MainSettingsActive()
    {
        this.gameObject.SetActive(true);
    }

    public void MainSettingsInactive()
    {
        this.gameObject.SetActive(false);
    }
}
