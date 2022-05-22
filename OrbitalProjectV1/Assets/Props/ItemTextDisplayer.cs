using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTextDisplayer : TextDisplayer
{
    SpriteRenderer spriteRenderer;
    ItemStats _itemstats;
    private RoomManager roomManager;


    protected override void Start()
    {
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
        _itemstats = GetComponentInParent<Item>().itemStats;
        this.currentword = _itemstats._name;
        GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
        if (gameObject != null)
        {
            this.player = gameObject.GetComponent<Player>();
        }
        roomManager = GameObject.FindObjectOfType<RoomManager>();
        SetRemainingWord(currentword);
        minDist = _itemstats.mindist;

    }

    

    protected override void CheckInput()
    {
        if (outOfRange())
        {
            //if out of range, we dont register inputs at all
            return;
        }

        for (int i = (int)KeyCode.A; i <= (int)KeyCode.Z; i++)
        {
            if (Input.GetKeyDown((KeyCode)i))
            {
                EnterLetter((char)i);
            }
        }
    }

    protected override void EnterLetter(char inputchar)
    {
        if (LetterCorrect(inputchar))
        {
            ReplaceLetter();
            currentcounter++;
            if (isWordComplete())
            {
                
                roomManager.FulfillCondition(_itemstats._name);
                spriteRenderer.enabled = false;
                this.gameObject.SetActive(false);

            }
        }
        else
        {
            if (isWordPartialComplete())
            {
                this.remainingword = currentword;
                SetRemainingWord(currentword);
                ResetCounter();
                
            }
        }
    }

    protected override bool outOfRange()
    {
        if (player != null)
        {

            return Vector2.Distance(player.transform.position, this.transform.position) > minDist;

        }
        else
        {
            return true;
        }

    }
}
