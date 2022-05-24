using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu2 : MonoBehaviour
{
    public void StartButton()
    {
        SceneManager.LoadScene("TutorialScene1");
    }

    public void SettingsButton()
    {
        this.gameObject.SetActive(false);

    }

    public void MainMenuActive()
    {
        this.gameObject.SetActive(true);
    }
}
