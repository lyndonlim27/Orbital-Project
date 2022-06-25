using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadScene : MonoBehaviour
{
    private DataPersistenceManager _dataManager;
    private Canvas _canvas;
    void Start()
    {
        _dataManager = FindObjectOfType<DataPersistenceManager>();
        _canvas = GetComponentInChildren<Canvas>(true);
        DontDestroyOnLoad(this);
    }

    public void LoadSceneFromData()
    {
        _canvas.gameObject.SetActive(true);
        StartCoroutine(Load());
    }
    private IEnumerator Load()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Assets/Scenes/" + _dataManager.currScene + ".unity");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        _dataManager.LoadGame();
        while (!_dataManager.loaded)
        {
            yield return null;
        }
        _canvas.gameObject.SetActive(false);
    }

    public void NextScene(string sceneName)
    {
        _canvas.gameObject.SetActive(true);
        StartCoroutine(Next(sceneName));
    }
    private IEnumerator Next(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Assets/Scenes/" + sceneName + ".unity");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        yield return StartCoroutine(SetPlayerStats());
        _dataManager.SaveGame();
        _canvas.gameObject.SetActive(false);
    }

    private IEnumerator SetPlayerStats()
    {
        Player player = FindObjectOfType<Player>(true);
        player.AddHealth(1000);
        player.AddMana(1000);
        yield return null;
    }
}

