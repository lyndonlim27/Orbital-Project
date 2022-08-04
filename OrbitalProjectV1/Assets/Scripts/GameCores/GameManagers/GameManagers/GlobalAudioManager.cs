using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalAudioManager : MonoBehaviour
{
    [SerializeField] List<AudioClip> audioClips;
    public int totalRooms;
    AudioSource audioSource;
    public static GlobalAudioManager instance { get; private set; }
    int breaks;
    int currindex;
    

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        breaks = (int) totalRooms / audioClips.Count; //6
        if (instance == null)
        {
            instance = this;
        } else
        {
            Debug.Log("more than one global audio manager");
        }
        currindex = -1;
    }

    public void PlayTrack(int roomid)
    {
        //change audio based on progression
        
        //if (currindex == -1)
        //{
        //    currindex = (int) roomid / breaks;
        //    LoadAudio(audioClips[currindex]);
        //} else
        
        int roomindex = Mathf.Min(roomid / breaks, audioClips.Count - 1);
        LoadAudio(audioClips[roomindex]);
        //if (roomindex == currindex)
        //{
        //    return;
        //}
        //else
        //{
        //    currindex = roomindex;
        //    //if (currindex == 3)
        //    //{
        //    //    audioSource.Stop();
        //    //    audioSource.volume = 0.8f;
        //    //}

            //LoadAudio(audioClips[currindex]);

        
        //}
       
        
    }

    public void PlaySpecificTrack(AudioClip audioClip)
    {
        LoadAudio(audioClip);
    }

    public void PlaySpecificTrack(AudioClip audioClip, float volume)
    {
        audioSource.volume = volume;
        LoadAudio(audioClip);
    }

    private void LoadAudio(AudioClip audioClip)
    {
        //audioSource.Stop();
        audioSource.clip = audioClip;
        audioSource.Play();
    }

}
