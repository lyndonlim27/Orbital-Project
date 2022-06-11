using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


/**
 * TextLogic of General Entities.
 */
public abstract class TextLogic : MonoBehaviour
{
    
    public TextMeshPro Textdisplayer { get; protected set; }
    protected Player player;
    public string remainingword { get; protected set; } 
    public string currentword { get; protected set; }
    private GameObject AudioSystem;

    protected float minDist;
    protected int currentcounter = 0;
    protected EntityBehaviour parent;
    protected Dictionary<KeyCode, AudioSource> audioSources;
    /**
     * Retrieving data.
     */
    protected virtual void Awake()
    {
        Textdisplayer = GetComponent<TextMeshPro>();
        player = GameObject.FindObjectOfType<Player>(true);
        parent = this.gameObject.transform.parent.GetComponent<EntityBehaviour>();
        
        
    }
    /**
     * Initialize gameObject.
     */
    protected virtual void Start()
    {
        

    }

    protected virtual void OnEnable()
    {
        Textdisplayer.enabled = !outOfRange() && !parent.isDead;
        remainingword = "";
        GenerateNewWord();
        InstantiateAudio();
        minDist = parent.GetData().minDist;
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

    /**
     * Generate a new word.
     */
    protected abstract void GenerateNewWord();

    /**
     * Updating remaining word.
     * @param currentword
     * Remaining word.
     */
    protected void UpdateRemainingWord(string word)
    {
        remainingword = word;
    }

    /**
     * Update.
     */
    protected virtual void Update()
    {
        Textdisplayer.enabled = (!outOfRange()) && !parent.isDead;
        CheckInput();
    }


    /**
     * Check Internal Input.
     */
    protected abstract bool CheckInternalInput();


    /**
     * Check Overall Input.
     */
    protected virtual void CheckInput()
    {
        if (Textdisplayer.enabled)
        {
            if (!CheckInternalInput())
            {
                return;
            }



            for (int i = (int)KeyCode.A; i <= (int)KeyCode.Z; i++)
            {
                if (Input.GetKeyDown((KeyCode)i))
                {
                    audioSources[(KeyCode)i].Play();
                    Validator((char)i);
                }

            }

        }

        

    }


    /**
     * Validator.
     * @param inputchar
     * Taking player input and checking if it is valid.
     */
    protected virtual void Validator(char inputchar)
    {

        if (LetterCorrect(inputchar))
        {
            
            ReplaceLetter();
            currentcounter++;
            if (isWordComplete())
            {
                PerformAction();
                GenerateNewWord();
                ResetCounter();
                
                
            }

        } else 
        {
            if (isWordPartialComplete())
            {
                ResetCounter();
                GenerateNewWord();
            }
               
        }

        

    }

    /**
     * Check if player is in range.
     * @return true if player is in range.
     */
    protected virtual bool outOfRange()
    {
        if (player != null)
        {

            return Vector2.Distance(player.transform.position, this.transform.position) > minDist;

        } else
        {
            return true;
        }
        
    }


    /**
     * Check if letter is correct.
     * @return true if letter is correct.
     */
    protected bool LetterCorrect(char let)
    {
        return remainingword[currentcounter] == char.ToLower(let);
        //return remainingword.IndexOf(char.ToLower(let)) == currentcounter;

    }


    /**
     * Replace the first occurence of the string.
     * @return the new string with the first char replaced.
     */
    private string ReplaceFirstOccurrence(string Source, char Find, char Replace)
    {
        int Place = Source.IndexOf(Find);
        
        string result = Source.Remove(Place, 1).Insert(Place, Replace.ToString());
        return result;
    }

    /**
     * Resetting counter of the word.
     */
    protected void ResetCounter()
    {
        currentcounter = 0;
    }

    /**
     * Replacing letter of the
     */
    protected void ReplaceLetter()
    {
        string replacedword = ReplaceFirstOccurrence(remainingword, remainingword[currentcounter], Char.ToUpper(remainingword[currentcounter]));
        UpdateRemainingWord(replacedword);
    }

    /**
     * Check if word is complete.
     * @return true if word is completed.
     */
    public bool isWordComplete()
    {

        return !remainingword.Any(c => char.IsLower(c));

    }

    /**
    * Check if word is partially completed.
    * @return true if word is partially completed.
    */
    protected bool isWordPartialComplete()
    {

        return remainingword.Any(c => char.IsUpper(c));

    }

    /**
     * Perform an action on completion of word.
     */
    protected abstract void PerformAction();
}
