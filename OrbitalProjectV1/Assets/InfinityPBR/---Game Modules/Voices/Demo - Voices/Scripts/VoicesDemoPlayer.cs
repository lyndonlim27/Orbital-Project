using InfinityPBR;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class VoicesDemoPlayer
{
    [Header("Plumbing")] 
    public Text nameText;
    [HideInInspector] public Voices voices;
    [HideInInspector] public int voiceIndex;
    [HideInInspector] public AudioSource audioSource;
    public GameObject linePanel;

    [Header("Voice")]
    public Voice voice;

    public void Setup()
    {
        nameText.text = voice.voice;
    }

    public void NextVoice()
    {
        int nextIndex = voiceIndex + 1;
        if (nextIndex >= voices.voices.Count)
            nextIndex = 0;

        voiceIndex = nextIndex;
        voice = voices.GetVoice(voiceIndex);

        Setup();
    }

    public void Speak(string line)
    {
        voice.PlayClip(audioSource, line);
        

        return;
        // Alt version
        audioSource.PlayOneShot(voice.GetAudioClip(line));
    }
}