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
    private bool randomWalkRooms = false;
    private static List<Vector2Int> exits = new List<Vector2Int>();

    [Header("ItemWithText")]
    [SerializeField]
    private ItemWithTextData[] itemWithTextDatas;

    [Header("EnemyDatas")]
    [SerializeField]
    private EnemyData[] enemyDatas;

    [Header("TrapDatas")]
    [SerializeField]
    private TrapData[] trapDatas;

    [Header("PressureSwitches")]
    [SerializeField]
    private SwitchData[] switchDatas;

    [Header("PuzzleDatas")]
    [SerializeField]
    private GameObject[] puzzles;

    protected override void RunProceduralGeneration()
    {
        CreateRooms();
    }

    private void CreateRooms()
    {
        var roomsList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPosition, new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);

        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

        if (randomWalkRooms)
        {
            floor = CreateRoomsRandomly(roomsList);
        }
        else
        {
            floor = CreateSimpleRooms(roomsList);
        }

        HashSet<Vector2Int> corridors = ConnectRooms(roomsList);
        floor.UnionWith(corridors);

        tilemapVisualizer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, tilemapVisualizer);
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

    private HashSet<Vector2Int> ConnectRooms(List<BoundsInt> rooms)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var currentRoom = rooms[Random.Range(0, rooms.Count)];
        rooms.Remove(currentRoom);

        List<RoomManager> roomMgrs = new List<RoomManager>();
        //List<Vector2Int> roomCenters = new List<Vector2Int>();
        int i = 1;
        GameObject RoomsContainer = new GameObject("RoomsContainer");
        
        while (rooms.Count > 0)
        {
            RoomManager roommgr = InstantiateRoom(currentRoom, i);
            roommgr.gameObject.transform.SetParent(RoomsContainer.transform);
            roomMgrs.Add(roommgr);
            //roomCenters.Add((Vector2Int) Vector3Int.RoundToInt(room.center));
            i++;
            BoundsInt closest = FindClosestPointTo((Vector2Int)Vector3Int.RoundToInt(currentRoom.center), rooms);
            rooms.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoom, closest);
            AddToExit(currentRoom, closest, newCorridor,roommgr);
            currentRoom = closest;
            corridors.UnionWith(newCorridor);
        }
        return corridors;
    }

    private void AddToExit(BoundsInt currentRoom, BoundsInt closest, HashSet<Vector2Int> newCorridor, RoomManager currRoom)
    {
        Vector2Int exit = Vector2Int.zero;
        foreach (Vector2Int vec in newCorridor)
        {
            if (vec.x == currentRoom.xMax ||
                vec.x == currentRoom.xMin ||
                vec.y == currentRoom.yMin ||
                vec.y == currentRoom.yMax)
            {
                exits.Add(vec);
                exit = vec;
            } 

            //if (vec.x == closest.xMax ||
            //    vec.x == closest.xMin ||
            //    vec.y == closest.yMin || 
            //    vec.y == closest.yMax)
            //{
            //    exits.Add(vec);
            //}
        }
        if (exit != Vector2Int.zero)
        {
            DoorBehaviour door = CreateDoor(exit);
            door.transform.SetParent(currRoom.transform);
            currRoom.SettingExitDoor(door);
        } 
        
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

    private RoomManager InstantiateRoom(BoundsInt room, int i)
    {
        RoomManager.ROOMTYPE randRoomType = (RoomManager.ROOMTYPE) Random.Range(0, (int) RoomManager.ROOMTYPE.COUNT);
        GameObject go = new GameObject($"Room{i}");
        BoxCollider2D boxCollider2D = go.AddComponent<BoxCollider2D>();
        boxCollider2D.size = new Vector2(room.xMax - room.xMin - 7f, room.yMax - room.yMin - 7f);
        go.transform.position = room.center;
        RoomManager createdroom;
        switch (randRoomType)
        {
            default:
            case RoomManager.ROOMTYPE.TREASURE_ROOM:
                createdroom = go.AddComponent<TreasureRoom_Mgr>();
                createdroom.SetUpEntityDatas(RandomizeTreasureDatas());
                break;
            case RoomManager.ROOMTYPE.PUZZLE_ROOM:
                createdroom = go.AddComponent<TorchPuzzleRoom_Mgr>();
                GameObject selectedpuzzle = RandomizePuzzle();
                selectedpuzzle.transform.position = GetRandomPointInCollider(room);
                selectedpuzzle.transform.SetParent(createdroom.transform);
                break;
            case RoomManager.ROOMTYPE.FIGHTING_ROOM:
                var fightroom = go.AddComponent<FightRoom_Mgr>();
                fightroom.waveNum = Random.Range(1, 5);
                createdroom = fightroom;
                createdroom.SetUpEntityDatas(RandomizeEnemyDatas());
                break;
            case RoomManager.ROOMTYPE.PUZZLE2_ROOM:
                createdroom = go.AddComponent<PressurePlateRoom_Mgr>();
                createdroom.SetUpEntityDatas(RandomizePressureSwitchDatas());
                break;
            /**
             * Everything below just uses fighting rooms as placeholders for now.
             */
            case RoomManager.ROOMTYPE.BOSSROOM:
                var bossroom = go.AddComponent<FightRoom_Mgr>();
                bossroom.waveNum = Random.Range(1, 5);
                createdroom = bossroom;
                createdroom.SetUpEntityDatas(RandomizeEnemyDatas());
                break;
            case RoomManager.ROOMTYPE.HYBRID_ROOM:
                var hybridroom = go.AddComponent<FightRoom_Mgr>();
                hybridroom.waveNum = Random.Range(1, 5);
                createdroom = hybridroom;
                createdroom.SetUpEntityDatas(RandomizeEnemyDatas());
                break;
            case RoomManager.ROOMTYPE.ROOMBEFOREBOSS:
                var roombeforeboss = go.AddComponent<FightRoom_Mgr>();
                roombeforeboss.waveNum = Random.Range(1, 5);
                createdroom = roombeforeboss;
                createdroom.SetUpEntityDatas(RandomizeEnemyDatas());
                break;
        }

        
       
        createdroom.RoomIndex = i;
        createdroom.roomtype = randRoomType;
        

        return createdroom;
        
    }

    public Vector2 GetRandomPointInCollider(BoundsInt bounds)
    {
        return new Vector2(
            Random.Range(bounds.xMin, bounds.xMax),
            Random.Range(bounds.yMin, bounds.yMax));
        //Vector2 randomPoint;
        ////do
        ////{
        //    //AABB axis - 4 possible bounds, top left, top right, bl, br
        //    //tl = min.x, max.y;
        //    //bl = min.x, min.y;
        //    //tr = max.x, max.y;
        //    //br = max.x, min.y;
        //    //anywhere within these 4 bounds are possible pts;
            

        ////} while (!col.OverlapPoint(randomPoint)); //&& !Physics2D.OverlapCircle(randomPoint, 1, LayerMask.GetMask("Obstacles"))
        ////&& safeRoute.Contains(randomPoint));
        //return randomPoint;
    }

    private DoorBehaviour CreateDoor(Vector2Int exit)
    {

        GameObject go = new GameObject("Door");
        go.AddComponent<Animator>();
        go.AddComponent<BoxCollider2D>();
        DoorBehaviour door = go.AddComponent<DoorBehaviour>();
        go.transform.position = new Vector2(exit.x,exit.y);

        return door;
    }
}
