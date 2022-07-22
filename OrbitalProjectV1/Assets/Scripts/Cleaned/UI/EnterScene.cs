using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Video;

public class EnterScene : MonoBehaviour
{

    [SerializeField] private string videoName;
    DataPersistenceManager dataPersistenceManager;
    string roomName;

    private void Awake()
    {
        // videoPlayer = GetComponent<VideoPlayer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        dataPersistenceManager = FindObjectOfType<DataPersistenceManager>();
        roomName = gameObject.name+ ":" + GetComponent<RoomManager>().RoomIndex;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player") && this.enabled == true)
           WaitForCutScene();
    }

    private void WaitForCutScene()
    {
        SerializableDictionary<string, int> rooms = dataPersistenceManager.gameData.rooms;
        if (rooms == null)
        {
            PlayVideo();
        }
        else
        {
            Debug.Log(roomName);
            rooms.TryGetValue(roomName, out int value);
            if (value == 0)
                PlayVideo();
        }
        this.enabled = false;
    }

    private void PlayVideo()
    {
        VideoManager videoManager = FindObjectOfType<VideoManager>();
        videoManager.PlayVideo(videoName);
    }
}
