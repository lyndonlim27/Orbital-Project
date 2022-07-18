using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class RoomDesign : MonoBehaviour
{

    public static RoomDesign instance { get; private set; }
    private TerrainGenerator terrainGenerator;
    private List<Vector3> structpoints;
    [SerializeField]
    [Range(0,100)]
    private float minDistBwStructs;

    [SerializeField]
    [Range(0, 100)]
    private float minDistfromCenter;

    public enum ROOM_DESIGNS
    {
        SWAMP,
        CAMP,
        TOWN,
        CATACOMB,
        TEMPLE,
        FARM,
        FOREST, /**DEFAULT**/

    }

    [Header("Decorations")]
    [Header("Town")]
    public GameObject[] houses;
    //public Dictionary<GameObject,GameObject> housesProto;
    public GameObject[] townmandatoryDecorations;
    // decided to do it inside house prefab instead.
    //public Tilemap houseInterior;
    //public TileBase interiorlu, interiorru, interirorld, interiorrd, interiorhoriz, interiorvert, interiorbase;


    [Header("Farm")]
    public GameObject[] farms;
    public GameObject[] farmDecorations;


    [Header("Camp")]
    public GameObject[] camps;
    public GameObject[] campDecorations;




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

    //private void CreateProtoTypes()
    //{
    //    foreach(GameObject house in houses)
    //    {
    //        var proto = Instantiate(house);
    //        proto.SetActive(false);
    //        housesProto.Add(house,proto);
    //    }
    //}

    public void GenerateRoomDesign(RoomManager.ROOMTYPE roomtype, RoomManager _currRoom)
    {
        switch (roomtype)
        {
            case RoomManager.ROOMTYPE.SAVE_ROOM:
            case RoomManager.ROOMTYPE.TREASURE_ROOM:
                //Town(_currRoom);
                Farm(_currRoom);
                break;
        }

    }


    /// <summary>
    /// A randomly generated town design.
    /// </summary>
    private void Town(RoomManager currRoom)
    {

        SpawnGameObjects(houses, true, 4, 6, currRoom);
        SpawnMandatoryObject(townmandatoryDecorations, currRoom);

    }

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
            foreach(GameObject deco in shuffledDeck)
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
            
            //GameObject prototype = housesProto[selecteddeco];
            
            
            
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

        return Physics2D.OverlapBox(pos, size, 0f, LayerMask.GetMask("Obstacles","HouseExterior","HouseInterior"));
        //return !prototype.GetComponentInChildren<HouseExteriorDesign>(true).col.IsTouchingLayers(LayerMask.GetMask("Obstacles"));
    }


    //public void CalculateBounds(GameObject go)
    //{
    //    Bounds bounds = go.gameObject.GetComponent<Collider2D>().bounds;
    //    bounds.size = Vector3.zero; // reset
    //    Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
    //    foreach (Collider2D col in colliders)
    //    {
    //        bounds.Encapsulate(col.bounds);
    //    }
    //}

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

    //private void DrawHouseInterior(Bounds boundsInt) 
    //{
    //    var min = Vector2Int.FloorToInt(boundsInt.min);
    //    var max = Vector2Int.FloorToInt(boundsInt.max);
    //    for (int i = min.x; i < max.x; i++)
    //    {
    //        for (int j = min.y; j < max.y; j++)
    //        {
    //            TileBase paintedTile;
    //            //handling corners
    //            if (i == min.x && j == min.y)
    //            {

    //            }
    //            //houseInterior.SetTile()
    //        }
    //    }
    //}

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

    public void Farm(RoomManager currRoom)
    {
        terrainGenerator.GenerateTerrain(currRoom);
    }


    public void Forest()
    {

    }

}
