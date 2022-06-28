using System.Collections;
using System.Collections.Generic;
using InfinityPBR;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class VoicesDemoControl : MonoBehaviour
{
    public Transform buttonBucket;
    public GameObject buttonPrefab;
    public Voices voices;
    public AudioSource audioSource;
    
    // Demo for voices
    public List<VoicesDemoPlayer> players = new List<VoicesDemoPlayer>();
    
    // Start is called before the first frame update
    void Start()
    {
        PopulateButtons();
        RandomizeVoices();
        foreach (VoicesDemoPlayer player in players)
        {
            player.voices = voices;
            player.audioSource = audioSource;
            player.Setup();
            SetupPlayerButtons(player);
        }
    }

    private void SetupPlayerButtons(VoicesDemoPlayer player)
    {
        foreach (string line in voices.lineNames)
        {
            GameObject newButton = Instantiate(buttonPrefab, player.linePanel.transform);
            newButton.GetComponentInChildren<Text>().text = line;
            PlayButtonDemo playButtonDemo = newButton.GetComponent<PlayButtonDemo>();
            playButtonDemo.player = player;
            playButtonDemo.line = line;
        }
    }
    
    
    // You can also assign a specific voice:
    private void AssignVoice(VoicesDemoPlayer player, string voiceName)
    {
        player.voice = voices.GetVoice(voiceName);
    }

    private void RandomizeVoices()
    {
        foreach (VoicesDemoPlayer player in players)
        {
            player.voice = voices.GetRandomVoice();
            player.voiceIndex = voices.GetVoiceIndex(player.voice.voice);
        }
    }

    private void PopulateButtons()
    {
        foreach (Voice voice in voices.voices)
        {
            foreach (Line line in voice.lines)
            {
                GameObject newButton;
                PlayButtonDemo buttonDemo;
                // Default
                newButton = Instantiate(buttonPrefab, buttonBucket);
                newButton.GetComponentInChildren<Text>().text = voice.voice + " = " + line.line + " - Default";
                buttonDemo = newButton.GetComponent<PlayButtonDemo>();
                buttonDemo.audioSource = audioSource;
                buttonDemo.voices = voices;
                buttonDemo.voice = voice.voice;
                buttonDemo.line = line.line;

                foreach (Emotion emotion in line.emotions)
                {
                    if (emotion.audioClip.Count > 0)
                    {
                        newButton = Instantiate(buttonPrefab, buttonBucket);
                        newButton.GetComponentInChildren<Text>().text = voice.voice + " = " + line.line + " - " + emotion.emotion;
                        buttonDemo = newButton.GetComponent<PlayButtonDemo>();
                        buttonDemo.audioSource = audioSource;
                        buttonDemo.voices = voices;
                        buttonDemo.voice = voice.voice;
                        buttonDemo.line = line.line;
                        buttonDemo.emotion = emotion.emotion;
                    }
                }
            }
        }
    }

    public void NextVoice(int playerIndex)
    {
        players[playerIndex].NextVoice();
    }
}