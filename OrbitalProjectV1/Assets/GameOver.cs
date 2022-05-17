using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{   
    public void Setup()
    {
        gameObject.SetActive(true);

    }

    public void RestartButton()
    {
        SceneManager.LoadScene("SampleScene1");
    }

    public void ExitButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
