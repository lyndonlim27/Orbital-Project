using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{   
    public void Setup()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        
        foreach (GameObject obj in allObjects)
        {
            if(obj.CompareTag("Enemy") || obj.CompareTag("Player"))
            {
                obj.SetActive(false);
            }
        }
        
        gameObject.SetActive(true);

    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
