using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//To take in input
public class PuzzleInputManager : MonoBehaviour
{

    [SerializeField] private Sprite[] letters;
    private GameObject AudioSystem;
    private Dictionary<KeyCode, AudioSource> audioSources;
    private TextMeshProUGUI textInput;
    [SerializeField] private LetterSlotNoDnD[] letterSlots;
    private int currindex;
    private RoomManager anyRoom;
    public bool guessed { get; private set; }
    //private LightSwitchSystem lightSwitchSystem;

    private void Awake()
    {
        InstantiateAudio();
        anyRoom = FindObjectOfType<RoomManager>();
        textInput = transform.Find("Text").Find("InputText").GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        
    }

    private void Start()
    {
        letterSlots = GetComponentsInChildren<LetterSlotNoDnD>(true);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        ReceiveInput();

    }

    protected void InstantiateAudio()
    {
        AudioSystem = GameObject.FindGameObjectWithTag("Audio");
        audioSources = new Dictionary<KeyCode, AudioSource>();
        AudioSource[] temp = AudioSystem.GetComponentsInChildren<AudioSource>();
        for (int i = (int)KeyCode.A; i <= (int)KeyCode.Z; i++)
        {
            audioSources[(KeyCode)i] = temp[i - 97];
        }
    }

    public void SetUp(int numofletters)
    {
        guessed = false;
        int i = 0;
        for (; i < numofletters; i++)
        {
            letterSlots[i].ClearData();
            letterSlots[i].gameObject.SetActive(true);
        }

        while (i < letterSlots.Length)
        {
            letterSlots[i].gameObject.SetActive(false);
            i++;
        }

        
    }

    public void SetUpText(string text)
    {
        textInput.text = text;
    }

    private void ReceiveInput()
    {
        LetterSlotNoDnD[] enabledslots = GetComponentsInChildren<LetterSlotNoDnD>(false);

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            audioSources[KeyCode.A].Play();
            currindex = Mathf.Max(0, currindex - 1);
            enabledslots[currindex].ClearData();
            

        }
        for (int i = (int)KeyCode.Alpha0; i <= (int)KeyCode.Alpha9; i++)
        {
            if (Input.GetKeyDown((KeyCode)i))
            {
                if (currindex >= enabledslots.Length)
                {
                    return;
                }
                else
                {
                    audioSources[KeyCode.A].Play();
                    int currnum = i - 48;
                    enabledslots[currindex].SetData(letters[currnum], currnum);
                    currindex++;

                }

            } 

        }
    }

    //close this gameobject.
    public void OnButtonClicked()
    {
        guessed = true;
        
        
        FindObjectOfType<SkillManager>().enabled = true;
        this.gameObject.SetActive(false);
        
    }

    public void ResetGuess()
    {
        guessed = false;
        for (int i = 0; i < letterSlots.Length; i++)
        {
            letterSlots[i].ClearData();
        }
        currindex = 0;

    }

    public LetterSlotNoDnD[] GetCurrentGuess()
    {
        return letterSlots;
    }


    
}
