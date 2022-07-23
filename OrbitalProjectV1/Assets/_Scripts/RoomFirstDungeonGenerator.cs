using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

public class RoomFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField]
    private int minRoomWidth = 4, minRoomHeight = 4;
    [SerializeField]
    private int dungeonWidth = 20, dungeonHeight = 20;
    [SerializeField]
    [Range(0,10)]
    private int offset = 1;

    [SerializeField]
    [Range(0, 1)]
    private float groundoffSet = 0.2f;

    [SerializeField]
    private bool randomWalkRooms = false;

    [Header("Random Seed")]
    public int currentSeed = -1;

    private static List<Vector2Int> exits = new List<Vector2Int>();
    private static HashSet<Vector2Int> seen = new HashSet<Vector2Int>();
    private HashSet<DoorBehaviour> doors;
    public HashSet<Vector2Int> corridors { get; private set; }
    private static Dictionary<BoundsInt, RoomManager> bound2Rooms = new Dictionary<BoundsInt, RoomManager>();

    [Header("Doors")]
    [SerializeField]
    private Sprite doorSprite;
    [SerializeField]
    private GameObject textCanvas;

    [Header("ItemWithText")]
    [SerializeField]
    private ItemWithTextData[] itemWithTextDatas;

    [Header("EnemyDatas")]
    [SerializeField]
    private EnemyData[] enemyDatas;

    [Header("BossData")]
    [SerializeField]
    private List<EnemyData> bosses;

    [Header("TrapDatas")]
    [SerializeField]
    private TrapData[] trapDatas;

    [Header("PressureSwitches")]
    [SerializeField]
    private SwitchData[] switchDatas;

    [Header("PuzzleDatas")]
    [SerializeField]
    private GameObject[] puzzles;

    [Header("PortalData")]
    [SerializeField]
    private ItemWithTextData portal;

    private _GameManager gameManager;


    public void GenerateRandomSeed()
    {
        int tempSeed = (int)System.DateTime.Now.Ticks;
        Random.InitState(tempSeed);
    }


    public void SetRandomSeed(int seed)
    {
        Random.InitState(seed);
    }


    protected override void RunProceduralGeneration()
    {
        seen.Clear();
        if (currentSeed == -1)
        {
            GenerateRandomSeed();
        }
        else
        {
            SetRandomSeed(currentSeed);
        }
        CreateRooms();
        ChangeRoomsToBoss();
        ChangeRoom(0, _GameManager.roomManagers, RoomManager.ROOMTYPE.TREASURE_ROOM);
        _GameManager.roomManagers[0].SetUpEntityDatas(RandomizeTreasureDatas());
    }

    private void CreateRooms()
    {
        var roomsList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPosition, new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);
        _GameManager.roomManagers.Clear();
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        doors = new HashSet<DoorBehaviour>();
        if (randomWalkRooms)
        {
            floor = CreateRoomsRandomly(roomsList);
        }
        else
        {
            floor = CreateSimpleRooms(roomsList);
        }


        corridors = ConnectRooms(roomsList);

        floor.UnionWith(corridors);
        tilemapVisualizer.PaintFloorTiles(floor, groundoffSet, roomsList, corridors);
        tilemapVisualizer.PaintCorridoorTiles(corridors);
        WallGenerator.CreateWalls(floor, tilemapVisualizer);
        tilemapVisualizer.PaintDecorations();
    }

    public void DestroyAllCollidingDoors()
    {
        foreach (DoorBehaviour door in doors)
        {
            if (CheckDoorsTouchingWalls(door))
            {
                DestroyImmediate(door.gameObject);
            }
        }
    }

    private void GetRoomCenters(List<BoundsInt> roomList, HashSet<Vector2Int> roomCenters)
    {
        foreach(BoundsInt bound in roomList)
        {
            roomCenters.Add((Vector2Int) Vector3Int.RoundToInt(bound.center));
        }
        
    }

    private HashSet<Vector2Int> CreateRoomsRandomly(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        for (int i = 0; i < roomsList.Count; i++)
        {
            var roomBounds = roomsList[i];
            var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
            var roomFloor = RunRandomWalk(randomWalkParameters, roomCenter);
            foreach (var position in roomFloor)
            {
                if(position.x >= (roomBounds.xMin + offset) && position.x <= (roomBounds.xMax - offset) && position.y >= (roomBounds.yMin - offset) && position.y <= (roomBounds.yMax - offset))
                {
                    floor.Add(position);
                }
            }
        }
        return floor;
    }

    //private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    //{
    //    HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
    //    var currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
    //    roomCenters.Remove(currentRoomCenter);

    //    while (roomCenters.Count > 0)
    //    {
    //        Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
    //        roomCenters.Remove(closest);
    //        HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
    //        currentRoomCenter = closest;
    //        corridors.UnionWith(newCorridor);
    //    }
    //    return corridors;
    //}

    /// <summary>
    /// Connect Rooms
    /// </summary>
    /// <param name="rooms"></param>
    /// <returns></returns>
    private HashSet<Vector2Int> ConnectRooms(List<BoundsInt> rooms)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var currentRoom = rooms[Random.Range(0, rooms.Count)];
        List<BoundsInt> allrooms = new List<BoundsInt>(rooms);
        allrooms.Remove(currentRoom);
        List<RoomManager> roomMgrs = new List<RoomManager>();
        int i = 1;
        GameObject RoomsContainer = new GameObject("RoomsContainer");
        
        while (allrooms.Count > 0)
        {
            

            RoomManager roommgr = InstantiateRoom(currentRoom, i, RoomsContainer);
            //if (i == 1)
            //{
            //    SpawnPlayer(currentRoom, roommgr);
            //}
            bound2Rooms[currentRoom] = roommgr;
            roomMgrs.Add(roommgr);
            i++;
            BoundsInt closest = FindClosestPointTo((Vector2Int)Vector3Int.RoundToInt(currentRoom.center), allrooms);
            allrooms.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoom, closest);
            
            CreateDoorIfInsideAnyOtherRooms(currentRoom, closest, newCorridor, roommgr, rooms);
            currentRoom = closest;
            corridors.UnionWith(newCorridor);
        }

        //for last room
        if (allrooms.Count == 0)
        {
            RoomManager roommgr = InstantiateRoom(currentRoom, i, RoomsContainer);
            roomMgrs.Add(roommgr);
            i++;
        }
        return corridors;
    }

    private void CreateDoorIfInsideAnyOtherRooms(BoundsInt currentRoom, BoundsInt closest, HashSet<Vector2Int> newCorridor, RoomManager currRoom, List<BoundsInt> allRooms)
    {
        HashSet<BoundsInt> visited = new HashSet<BoundsInt>();
        
        foreach (Vector2Int vec in newCorridor)
        {
            foreach (BoundsInt room in allRooms)
            {
                if (insideRoom(vec, room)/*&& !visited.Contains(room)*/) {

                    visited.Add(room);
                    if ((vec.x == room.xMax - offset||
                    vec.x == room.xMin + offset||
                    vec.y == room.yMin + offset||
                    vec.y == room.yMax - offset))
                    {
                        //string dir = CheckDoorDirection(vec,room);
                        bool left = vec.x == room.xMin + offset && (vec.y != room.yMax - offset || vec.y != room.yMin + offset);
                        bool down = vec.y == room.yMin + offset && (vec.x != room.xMax - offset || vec.x != room.xMin + offset);
                        if (!seen.Contains(vec))
                        {
                            seen.Add(vec);
                            exits.Add(vec);
                            DoorBehaviour door = CreateDoor(vec,left,down);
                            if (door != null)
                            {
                                doors.Add(door);
                                door.transform.SetParent(currRoom.transform, true);
                                currRoom.SettingExitDoor(door);
                                var visitedroom = bound2Rooms.GetValueOrDefault(room, null);
                                if (visitedroom != null)
                                {
                                    visitedroom.SettingExitDoor(door);
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }       
    }

    //private string CheckDoorDirection(Vector2Int vec, BoundsInt room)
    //{
    //    if (vec.x == room.xMin + offset)
    //    {
    //        return "left";
    //    } else if (vec.y == room.yMin + offset)
    //    {
    //        return "down";
    //    } else if (vec.x == room.xMin + offset && vec)
        
    //}

    private bool insideRoom(Vector2Int vec, BoundsInt closest)
    {
        return closest.xMin + offset <= vec.x && vec.x <= closest.xMax - offset && closest.yMin + offset <= vec.y && vec.y <= closest.yMax - offset;
    }

    private HashSet<Vector2Int> CreateCorridor(BoundsInt currentRoomCenter, BoundsInt destination)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var position = (Vector2Int)Vector3Int.RoundToInt(currentRoomCenter.center);
        corridor.Add(position);

        while (position.y != ((Vector2Int) Vector3Int.RoundToInt(destination.center)).y)

        {
            
            if (destination.center.y > position.y)
            {
                position += Vector2Int.up;
            }
            else if (destination.center.y < position.y)
            {
                position += Vector2Int.down;
            }
            corridor.Add(position);
        }
        while (position.x != ((Vector2Int)Vector3Int.RoundToInt(destination.center)).x)
        {
            if (destination.center.x > position.x)
            {
                position += Vector2Int.right;
            }
            else if (destination.center.x < position.x)
            {
                position += Vector2Int.left;
            }
            corridor.Add(position);
        }
        return corridor;
    }


    //private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    //{
    //    HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
    //    var position = currentRoomCenter;
    //    corridor.Add(position);
    //    while (position.y != destination.y)
    //    {
    //        if (destination.y > position.y)
    //        {
    //            position += Vector2Int.up;
    //        }
    //        else if (destination.y < position.y)
    //        {
    //            position += Vector2Int.down;
    //        }
    //        corridor.Add(position);
    //    }
    //    while (position.x != destination.x)
    //    {

    //        if (destination.x > position.x)
    //        {
    //            position += Vector2Int.right;
    //        }
    //        else if (destination.x < position.x)
    //        {
    //            position += Vector2Int.left;
    //        }
    //        corridor.Add(position);
    //    }
    //    return corridor;
    //}

    //private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    //{
    //    Vector2Int closest = Vector2Int.zero;
    //    float distance = float.MaxValue;
    //    foreach (var position in roomCenters)
    //    {
    //        float currentDistance = Vector2.Distance(position, currentRoomCenter);
    //        if (currentDistance < distance)
    //        {
    //            distance = currentDistance;
    //            closest = position;
    //        }
    //    }
    //    return closest;
    //}

    private BoundsInt FindClosestPointTo(Vector2Int currentRoomCenter, List<BoundsInt> rooms)
    {
        BoundsInt closest = new BoundsInt();
        float distance = float.MaxValue;
        foreach (var room in rooms)
        {
            float currentDistance = Vector2.Distance(room.center, currentRoomCenter);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = room;
            }
        }
        return closest;
    }


    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        foreach (var room in roomsList)
        {
            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(position);
                }
            }
        }
        return floor;
    }

    private ItemWithTextData[] RandomizeTreasureDatas()
    {
        List<ItemWithTextData> edClones = new List<ItemWithTextData>();

        int rand = Random.Range(1, 2);
        while (rand-- > 0)
        {
            int rand2 = Random.Range(0, itemWithTextDatas.Length);
            ItemWithTextData clone = Instantiate(itemWithTextDatas[rand2]);
            clone.random = true;
            clone.spawnAtStart = true;
            edClones.Add(clone);
        }


        return edClones.ToArray();

    }

    private EnemyData[] RandomizeEnemyDatas()
    {
        List<EnemyData> edClones = new List<EnemyData>();

        int rand = Random.Range(1, 7);
        while (rand-- > 0)
        {
            int rand2 = Random.Range(0, enemyDatas.Length);
            EnemyData clone = Instantiate(enemyDatas[rand2]);
            clone.random = true;
            clone.spawnAtStart = true;
            edClones.Add(clone);
        }


        return edClones.ToArray();

    }

    /**
     * For pressure switches, we also need to instantiate boxes.
     */
    private SwitchData[] RandomizePressureSwitchDatas()
    {
        List<SwitchData> edClones = new List<SwitchData>();

        int rand = Random.Range(2, 4);
        while (rand-- > 0)
        {
            int rand2 = Random.Range(0, switchDatas.Length);
            SwitchData clone = Instantiate(switchDatas[rand2]);
            clone.random = true;
            clone.spawnAtStart = true;
            edClones.Add(clone);
        }


        return edClones.ToArray();

    }



    private GameObject RandomizePuzzle()
    {

        int rand = Random.Range(0, puzzles.Length);
        GameObject clone = Instantiate(puzzles[rand]);

        return clone;

    }


    private RoomManager InstantiateRoomTesting(BoundsInt room, int i, GameObject container)
    {
        GameObject go = new GameObject($"Room{i}");
        go.transform.SetParent(container.transform, true);
        go.transform.position = room.center;
        go.layer = LayerMask.NameToLayer("spawnArea");
        BoxCollider2D boxCollider2D = go.AddComponent<BoxCollider2D>();
        boxCollider2D.isTrigger = true;
        boxCollider2D.size = new Vector2(room.xMax - room.xMin - 7f, room.yMax - room.yMin - 7f);
        RoomManager createdroom = go.AddComponent<TreasureRoom_Mgr>();
        createdroom.SetUpRoomSize((Vector2Int) room.size);
        createdroom.SetUpEntityDatas(RandomizeTreasureDatas());
        createdroom.RoomIndex = i;
        //20% chance
        if (Random.Range(0, 10) >= 8)
        {
            createdroom.SetUpPortal(portal);
        }

        _GameManager.roomManagers.Add(createdroom);
        return createdroom;
    }

    private RoomManager InstantiateRoom(BoundsInt room, int i, GameObject container)
    {
        RoomManager.ROOMTYPE randRoomType = (RoomManager.ROOMTYPE)Random.Range(0, (int) RoomManager.ROOMTYPE.ROOMBEFOREBOSS);
        GameObject go = new GameObject($"Room{i}");
        go.transform.SetParent(container.transform,true);
        go.transform.position = room.center;
        go.layer = LayerMask.NameToLayer("spawnArea");
        BoxCollider2D boxCollider2D = go.AddComponent<BoxCollider2D>();
        boxCollider2D.isTrigger = true;
        boxCollider2D.size = new Vector2(room.xMax - room.xMin - 7f, room.yMax - room.yMin - 7f);
        
        RoomManager createdroom;
        switch (randRoomType)
        {
            default:
            case RoomManager.ROOMTYPE.TREASURE_ROOM:
                createdroom = go.AddComponent<TreasureRoom_Mgr>();
                createdroom.SetUpEntityDatas(RandomizeTreasureDatas());
                break;
            case RoomManager.ROOMTYPE.PUZZLE_ROOM:
                createdroom = go.AddComponent<PuzzleRoom_Mgr>();
                //GameObject selectedpuzzle = RandomizePuzzle();
                //selectedpuzzle.transform.position = GetRandomPointInCollider(room);
                //selectedpuzzle.transform.SetParent(createdroom.transform);
                break;
            case RoomManager.ROOMTYPE.FIGHTING_ROOM:
                var fightroom = go.AddComponent<FightRoom_Mgr>();
                fightroom.waveNum = Random.Range(1, 5);
                createdroom = fightroom;
                createdroom.SetUpEntityDatas(RandomizeEnemyDatas());
                break;
            case RoomManager.ROOMTYPE.PUZZLE2_ROOM:
                createdroom = go.AddComponent<PuzzleRoom_Mgr>();
                //createdroom.SetUpEntityDatas(RandomizePressureSwitchDatas());
                break;
            case RoomManager.ROOMTYPE.HYBRID_ROOM:
                var hybridroom = go.AddComponent<HybridRoom_Mgr>();
                createdroom = hybridroom;
                createdroom.SetUpEntityDatas(RandomizeEnemyDatas());
                break;
            
        }

        createdroom.roomtype = randRoomType;
        createdroom.SetUpRoomSize((Vector2Int)room.size - Vector2Int.one * 3);
        createdroom.RoomIndex = i;
        createdroom.roomtype = randRoomType;
        if (Random.Range(0, 10) >= 8)
        {
            createdroom.SetUpPortal(portal);
        }

        _GameManager.roomManagers.Add(createdroom);
        return createdroom;
        
    }

    /// <summary>
    /// Change 4 rooms to before boss and boss.
    /// </summary>
    private void ChangeRoomsToBoss()
    {
        int i = 2;
        while (i-- > 0)
        {
            List<RoomManager> rooms = _GameManager.roomManagers;
            int rand;
            if (i == 1)
            {
                rand = Random.Range((int)(0.45f * rooms.Count), (int)(0.6f * rooms.Count));
            } else
            {
                rand = Random.Range((int)(0.8f * rooms.Count), rooms.Count);
            }
            ChangeRoom(rand, rooms, RoomManager.ROOMTYPE.BOSSROOM);
            if (rand - 1 >= 0)
            {
                ChangeRoom(rand - 1, rooms, RoomManager.ROOMTYPE.ROOMBEFOREBOSS);

            }
            
        }
        
    }

    private void ChangeRoom(int index, List<RoomManager> rooms, RoomManager.ROOMTYPE rOOMTYPE)
    {
        RoomManager currRoom = rooms[index];
        DoorBehaviour[] doorsincurr = currRoom.GetDoors();
        GameObject currroomGameObject = currRoom.gameObject;
        Vector2Int roomSize = currRoom.GetRoomSize();
        DestroyImmediate(currRoom);
        RoomManager newroom;
        switch (rOOMTYPE)
        {
            default:
            case RoomManager.ROOMTYPE.ROOMBEFOREBOSS:
                newroom = currroomGameObject.AddComponent<TreasureRoom_Mgr>();
                break;
            case RoomManager.ROOMTYPE.BOSSROOM:
                newroom = currroomGameObject.AddComponent<BossRoom_Mgr>();
                if (bosses.Count == 0)
                {
                    Debug.LogError("Did not allocate any bosses");
                } else
                {
                    EnemyData selectedboss = bosses[UnityEngine.Random.Range(0, bosses.Count - 1)];
                    bosses.Remove(selectedboss);
                    newroom.SetUpEntityData(selectedboss);
                }
                
                break;

        }
        if (doorsincurr != null)
        {
            foreach (DoorBehaviour door in doorsincurr)
            {
              
                newroom.SettingExitDoor(door);
                
            }
        }

        newroom.RoomIndex = index + 1;
        newroom.roomtype = rOOMTYPE;
        newroom.SetUpRoomSize(roomSize);
        _GameManager.roomManagers[index] = newroom;


    }

    /// <summary>
    /// Get A Random Point in the Collider given
    /// </summary>
    /// <param name="bounds"></param>
    /// <returns>Random Point</returns>
    public Vector2 GetRandomPointInCollider(BoundsInt bounds)
    {
        return new Vector2(
            Random.Range(bounds.xMin, bounds.xMax),
            Random.Range(bounds.yMin, bounds.yMax));
    }

    /// <summary>
    /// Parent Method for creation of door.
    /// </summary>
    /// <param name="exit"></param>
    /// <param name="left"></param>
    /// <param name="down"></param>
    /// <returns>Door</returns>
    private DoorBehaviour CreateDoor(Vector2Int exit, bool left, bool down)
    {
        GameObject go = new GameObject("Door");
        go.layer = LayerMask.NameToLayer("Doors");
        DoorBehaviour door = InstantiatingDoorDefaults(go);
        SettingDoorData(door);
        SettingDoorTransform(exit, left, down, door);
        if (CheckDoorsTouchingWalls(door))
        {
            Destroy(door);
            return null;
        } else
        {
            InstantiatingTextLogic(go);
            return door;
        }
        
    }

    /// <summary>
    /// Instantiating default items before adding door behaviour.
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    private DoorBehaviour InstantiatingDoorDefaults(GameObject go)
    {
        go.AddComponent<Animator>();
        SpriteRenderer _spriteRenderer = go.AddComponent<SpriteRenderer>();
        _spriteRenderer.sortingOrder = 1;
        go.AddComponent<AudioSource>();
        BoxCollider2D col = go.AddComponent<BoxCollider2D>();
        DoorBehaviour door = go.AddComponent<BreakableDoorBehaviour>();
     
        return door;
    }

    /// <summary>
    /// Adding textLogic for breakable door behaviour.
    /// </summary>
    /// <param name="go"></param>
    private void InstantiatingTextLogic(GameObject go)
    {
        GameObject Tcinstance = Instantiate(textCanvas);
        Tcinstance.transform.SetParent(go.transform);
        Tcinstance.transform.localPosition = new Vector3(0, 4f);
    }


    /// <summary>
    /// Setting Up Door Transform.
    /// </summary>
    /// <param name="exit"></param>
    /// <param name="left"></param>
    /// <param name="down"></param>
    /// <param name="go"></param>
    private void SettingDoorTransform(Vector2Int exit, bool left, bool down, DoorBehaviour door)
    {
        door.transform.position = new Vector2(exit.x, exit.y);
        door.transform.position += left ? new Vector3(-0.5f, 0.5f) : down ? new Vector3(0.5f, -0.5f) : new Vector3(0.5f, 0.5f);
        door.transform.localScale = new Vector2(0.6f, 0.6f);
        
        
    }

    /// <summary>
    /// Setting Up Door Scriptable Data.
    /// </summary>
    /// <param name="door"></param>
    private void SettingDoorData(DoorBehaviour door)
    {
        door.enabled = false;
        DoorData doorData = ScriptableObject.CreateInstance<DoorData>();
        doorData._name = "UNLOCK";
        doorData.minDist = 1.5f;
        doorData.sprite = doorSprite;
        doorData._type = EntityData.TYPE.DOOR;
        door.SetEntityStats(doorData);
        door.enabled = true;
    }

    private bool CheckDoorsTouchingWalls(DoorBehaviour door)
    {
        bool isTouching = Physics2D.OverlapCircle(door.transform.position, 0.01f, LayerMask.GetMask("Obstacles"));
        //List<Collider2D> colliders = new List<Collider2D>();
        //ContactFilter2D contactFilter2D = new ContactFilter2D();
        //contactFilter2D.SetLayerMask(LayerMask.GetMask("Obstacles"));
        //int isTouching = door.GetComponent<Collider2D>().OverlapCollider(contactFilter2D, colliders);
        if (isTouching)
        {
            return true;
        }
        return false;
    }

}
