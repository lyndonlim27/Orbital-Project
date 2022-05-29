using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


/**
 * GameOver UI Page.
 */
public class GameOver : MonoBehaviour
{

    /**
     * Disable all active gameObjects and Setup GameOver Page.
     */
    public void Setup()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.CompareTag("Enemy") || obj.CompareTag("Player"))
            {
                //if (!(obj.CompareTag("MainCamera") || obj.CompareTag("CMCam") || obj.CompareTag("Audio")))
                obj.SetActive(false);
            }
        }
        gameObject.SetActive(true);

    }


    /**
     * Restart Button.
     */
    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /**
     * Exit Button.
     */
    public void ExitButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
