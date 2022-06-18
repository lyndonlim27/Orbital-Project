using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    private GameData _gameData;
    private List<IDataPersistence> _dataPersistences;
    private FileDataHandler dataHandler;

    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Found more than one Data Persistence Manager in the scene. Please put only one data manager");
        }
        instance = this;
    }

    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
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
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        this._gameData = dataHandler.Load();

        if(this._gameData == null)
        {
            Debug.Log("No data was found. Creating new game instead");
            NewGame();
        }

        foreach(IDataPersistence dataPersistence in _dataPersistences)
        {
            dataPersistence.LoadData(_gameData);
        }

    }

    [ContextMenu("Save")]
    public void SaveGame()
    {
        this._gameData = new GameData();
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        foreach (IDataPersistence dataPersistence in _dataPersistences)
        {
            dataPersistence.SaveData(ref _gameData);
        }
        dataHandler.Save(_gameData);
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistences = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistences);
    }
}


