using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool connected;
    public bool isThereARoomHere;
    public GameObject roomAtSpawnLocation;
    public GameObject spawnRoomLocation;
    public GameObject wall;
    public GameObject hallway;
    public GameObject door;
    public AudioSource closeSound;
    public void Connect()
    {
        hallway.SetActive(true);
        wall.SetActive(false);
        connected = true;
        isThereARoomHere = true;
    }

    public void CloseDoors()
    {
        closeSound.Play();
        door.SetActive(true);
    }
    public void OpenDoors()
    {
        closeSound.Play();
        door.SetActive(false);
    }
}