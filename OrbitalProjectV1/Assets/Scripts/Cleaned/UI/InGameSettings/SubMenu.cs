using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubMenu : MonoBehaviour
{
    [SerializeField] private MainSettings settings;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ControlMenuInactive();
            settings.MainSettingsActive();
        }
    }

    public void ControlMenuActive()
    {
        this.gameObject.SetActive(true);

    }

    public void ControlMenuInactive()
    {
        this.gameObject.SetActive(false);
    }
}
