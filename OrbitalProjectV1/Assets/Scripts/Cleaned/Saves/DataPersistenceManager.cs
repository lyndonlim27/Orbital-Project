using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;
    [SerializeField] private string _email;
    [SerializeField] private string _password;

    private GameData _gameData;
    private List<IDataPersistence> _dataPersistences;

    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Data Persistence Manager in the scene. Please put only one data manager");
        }
        instance = this;
    }

    private void Start()
    {
        this._dataPersistences = FindAllDataPersistenceObjects();
        //LoadGame();

    }

    [ContextMenu("New Game")]
    public void NewGame()
    {
        this._gameData = new GameData();
    }

    [ContextMenu("Load")]
    public void LoadGame()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataLoad, OnError);
    }

    [ContextMenu("Save")]
    public void SaveGame()
    {
        this._gameData = new GameData();
        foreach (IDataPersistence dataPersistence in _dataPersistences)
        {
            dataPersistence.SaveData(ref _gameData);
        }
        string dataToStore = JsonUtility.ToJson(_gameData, true);
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { "Player", dataToStore }
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
    }


    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistences = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistences);
    }

    [ContextMenu("Register")]
    public void Register()
    {
        var request = new RegisterPlayFabUserRequest
        {
            Email = this._email,
            Password = _password,
            RequireBothUsernameAndEmail = false 
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    [ContextMenu("Login")]
    public void Login()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = this._email,
            Password = _password,
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("Registered and logged in");
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Logged in");
    }

    private void OnError(PlayFabError error)
    {
        Debug.Log("Error logging in/creating account");
    }

    private void OnDataSend(UpdateUserDataResult result)
    {
        Debug.Log("Saved");
    }

    private void OnDataLoad(GetUserDataResult result)
    {
        Debug.Log("Loaded");
        GameData loadedData = JsonUtility.FromJson<GameData>(result.Data["Player"].Value);
        foreach (IDataPersistence dataPersistence in _dataPersistences)
        {
            dataPersistence.LoadData(loadedData);
        }
    }
}




