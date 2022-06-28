using System.Collections;
using System.Collections.Generic;
using InfinityPBR;
using UnityEngine;

public class PlayButtonDemo : MonoBehaviour
{
    public AudioClip clip;
    public string voice;
    public string line;
    public string emotion = "";
    public AudioSource audioSource;
    public Voices voices;
    public VoicesDemoPlayer player;

    public void PlayClip()
    {
        player.Speak(line);
    }
}
