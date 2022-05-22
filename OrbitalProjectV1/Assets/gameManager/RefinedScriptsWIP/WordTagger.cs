using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using System;

//generic 
public class WordTagger : MonoBehaviour
{
    //handling logic of words;
    private Player player;
    public string currentword { get; private set; }
    public string remainingword { get; private set; }
    [SerializeField] private WordBank wordBank;
    private TextMeshPro wordDisplayer;
    private int currentcounter = 0;
    private GameObject _GameObject;
    private float minDist;

    private void Awake()
    {
        this.player = GameObject.FindObjectOfType<Player>();
        this._GameObject = this.transform.parent.gameObject;
        this.wordDisplayer = GetComponent<TextMeshPro>();
        
    }

    private void Start()
    {
        //this.wordDisplayer.enabled = outOfRange();
        //Entity entity = _GameObject.GetComponent<Entity>();
        //Item item = _GameObject.GetComponent<Item>();
        //if (entity == null)
        //{
        //    minDist = item.mindist;
        //}
        //else
        //{
        //    minDist = entity.maxDist;
        //}
    }

    private void Update()
    {
        wordDisplayer.enabled = outOfRange();
    }


    protected virtual void CheckInput()
    {
        // only accepting single keypress, multikeypress too troublesome to handle;

        for (int i = (int)KeyCode.A; i <= (int)KeyCode.Z; i++)
        {
            if (Input.GetKeyDown((KeyCode)i))
            {
                EnterLetter((char)i);
            }

        }

    }

    protected virtual void EnterLetter(char inputchar)
    {
        if (LetterCorrect(inputchar))
        {
            ReplaceLetter();
            currentcounter++;
            if (isWordComplete())
            {
                IntiateCompleteAction(_GameObject.tag);
            }

        }
        else
        {
            if (isWordPartialComplete())
            {
                InitiatePartialAction(_GameObject.tag);
            }

            // dont do anything if its the starting character;
        }

    }

    protected virtual bool outOfRange()
    {
        if (player != null)
        {

            return Vector2.Distance(player.transform.position, this.transform.position) > minDist;

        }
        else
        {
            //if player not found always return true
            return true;
        }

    }

    protected bool LetterCorrect(char let)
    {
        return char.IsLower(let) && remainingword.IndexOf(let) == currentcounter;

    }

    private string ReplaceFirstOccurrence(string Source, char Find, char Replace)
    {
        int Place = Source.IndexOf(Find);
        string result = Source.Remove(Place, 1).Insert(Place, Replace.ToString());
        return result;
    }

    protected void ResetCounter()
    {
        currentcounter = 0;
    }

    protected void ReplaceLetter()
    {
        SetRemainingWord(ReplaceFirstOccurrence(remainingword, remainingword[currentcounter], Char.ToUpper(remainingword[currentcounter])));
    }

    public bool isWordComplete()
    {

        return !remainingword.Any(c => char.IsLower(c));

    }

    protected bool isWordPartialComplete()
    {

        return remainingword.Any(c => char.IsUpper(c));

    }

    private void IntiateCompleteAction(string type)
    {
        switch (type)
        {
            case "Enemy":
                //player.Shoot(gameObject);
                Entity entity = _GameObject.GetComponent<Entity>();
                if (!entity.isDead)
                {
                    currentword = wordBank.GetWord();
                }
                break;
            case "Droppable":
                ItemStats item = _GameObject.GetComponent<ItemStats>();
                RoomManager roomManager = GameObject.FindObjectOfType<RoomManager>();
                roomManager.FulfillCondition(item._name);
                this._GameObject.GetComponentInParent<SpriteRenderer>().enabled = false;
                this._GameObject.SetActive(false);
                break;
        }
        ResetCounter();

    }

    private void InitiatePartialAction(string type)
    {
        switch (type)
        {
            case "Enemy":
                //player.Shoot(gameObject);
                currentword = wordBank.GetWord();
                break;
            case "Droppable":
                this.remainingword = currentword;
                break;
        }

        ResetCounter();

    }

    protected void SetRemainingWord(string currentword)
    {
        remainingword = currentword;

    }
}
