using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    Dictionary<RoomManager, DoorBehaviour[]> RoomDoors;
    List<DoorBehaviour> clearedDoors;

    private void Awake()
    {
        clearedDoors = new List<DoorBehaviour>();
        RoomDoors = new Dictionary<RoomManager, DoorBehaviour[]>();
        GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");
        foreach (GameObject room in rooms)
        {
            RoomManager _ROOM = room.GetComponent<RoomManager>();
            DoorBehaviour[] doors = _ROOM.GetDoors();
            if (doors.Length != 0)
            {
                RoomDoors.Add(_ROOM, doors);
            }

        }
    }


    public void clearDoor(RoomManager room, int index)
    {

        DoorBehaviour cleareddoor = RoomDoors[room][index];


        if (!clearedDoors.Contains(cleareddoor))
        {
            clearedDoors.Add(cleareddoor);
        }

        foreach (DoorBehaviour door in clearedDoors)
        {
            door.UnlockDoor();
            HashSet<RoomManager> roomManagers = door.GetRoomManagers();
            foreach (RoomManager roomc in roomManagers)
            {
                roomc.DisableCollider();
            }

        }

    }

    public void LockDoors(RoomManager room)
    {
        DoorBehaviour[] allDoors = RoomDoors[room];
        foreach (DoorBehaviour door in allDoors)
        {
            door.LockDoor();
        }

    }

    public void TemporaryOpen(RoomManager room, int index)
    {

        RoomDoors[room][index].UnlockDoor();

    }


}
