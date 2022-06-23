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
    public IEnumerator Load()
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
}

