using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/** 
 * Main Menu.
 */
public class MainMenu : MonoBehaviour
{

    /**
     * Load First Scene.
     */

    public void StartButton()
    {
        SceneManager.LoadScene("TutorialLevel");
    }

    /**
     * On button pressed, activate Settings panel.
     */
    public void SettingsButton()
    {
        this.gameObject.SetActive(false);

    }

    /**
     * On button pressed, activate Main Menu panel.
     */
    public void MainMenuActive()
    {
        this.gameObject.SetActive(true);
    }
}
