using TMPro;
using UnityEngine;
using System.Collections;
using Ink.Runtime;
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.UI;
using EntityDataMgt;
using EntityCores;

/** 
 * DialogueManager.
 * Manages all dialogues in the room.
 */
namespace GameManagement
{
    public class DialogueManager : MonoBehaviour
    {
        //null in the beginning

        public static DialogueManager instance { get; private set; }

        private Story currentstory;

        private TextMeshProUGUI[] choicesText;

        private RoomManager roomManager;

        private NPCBehaviour currentNPC;

        private NPCData _npcData;

        private EnemyData _enemyData;

        private Coroutine coroutine;

        private string nextline;

        //private int currindex;

        //private Player player;

        public bool playing { get; private set; }

        private bool inExit;


        [Header("DialogueUI")]
        [SerializeField] private GameObject dialoguePanel;
        [SerializeField] private TextMeshProUGUI dialoguetext;
        [SerializeField] private Image dialogueImage;
        [SerializeField] private GameObject[] choices;

        /** 
         * Retrieving of Data.
         */
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            dialoguePanel = GameObject.Find("DialoguePanel");
            dialoguetext = dialoguePanel.GetComponentInChildren<TextMeshProUGUI>();
            dialogueImage = dialoguePanel.transform.Find("DialogueImage").GetComponent<Image>();
            for (int i = 0; i < choices.Length; i++)
            {
                choices[i] = dialoguePanel.transform.Find("Choices").Find($"Choice{i + 1}").gameObject;
            }
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
            //StopCoroutine(coroutine);
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                StopCoroutine(SelectFirstChoice());
                coroutine = null;
                dialoguetext.text = nextline;
            }
            ContinueStory();

        }


        /**
         * Update is called once per frame.
         */
        void Update()
        {
            if (!playing || inExit)
            {
                return;
            }
            else
            {
                ////if (coroutine == null && Input.GetKeyDown(KeyCode.Space))
                ////{
                ////    ContinueStory();
                ////} else if (coroutine != null && Input.GetKeyDown(KeyCode.Space))
                ////{
                ////    StopCoroutine(coroutine);
                ////    dialoguetext.text = nextline;
                //if (Input.GetKeyDown(KeyCode.Space)) {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (coroutine != null)
                    {
                        StopCoroutine(coroutine);
                        coroutine = null;
                        dialoguetext.text = nextline;
                    }
                    else
                    {
                        ContinueStory();
                    }

                }

                else if (Input.GetKeyDown(KeyCode.Escape))
                {
                    StartCoroutine(ExitDialogue());
                }
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
            _npcData = (NPCData)npc.GetData();
            currentstory = new Story(_npcData.story.text);
            playing = true;
            dialogueImage.sprite = _npcData.dialogueFace;
            dialoguePanel.SetActive(true);
            ContinueStory();

        }

        public void EnterDialogue(EnemyBehaviour enemy)
        {
            currentNPC = null;
            _enemyData = (EnemyData)enemy.GetData();
            currentstory = new Story(_enemyData.story.text);
            playing = true;
            dialogueImage.sprite = _enemyData.sprite;
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
                nextline = currentstory.Continue();
                coroutine = StartCoroutine(TypeSentence(nextline));

                if (currentNPC != null && currentstory.currentTags.Count > 0 &&
                    currentstory.currentTags[currentstory.currentTags.Count - 1] == "NPC")
                {
                    if (!currentNPC.fulfilled)
                    {
                        currentNPC.Fulfill();
                    }

                    //THE CHANGE HERE.
                    //dialoguePanel.SetActive(false);
                    //THE CHANGE HERE.
                }

                DisplayChoices();

            }
            else
            {
                if (choicesText.ToList().TrueForAll(choice => !choice.gameObject.activeInHierarchy))
                {
                    StartCoroutine(ExitDialogue());
                }

            }
        }

        /**
         * Exit Dialogue.
         */
        public IEnumerator ExitDialogue()
        {
            inExit = true;
            yield return new WaitForSeconds(0.5f);
            if (currentNPC != null)
            {
                currentNPC.NPCAfterAction();
            }

            playing = false;
            dialoguePanel.SetActive(false);
            inExit = false;
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

        public void SetCurrentRoom(RoomManager roomManager)
        {
            this.roomManager = roomManager;
        }


    }
}
