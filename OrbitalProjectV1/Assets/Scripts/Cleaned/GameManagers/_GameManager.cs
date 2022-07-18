using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class _GameManager : MonoBehaviour, IDataPersistence
{  
    [Header("AudioManager")]
    [SerializeField]
    GameObject audioManager;

    [Header("WordBank")]
    [SerializeField]
    GameObject wordBank;

    [Header("UIManager")]
    [SerializeField]
    GameObject UIManager;

    [Header("Dialogue Manager")]
    [SerializeField]
    GameObject DialogueManager;    

    [Header("Pool Manager")]
    [SerializeField]
    GameObject PoolManager;

    [Header("VideoManager")]
    [SerializeField]
    GameObject VideoManager;

    [Header("DamageFlicker")]
    [SerializeField]
    GameObject DamageFlicker;

    [Header("Camera")]
    [SerializeField]
    GameObject CineCamera;

    [Header("RoomManagers")]
    public static List<RoomManager> roomManagers = new List<RoomManager>();

    [Header("MapGenerator")]
    [SerializeField]
    RoomFirstDungeonGenerator mapgenerator;

    private GameData gameData;
    private Player player;

    [Header("Local Managers")]

    private DamageFlicker _damageFlickerl;
    private VideoManager _vidManagerl;
    private GameObject _audioManagerl;
    private WordBank _wordBankl;
    private PuzzleInputManager _puzzleInputManagerl;
    private DialogueManager _dialogueManagerl;
    private PoolManager _poolManagerl;


    [SerializeField] private int roomIndex = 1;
    /// <summary>
    /// Loads Up all Managers required for gameplay.
    /// </summary>
    public void GenerateManagers()
    {
        GameObject _audioManager = GameObject.Find("AudioManager(Clone)");
        if (_audioManager == null)
        {
            _audioManager = Instantiate(audioManager);
            
        }

        GameObject _wordBank = GameObject.Find("wordGenerator(Clone)");
        if (_wordBank == null)
        {
            _wordBank = Instantiate(wordBank);
            
        }

        GameObject _UIManager = GameObject.Find("UIManager(Clone)");
        if (_UIManager == null)
        {
            _UIManager =Instantiate(UIManager);
            
        }

        GameObject _DialogueManager = GameObject.Find("DialogueManager1(Clone)");
        if (_DialogueManager == null)
        {
            _DialogueManager = Instantiate(DialogueManager);
            
        }

        GameObject _PoolManager = GameObject.Find("PoolManager(Clone)");
        if (_PoolManager == null)
        {
            _PoolManager = Instantiate(PoolManager);
            
        }

        GameObject _DamageFlicker = GameObject.Find("DamageFlicker(Clone)");
        if (_DamageFlicker == null)
        {
            _DamageFlicker = Instantiate(DamageFlicker);
        }

        GameObject _VideoManager = GameObject.Find("VideoManager(Clone)");
        if (_VideoManager == null)
        {
            _VideoManager = Instantiate(VideoManager);
        }



        //GameObject _AStar = GameObject.Find("A_");
        //if (_AStar == null)
        //{
        //    _AStar = Instantiate(AStar);
        //}


        //_AStar.transform.SetParent(transform);
        _DamageFlicker.transform.SetParent(transform);
        _audioManager.transform.SetParent(transform);
        _wordBank.transform.SetParent(transform);
        _UIManager.transform.SetParent(transform);
        _DialogueManager.transform.SetParent(transform);
        _PoolManager.transform.SetParent(transform);
        _VideoManager.transform.SetParent(transform);

        // local copies.
        _audioManagerl = _audioManager;
        _wordBankl = _wordBank.GetComponent<WordBank>();
        _damageFlickerl = _DamageFlicker.GetComponent<DamageFlicker>();
        _vidManagerl = _VideoManager.GetComponent<VideoManager>();
        _dialogueManagerl = _DialogueManager.GetComponent<DialogueManager>();
        _puzzleInputManagerl = _UIManager.GetComponentInChildren<PuzzleInputManager>();
        _poolManagerl = _PoolManager.GetComponent<PoolManager>();
        mapgenerator = FindObjectOfType<RoomFirstDungeonGenerator>();

    }

    /// <summary>
    /// Set Up Camera to follow player transform.
    /// </summary>
    /// <param name="player"></param>
    private void SetUpCamera(Transform player)
    {
        GameObject _CineCamera = GameObject.Find("CineCamera(Clone)");
        if (_CineCamera == null)
        {
            _CineCamera = Instantiate(CineCamera);
        }
        _CineCamera.transform.SetParent(transform);
        _CineCamera.GetComponent<CinemachineVirtualCamera>().Follow = player;
    }

    /// <summary>
    /// Spawn player in the first room in the map.
    /// </summary>
    public void SpawnPlayer()
    {
        player = FindObjectOfType<Player>(true);
        if (player == null)
        {
            Player playerPrefab = Resources.Load<Player>("Player 1") as Player;
            player = Instantiate(playerPrefab);
        }
        RoomManager roommgr = GameObject.Find("Room1").GetComponent<RoomManager>();
        player.SetCurrentRoom(roommgr);
        player.transform.position = roommgr.transform.position;
        SetUpCamera(player.transform);
    }

    public void SetPlayerPosition()
    {
        if (player == null)
        {
            SpawnPlayer();
        }
        RoomManager currRoom = roomManagers[roomIndex - 1];
        player.SetCurrentRoom(currRoom);
        player.transform.position = currRoom.transform.position;

    }

    public void GenerateMap(int seed)
    {
        mapgenerator.currentSeed = seed;
        mapgenerator.GenerateDungeon();
    }

    private void PlayIntroScene()
    {
        if (_vidManagerl == null)
        {
            Debug.LogError("No vid manager found");
        } else
        {
            _vidManagerl.PlayVideo("LastLevelStartScene");
        }
    }

    private void PlayEndScene()
    {
        _vidManagerl.PlayVideo("EndScene");
    }

    private void Awake()
    {
        Debug.Log("Debug Awake");
        GenerateManagers();
        //GenerateMap(-1);        
    }

    private void Start()
    {
        //PlayIntroScene();
        //SpawnPlayer();
    }

    public void LoadData(GameData data)
    {
        GenerateMap(data.currentSeed);
               
    }


    public void SaveData(ref GameData data)
    {
        data.currentSeed = mapgenerator.currentSeed;
    }
}
