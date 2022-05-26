using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpSettings : MonoBehaviour
{
    private RoomManager roomManager;
    // Start is called before the first frame update
    void Awake()
    {
        roomManager = FindObjectOfType<RoomManager>(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PopUpSettingsActive()
    {
        this.gameObject.SetActive(true);
        roomManager.PauseGame();
    }

    public void PopUpSetingsInactive()
    {
        this.gameObject.SetActive(false);
        roomManager.ResumeGame();
        Debug.Log("WHY");
    }
}
