using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;
using UnityEngine.Tilemaps;

public class _GameManager : MonoBehaviour, IDataPersistence
{
    #region Variables

    public static _GameManager instance { get; private set; }

    #region ManagerPrefabs
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
    public static bool allTerrainsGenerated { get; private set; }

    [Header("MapGenerator")]
    [SerializeField]
    RoomFirstDungeonGenerator mapgenerator;

    [Header("Ship")]
    [SerializeField]
    private GameObject ship;

    #endregion

    #region Player and GameData
    private GameData gameData;
    private Player player;
    private bool spawned;
    private bool painted;
    public bool playerspawned;
    [SerializeField] private int roomIndex = 1;
    #endregion

    #region LocalManagers
    [Header("Local Managers")]

    private DamageFlicker _damageFlickerl;
    private VideoManager _vidManagerl;
    private GlobalAudioManager _audioManagerl;
    private WordBank _wordBankl;
    private PuzzleInputManager _puzzleInputManagerl;
    private DialogueManager _dialogueManagerl;
    private PoolManager _poolManagerl;
    #endregion
    #endregion

    #region Public Methods

    #region Generation and Player Spawn
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
        _DamageFlicker.transform.SetParent(transform);
        _audioManager.transform.SetParent(transform);
        _wordBank.transform.SetParent(transform);
        _UIManager.transform.SetParent(transform);
        _DialogueManager.transform.SetParent(transform);
        _PoolManager.transform.SetParent(transform);
        _VideoManager.transform.SetParent(transform);

        // local copies.
        _audioManagerl = _audioManager.GetComponentInChildren<GlobalAudioManager>();
        _wordBankl = _wordBank.GetComponent<WordBank>();
        _damageFlickerl = _DamageFlicker.GetComponent<DamageFlicker>();
        _vidManagerl = _VideoManager.GetComponent<VideoManager>();
        _dialogueManagerl = _DialogueManager.GetComponent<DialogueManager>();
        _puzzleInputManagerl = _UIManager.GetComponentInChildren<PuzzleInputManager>();
        _poolManagerl = _PoolManager.GetComponent<PoolManager>();
        mapgenerator = FindObjectOfType<RoomFirstDungeonGenerator>();

    }

    public void GenerateMap(int seed)
    {
        mapgenerator.currentSeed = seed;
        mapgenerator.GenerateDungeon();
    }

    /// <summary>
    /// Spawn player in the first room in the map.
    /// </summary>
    public IEnumerator SpawnPlayer()
    {
        while (!allTerrainsGenerated)
        {
            yield return null;
        }
        player = FindObjectOfType<Player>(true);
        if (player == null)
        {
            Player playerPrefab = Resources.Load<Player>("Player 1") as Player;
            player = Instantiate(playerPrefab);
        }
        foreach(RoomManager room in roomManagers)
        {
            room.GetSpawnablePointsInRoom();
            yield return null;
        }
        if(gameData.currScene != "FinalLevel")
        {
            RoomManager roommgr = GameObject.Find("Room1").GetComponent<RoomManager>();
            player.SetCurrentRoom(roommgr);
            player.transform.position = roommgr.GetRandomObjectPoint();
            
        } else
        {
            player.transform.position = gameData.currPos;
            FindAllIncompleteRooms();
            
        }
        SetUpCamera(player.transform);
        yield return null;
        DisableAllOtherRooms();
        playerspawned = true;

    }

    private void DisableAllOtherRooms()
    {
        roomManagers.ForEach(room =>
        {
            if (room != player.GetCurrentRoom())
            {
                room.enabled = false;
            }
        });
        
    }

    private void FindAllIncompleteRooms()
    {
        if (gameData != null)
        {
            List<int> incompleteRooms = new List<int>();

            foreach (string room in gameData.rooms.Keys)
            {
                if (gameData.rooms[room] == 0)
                {

                    int index = room.LastIndexOf(":") + 1;
                    incompleteRooms.Add(int.Parse(room.Substring(index)));
                }
            }
            incompleteRooms.Sort();
            var currRoom = roomManagers[incompleteRooms[0]];
            player.SetCurrentRoom(currRoom);
        }
        


    }

    public void EnableRoom(int roomIndex)
    {
        if (roomIndex >= 0 && roomIndex < roomManagers.Count)
        {
            roomManagers[roomIndex].enabled = true;
        }
        
    }

    //public void SetPlayerPosition()
    //{
    //    if (player == null)
    //    {
    //        StartCoroutine(SpawnPlayer());
    //    }
    //    RoomManager currRoom = roomManagers[roomIndex - 1];
    //    player.SetCurrentRoom(currRoom);
    //    player.transform.position = currRoom.GetRandomObjectPoint();

    //}
    #endregion

    #region Save and Loads
    public void LoadData(GameData data)
    {
        //allTerrainsGenerated = false;
        //GenerateMap(data.currentSeed);
        //painted = false;
        //spawned = false;
        //playerspawned = true;
    }

    public void SaveData(ref GameData data)
    {
        data.currentSeed = mapgenerator.currentSeed;
        data.currPos = player.transform.position;
    }
    #endregion

    #endregion

    #region Private Methods
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

   //public void createcam()
   // {
   //     GameObject _CineCamera = GameObject.Find("CineCamera(Clone)");
   //     if (_CineCamera == null)
   //     {
   //         _CineCamera = Instantiate(CineCamera);
   //     }
   //     _CineCamera.transform.SetParent(transform);
   // }





    #endregion

    #region Monobehaviour
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        GenerateManagers();
        allTerrainsGenerated = false;
        playerspawned = false;
        
    }

    private void Start()
    {
        gameData = DataPersistenceManager.Instance.gameData;
        _audioManagerl.totalRooms = roomManagers.Count;
        _audioManagerl.GetComponent<AudioSource>().volume = 0.3f;
        if (gameData.currentSeed == 0)
        {
            //PlayIntroScene();
            GenerateMap(-1);
        }
        else
        {
            GenerateMap(gameData.currentSeed);
            
        }
        StartCoroutine(SpawnPlayer());
        
        painted = false;
        spawned = false;

    }

    private void Update()
    {
        if (roomManagers.Count > 0 && FragmentUI.instance.IsComplete() && !spawned)
        {
            spawned = true;
            UITextDescription.instance.StartDescription("Find the ship!");
            SpawnShip();
            
        }

    }

    private void SpawnShip()
    {
        var selectedroom = roomManagers[UnityEngine.Random.Range(0, roomManagers.Count)];
        StartCoroutine(selectedroom.SpawnEndCredit(ship));
        
    }

    private void LateUpdate()
    {
        allTerrainsGenerated = roomManagers.TrueForAll(room => room.terrainGenerated);
    }


    private void DebugUnwalkableTiles()
    {
        if (allTerrainsGenerated && !painted)
        {
            painted = true;
            TerrainGenerator.instance.PaintUnwalkableTiles();
        }
    }
    #endregion

}
