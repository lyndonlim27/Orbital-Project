using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class RoomDesign : MonoBehaviour
{
    #region Variables
    #region DefaultProperties
    public static RoomDesign instance { get; private set; }
    private TerrainGenerator terrainGenerator;
    private List<Vector3> structpoints;
    public enum ROOM_DESIGNS
    {
        SWAMP,
        CAMP,
        TOWN,
        CATACOMB,
        TEMPLE,
        FARM,
        FOREST, /**DEFAULT**/
        FIREKNIGHTROOM,
        WATERMAGEROOM,
        BLADEKEEPERROOM,


    }
    #endregion

    #region PhysicsFactors
    [SerializeField]
    [Range(0, 100)]
    private float minDistBwStructs;

    [SerializeField]
    [Range(0, 100)]
    private float minDistfromCenter;
    #endregion

    #region DecorativeMaterials
    [Header("Decorations")]

    #region Town
    [Header("Town")]
    public GameObject[] houses;
    public GameObject[] townmandatoryDecorations;
    #endregion 

    #region Farm
    [Header("Farm")]
    public GameObject[] farms;
    public GameObject[] farmDecorations;
    #endregion

    #region Camp
    [Header("Camp")]
    public GameObject[] camps;
    public GameObject[] campDecorations;
    #endregion

    #region BossRoomDecoratives
    //some decoratives for bossrooms.
    #endregion

    #endregion

    #endregion

    #region Monobehaviour
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        structpoints = new List<Vector3>();
        //not sure if i wanna use an objectpool for this. An objectpool would resolve this problem of not being able to get collider size of prefab but can have other problems i dont wanna deal with.
        //housesProto = new Dictionary<GameObject, GameObject>();
        //CreateProtoTypes();
    }

    private void Start()
    {
        terrainGenerator = TerrainGenerator.instance;
    }
    #endregion

    #region Client-Access Methods
    public void GenerateRoomDesign(RoomManager.ROOMTYPE roomtype, RoomManager _currRoom)
    {
        _currRoom.terrainGenerated = true;
        switch (roomtype)
        {
            default:
            case RoomManager.ROOMTYPE.SAVE_ROOM:
            case RoomManager.ROOMTYPE.TREASURE_ROOM:
                int random = Random.Range(0, 3);
                switch (random)
                {
                    case 0:
                        Town(_currRoom);
                        break;
                    case 1:
                        Farm(_currRoom);
                        break;
                    case 2:
                        Plantation(_currRoom);
                        break;
                }
                break;
            case RoomManager.ROOMTYPE.BOSSROOM:
                EntityData entityData = _currRoom.GetBossData();
                switch (entityData._name)
                {
                    case "WaterMage":
                        WaterMageRoom(_currRoom);
                        break;
                    case "FireKnight":
                        FireKnightRoom(_currRoom);
                        break;
                    case "BladeKeeper":
                        BladeKeeperRoom(_currRoom);
                        break;
                    case "Hashinshin":
                        HashinshinRoom(_currRoom);
                        break;
                    case "GroundMonk":
                        GroundMonkRoom(_currRoom);
                        break;
                }
                break;
        }

    }
    #endregion

    #region InternalMethods
    private void SpawnMandatoryObject(GameObject[] mandatoryObjs, RoomManager currRoom)
    {
        var selectedDeco = mandatoryObjs[Random.Range(0, mandatoryObjs.Length)];
        Instantiate(selectedDeco, currRoom.transform.position + Vector3.up * 2, Quaternion.identity, currRoom.transform);
    }

    private void SpawnGameObjects(GameObject[] decorations, bool compulsory, int min, int max, RoomManager currRoom)
    {
        int rand = Random.Range(min, max);
        int iterations = 200;
        Debug.Log(rand);
        Debug.Log("We decorated");
        while (iterations-- > 0 && rand > 0)
        {

            List<GameObject> shuffledDeck = Shuffle(decorations);
            foreach (GameObject deco in shuffledDeck)
            {
                var col = deco.GetComponentInChildren<HouseExteriorDesign>().col;
                Vector2 size = col.size;
                Vector3 roomCenter = currRoom.GetRoomAreaBounds().center;
                Vector3 pos = currRoom.GetRandomObjectPointGivenSize(size.magnitude);
                Vector3 offSet = col.offset;
                float dist = Vector3.Distance(pos + offSet, roomCenter);
                float maxDist = Vector3.Distance(roomCenter, currRoom.GetRoomAreaBounds().max);
                bool withinCertainRadius = dist / maxDist <= 0.5f;
                if (pos != Vector3.zero && withinCertainRadius && CheckDistanceFromStrucs(pos + offSet, roomCenter) && !CheckNotOutofBounds(size, pos + offSet, currRoom))
                {
                    GameObject decoCopy = Instantiate(deco, pos, Quaternion.identity, currRoom.transform);
                    rand--;
                    structpoints.Add(pos);
                    break;
                }
            }

        }


    }

    private List<GameObject> Shuffle(GameObject[] decorations)
    {
        List<GameObject> copy = new List<GameObject>(decorations);
        List<GameObject> shuffledDeck = new List<GameObject>();

        while (copy.Count > 0)
        {
            int rand = Random.Range(0, copy.Count);
            shuffledDeck.Add(copy[rand]);
            copy.Remove(copy[rand]);
        }

        return shuffledDeck;

    }

    private bool CheckNotOutofBounds(Vector2 size, Vector3 pos, RoomManager currRoom)
    {

        return Physics2D.OverlapBox(pos, size, 0f, LayerMask.GetMask("Obstacles", "HouseExterior", "HouseInterior"));
    }

    private bool CheckDistanceFromStrucs(Vector3 pos, Vector3 roomcenter)
    {
        if (Vector3.Distance(pos, roomcenter) >= minDistfromCenter)
        {
            if (structpoints.Count == 0)
            {
                return true;
            }
            return structpoints.TrueForAll(x => Vector3.Distance(x, pos) >= minDistBwStructs);
        }

        return false;

    }
    #endregion

    #region RoomGenerations
    /// <summary>
    /// A randomly generated town design.
    /// </summary>
    #region OtherRoomTypes
    private void Town(RoomManager currRoom)
    {
        SpawnGameObjects(houses, true, 4, 6, currRoom);
        SpawnMandatoryObject(townmandatoryDecorations, currRoom);

    }

    public void Swamp()
    {


    }

    public void Camp()
    {


    }

    public void Catacomb()
    {

    }

    public void Temple()
    {

    }

    public void Forest()
    {

    }
    #endregion

    #region perlinRooms

    public void Farm(RoomManager currRoom)
    {
        LoadRoomData("Farm", currRoom);
    }

    public void Plantation(RoomManager currRoom)
    {
        LoadRoomData("Plantation", currRoom);
    }

    #region BossRooms
    public void FireKnightRoom(RoomManager currRoom)
    {
        LoadRoomData("FireKnightRoom", currRoom);
    }

    public void WaterMageRoom(RoomManager currRoom)
    {
        LoadRoomData("WaterMageRoom", currRoom);
    }

    public void HashinshinRoom(RoomManager currRoom)
    {
        LoadRoomData("HashinshinRoom", currRoom);
    }

    public void GroundMonkRoom(RoomManager currRoom)
    {
        LoadRoomData("GroundMonkRoom", currRoom);
    }

    public void BladeKeeperRoom(RoomManager currRoom)
    {
        LoadRoomData("BladeKeeperRoom", currRoom);
    }

    private void LoadRoomData(string dataname, RoomManager currRoom)
    {
        float randomOffset = Random.Range(-0.05f, 0.05f);
        var RoomData = Resources.Load<LevelDesign>($"Data/LevelDesignData/{dataname}");
        RoomData.frequency += randomOffset;
        RoomData.normalizer += randomOffset;
        terrainGenerator.LoadLevelData(RoomData);
        terrainGenerator.GenerateTerrainType2(currRoom);
    }
    #endregion BossRooms

    #endregion PerlinRooms
    #endregion

    #region ToBeConsidered
    //private void CalculateBounds(GameObject go)
    //{
    //    Bounds bounds = go.gameObject.GetComponent<Collider2D>().bounds;
    //    bounds.size = Vector3.zero; // reset
    //    Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
    //    foreach (Collider2D col in colliders)
    //    {
    //        bounds.Encapsulate(col.bounds);
    //    }
    //}

    #endregion
}
