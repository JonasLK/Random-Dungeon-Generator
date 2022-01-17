using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerationCoroutine : MonoBehaviour
{
    public int maxRoomAmount;
    private int currentRoomAmount;
    public int spawnRoomChance;
    public int connectDoorChance;
    public GameObject[] roomPrefabs;
    public List<GameObject> rooms;
    [HideInInspector] public List<GameObject> occupiedCoordinates;
    [HideInInspector] public List<GameObject> tempRooms;
    private GameObject justSpawnedRoom;
    private Room roomScript;
    private Door doorScript;
    private int doorNumber;
    private int doorNumber2;
    private int coordinatesChecker;

    void Start()
    {
        occupiedCoordinates.Add(rooms[0]);
        StartCoroutine(InstanciateRooms());
    }
    public IEnumerator InstanciateRooms()
    {
        foreach (GameObject Room in rooms)
        {
            roomScript = Room.GetComponent<Room>();
            foreach (GameObject door in roomScript.Doors)
            {
                doorScript = door.GetComponent<Door>();
                if (currentRoomAmount < maxRoomAmount - 1)
                {
                    if (doorScript.connected == false && Random.Range(0, spawnRoomChance) == 0)
                    {
                        if (doorScript.isThereARoomHere == false)
                        {
                            foreach (GameObject coordinates in occupiedCoordinates)
                            {
                                if (doorScript.spawnRoomLocation.transform.position == coordinates.transform.position)
                                {
                                    //print(coordinates.transform.position);
                                    coordinatesChecker++;
                                    doorScript.isThereARoomHere = true;
                                }
                            }
                            if (coordinatesChecker < 1)
                            {
                                justSpawnedRoom = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Length)], doorScript.spawnRoomLocation.transform.position, Quaternion.identity);
                                if (currentRoomAmount == maxRoomAmount - 2)
                                {
                                    justSpawnedRoom.GetComponent<Room>().winRoom = true;
                                }
                                //print(justSpawnedRoom.transform.position);
                                occupiedCoordinates.Add(justSpawnedRoom);
                                tempRooms.Add(justSpawnedRoom);
                                currentRoomAmount++;
                                DecideDoor(justSpawnedRoom, door, doorNumber);
                                foreach (GameObject justSpawnedDoor in justSpawnedRoom.GetComponent<Room>().Doors)
                                {
                                    doorScript = justSpawnedDoor.GetComponent<Door>();
                                    foreach (GameObject coordinates in occupiedCoordinates)
                                    {
                                        if (doorScript.spawnRoomLocation.transform.position == coordinates.transform.position)
                                        {
                                            doorScript.isThereARoomHere = true;
                                            doorScript.roomAtSpawnLocation = coordinates;
                                        }
                                    }
                                    if (doorScript.roomAtSpawnLocation != null)
                                    {
                                        if (doorScript.connected == false)
                                        {
                                            if (Random.Range(0, connectDoorChance) == 0)
                                            {
                                                DecideDoor(doorScript.roomAtSpawnLocation, justSpawnedDoor, doorNumber2);
                                            }
                                        }
                                        doorScript.roomAtSpawnLocation = null;
                                    }
                                    doorNumber2++;
                                }
                                doorNumber2 = 0;
                            }
                            else
                            {
                                coordinatesChecker = 0;
                            }
                        }
                    }
                    doorNumber++;
                }
            }
            doorNumber = 0;
        }
        if (tempRooms.Count > 0)
        {
            foreach (GameObject room in tempRooms)
            {
                rooms.Add(room);
            }
            tempRooms.Clear();
        }
        yield return new WaitForSeconds(0.1f);
        if (currentRoomAmount < maxRoomAmount - 1)
        {
            StartCoroutine(InstanciateRooms());
        }
    }

    private void DecideDoor(GameObject spawned, GameObject otherDoor, int door)
    {
        if (door < 2)
        {
            door += 2;
        }
        else
        {
            door -= 2;
        }

        spawned.GetComponent<Room>().Doors[door].GetComponent<Door>().Connect();
        otherDoor.GetComponent<Door>().Connect();
    }
}