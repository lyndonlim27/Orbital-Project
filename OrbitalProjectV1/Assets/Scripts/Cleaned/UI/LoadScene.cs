using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;


public class LoadScene : MonoBehaviour
{
    private DataPersistenceManager _dataManager;
    public static LoadScene Instance { get; private set; }
    private Canvas _canvas;

    private void Awake()
    {
        if (Instance != null)
        {
            this.enabled = false;
        } else
        {
            Instance = this;
        }
    }

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
        //if (_dataManager.currScene != SceneManager.GetActiveScene().ToString())
        //{
            
        //}
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
        yield return new WaitForSeconds(1f);
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
        while (!_dataManager.saved)
        {
            yield return null;
        }

        _dataManager.LoadGame();
        while (!_dataManager.loaded)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        _canvas.gameObject.SetActive(false);
    }

    private IEnumerator SetPlayerStats()
    {
        Player player = FindObjectOfType<Player>(true);
        if (player != null)
        {
            player.AddHealth(1000);
            player.AddMana(1000);
        }

        yield return null;


    }

    public void LoadMainMenu()
    {
        _canvas.gameObject.SetActive(true);
        StartCoroutine(LoadMainScene());
    }

    private IEnumerator LoadMainScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Assets/Scenes/MainMenu.unity");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        _canvas.gameObject.SetActive(false);
    }

}

