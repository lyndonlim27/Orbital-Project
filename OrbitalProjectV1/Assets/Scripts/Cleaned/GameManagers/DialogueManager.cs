using TMPro;
using UnityEngine;
using System.Collections;
using Ink.Runtime;
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;


/** 
 * DialogueManager.
 * Manages all dialogues in the room.
 */

public class DialogueManager : MonoBehaviour
{
    //null in the beginning
    private Story currentstory;

    private TextMeshProUGUI[] choicesText;

    private RoomManager roomManager;

    private NPCBehaviour currentNPC;

    private NPCData _npcData;

    public bool playing { get; private set; }

    [Header("DialogueUI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialoguetext;
    [SerializeField] private GameObject[] choices;

    /** 
     * Retrieving of Data.
     */
    private void Awake()
    {
        roomManager = FindObjectOfType<RoomManager>();
        //npc = roomManager.npcs;
        //_npcData = roomManager._npcData;

    }

    /**
     * Initializing DialogueManager.
     */
    void Start()
    {
        playing = false;
        dialoguePanel.SetActive(false);
        choicesText = new TextMeshProUGUI[choices.Length];
        for (int i = 0; i < choices.Length; i++)
        {
            choicesText[i] = choices[i].GetComponentInChildren<TextMeshProUGUI>();
           //choices[i].gameObject.SetActive(false);
        }


    }

    /**
     * Displaying Choices.
     */

    private void DisplayChoices()
    {
        List<Choice> currentchoices = currentstory.currentChoices;

        if (currentchoices.Count > choices.Length)
        {
            Debug.Log("More choices were given than supported. Number of choices: " + currentchoices.Count);
        }

        int index = 0;
        // enable and initialize the choices up to the amount of choices for this line of dialogue
        foreach (Choice choice in currentchoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }
        // go through the remaining choices the UI supports and make sure they're hidden
        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }
        StartCoroutine(SelectFirstChoice());
    }

    /**
     * MakeChoices.
     * @param choiceIndex 
     * Continue Story based on choice selected.
     */
    public void MakeChoice(int choiceIndex)
    {
        currentstory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }
    

    /**
     * Update is called once per frame.
     */
    void Update()
    {
        Debug.Log(FindObjectOfType<RoomManager>());
        if (!playing)
        {
            return;
        } else if (Input.GetKeyDown(KeyCode.Space))
        {
            ContinueStory();
        }

    }

    /**
     * Enter Dialogue.
     * @param npc
     * Initialize 
     */
    public void EnterDialogue(NPCBehaviour npc)
    {
        currentNPC = npc;
        _npcData = (NPCData) npc.GetData();
        currentstory = new Story(_npcData.story.text);
        playing = true;
        dialoguePanel.SetActive(true);
        ContinueStory();

    }


    /**
     * Continue Story.
     */
    private void ContinueStory()
    {   
        if (currentstory.canContinue)
        {
            string nextline = currentstory.Continue();
            StartCoroutine(TypeSentence(nextline));
            if (currentstory.currentTags.Count > 0 &&
                currentstory.currentTags[currentstory.currentTags.Count - 1] == "NPC")
            {
                currentNPC.Fulfill();

                roomManager.FulfillCondition(_npcData._name);

            }

            DisplayChoices();
           
        }
        else
        {
            StartCoroutine(ExitDialogue());
        }
    }

    /**
     * Exit Dialogue.
     */
    private IEnumerator ExitDialogue()
    {
        yield return new WaitForSeconds(0.5f);
        playing = false;
        dialoguePanel.SetActive(false);
    }

    /**
     * Typing Effects for Dialogues.
     * @param sentence
     * Add typing effects to dialogue sentences.
     */
    IEnumerator TypeSentence(string sentence)
    {
        dialoguetext.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialoguetext.text += letter;
            yield return null;
        }
      
        yield return null;
    }

    private IEnumerator SelectFirstChoice()
    {
        // Event System requires we clear it first, then wait
        // for at least one frame before we set the current selected object.
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }
    


}
