using TMPro;
using UnityEngine;
using System.Collections;
using Ink.Runtime;
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    // Start is called before the first frame update
    

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialoguetext;
    private static DialogueManager dialinst;
    public bool playing { get; private set; }
    private Story currentstory;
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    private void Awake()
    {
        if (dialinst != null)
        {
            Debug.Log("Found more than one dialogue manager");
        } else
        {
            dialinst = this;
        }
    }

    // for getting the instance of dialmanager;
    public static DialogueManager GetInstance()
    {
        return dialinst;
    }

    void Start()
    {
        playing = false;
        dialoguePanel.SetActive(false);

        choicesText = new TextMeshProUGUI[choices.Length];
        for (int i = 0; i < choices.Length; i++)
        {
            choicesText[i] = choices[i].GetComponentInChildren<TextMeshProUGUI>();
        }

        

    }

    private void displayChoices()
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

    private IEnumerator SelectFirstChoice()
    {
        // Event System requires we clear it first, then wait
        // for at least one frame before we set the current selected object.
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int choiceIndex)
    {
        currentstory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }
    

    // Update is called once per frame
    void Update()
    {
        if (!playing)
        {
            return;
        } else if (Input.GetKeyDown(KeyCode.Space))
        {
            ContinueStory();
        }

    }

    public void enterDialogue(TextAsset inkjson)
    {
        currentstory = new Story(inkjson.text);
        playing = true;
        dialoguePanel.SetActive(true);
        ContinueStory();
    }

    private void ContinueStory()
    {
        if (currentstory.canContinue)
        {
            string nextline = currentstory.Continue();
            StartCoroutine(TypeSentence(nextline));

            displayChoices();
        }
        else
        {
            StartCoroutine(ExitDialogue());
        }
    }

    private IEnumerator ExitDialogue()
    {
        yield return new WaitForSeconds(0.2f);
        playing = false;
        dialoguePanel.SetActive(false);
    }

    //for rpg effect
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


}
