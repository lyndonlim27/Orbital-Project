using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjConControlMenu : MonoBehaviour
{

    private ProjConMainMenu projConMainMenu;
    // Start is called before the first frame update
    void Start()
    {
        projConMainMenu = FindObjectOfType<ProjConMainMenu>(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ControlMenuInctive();
            projConMainMenu.MainMenuActive();
        }
    }

    public void ControlMenuActive()
    {
        this.gameObject.SetActive(true);
    }

    public void ControlMenuInctive()
    {
        this.gameObject.SetActive(false); ;
    }
}
