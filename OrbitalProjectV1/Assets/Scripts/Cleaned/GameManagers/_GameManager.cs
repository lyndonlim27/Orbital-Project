using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class _GameManager : MonoBehaviour
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

    [Header("DamageFlicker")]
    [SerializeField]
    GameObject DamageFlicker;

    [Header("Camera")]
    [SerializeField]
    GameObject CineCamera;

    [Header("RoomManagers")]
    public List<RoomManager> roomManagers = new List<RoomManager>();

    
    private Player player;
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
}