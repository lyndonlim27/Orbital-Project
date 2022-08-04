using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using EntityCores;

namespace GameManagement.Helpers
{
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
            }
            else
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
            _dataManager.LoadData();
            while (!_dataManager.loadedData)
            {
                yield return null;
            }
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Assets/Scenes/" + _dataManager.gameData.currScene + ".unity");

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
            _dataManager.SaveGame();
            while (!_dataManager.saved)
            {
                yield return null;
            }
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Assets/Scenes/" + sceneName + ".unity");
            _dataManager.gameData.rooms = null;
            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
            yield return StartCoroutine(SetPlayerStats());
            if (_GameManager.instance != null)
            {
                while (!_GameManager.instance.playerspawned)
                {
                    yield return null;
                }
            }


            _dataManager.SaveGame();
            while (!_dataManager.saved)
            {
                yield return null;
            }
            _dataManager.LoadData();
            while (!_dataManager.loadedData)
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
                _dataManager.LoadData();
                while (!_dataManager.loadedData)
                {
                    yield return null;
                }
                player.SetStats(_dataManager.gameData);
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
}

