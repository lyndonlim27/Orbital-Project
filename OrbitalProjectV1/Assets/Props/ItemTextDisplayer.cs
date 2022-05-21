using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTextDisplayer : TextDisplayer
{
    [SerializeField] Item item;
    protected override void Start()
    {
        this.currentword = item._name;
        GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
        if (gameObject != null)
        {
            this.player = gameObject.GetComponent<Player>();
        }
        SetRemainingWord(currentword);
        minDist = item.mindist;
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
                RoomManager roomManager = GameObject.FindObjectOfType<RoomManager>();
                roomManager.FulfillCondition(item._name);
                this.gameObject.GetComponentInParent<SpriteRenderer>().enabled = false;
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
