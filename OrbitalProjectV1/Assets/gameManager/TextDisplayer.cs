using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TextDisplayer : MonoBehaviour
{
    public Player player;
    public TextMeshPro wordtoDisplay = null;
    public string remainingword = "";
    public string currentword = "";
    public WordBank wordBank;
    public Entity entity;
    int currentcounter = 0;
    private Dictionary<char, string> dict = new Dictionary<char, string>();

    private void Awake()
    {
        
        for (char c = 'a'; c <= 'z'; c++)
        {
            int val = c - 'a' + 16;
            string temp = string.Format("<sprite={0}>", val);
            dict.Add(c, temp);
        }

        for (char c = 'A'; c <= 'Z'; c++)
        {
            int val = c - 'A' + 70;
            string temp = string.Format("<sprite={0}>", val);
            dict.Add(c, temp);
        }



    }
    // Start is called before the first frame update
    void Start()
    {
        entity = GetComponentInParent<Entity>();
        GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
        if (gameObject != null)
        {
            this.player = gameObject.GetComponent<Player>();
        }
        GenerateNewWord();
    }

    
    private void GenerateNewWord()
    {

        // generate a new word from wordbank;
        currentword = wordBank.GetWord();
        SetRemainingWord(currentword);
    }

    private void SetRemainingWord(string currentword)
    {
        remainingword = currentword;
        wordtoDisplay.text = ConvertToCustomSprites(remainingword);
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }

    

    private void CheckInput()
    {
        if (outOfRange() || entity.stateMachine.currState == StateMachine.STATE.STOP)
        {
            //if out of range, we dont register inputs at all
            return;
        }
        // only accepting single keypress, multikeypress too troublesome to handle;

        for (int i = (int)KeyCode.A; i < (int)KeyCode.Z; i++)
        {
            if (Input.GetKeyDown((KeyCode)i)) {
                EnterLetter((char)i);
            }

        }

    }

    private void EnterLetter(char inputchar)
    {
        if (LetterCorrect(inputchar))
        { 
            ReplaceLetter();
            currentcounter++;
            if (isWordComplete())
            {
                player.Shoot(entity);
                ResetCounter();
                GenerateNewWord();
                return;
                // generate new word and not inst animation for multi-length monsters
                //GenerateNewWord();
            }

        } else 
        {
            if (isWordPartialComplete())
            {
                ResetCounter();
                GenerateNewWord();
            }
                
            // dont do anything if its the starting character;
        }

    }

    private bool outOfRange()
    {
        if (player != null)
        {
            return Vector2.Distance(player.transform.position, entity.transform.position) > 7f;

        } else
        {
            return true;
        }
        
    }

    private bool LetterCorrect(char let)
    {
        return char.IsLower(let) && remainingword.IndexOf(let) == currentcounter;

    }

    private string ReplaceFirstOccurrence(string Source, char Find, char Replace)
    {
        int Place = Source.IndexOf(Find);
        string result = Source.Remove(Place, 1).Insert(Place, Replace.ToString());
        return result;
    }

    private void ResetCounter()
    {
        currentcounter = 0;
    }

    private void ReplaceLetter()
    {
        SetRemainingWord(ReplaceFirstOccurrence(remainingword,remainingword[currentcounter],Char.ToUpper(remainingword[currentcounter])));
    }

    public bool isWordComplete()
    {

        return !remainingword.Any(c => char.IsLower(c));

    }

    private bool isWordPartialComplete()
    {

        return remainingword.Any(c => char.IsUpper(c));

    }

    private string ConvertToCustomSprites(string word)
    {
        string result = "";
        foreach (char c in word)
        {
            result += dict[c];
        }

        return result;
    }


}
