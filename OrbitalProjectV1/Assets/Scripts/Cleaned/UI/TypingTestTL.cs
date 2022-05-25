using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;

public class TypingTestTL : TextLogic
{
    [SerializeField] private TextAsset text;

    private float seconds;
    private int wordCount;
    private CanvasConverter textConverter;
    private TextMeshProUGUI CanvasDisplayer;

    protected override void Awake()
    {
        currentword = "";
        seconds = 10;
        wordCount = 0;
        StringBuilder sb = new StringBuilder();
        string story = text.text;
        foreach (char c in story)
        {
            if (c == ' ' || c == '\n')
            {
                currentword += " ";
                
            }

            else if (char.IsLetter(c))
            {
                currentword += c;
            }

        }

        remainingword = currentword.ToLower();
        CanvasDisplayer = GetComponent<TextMeshProUGUI>();
        textConverter = GetComponent<CanvasConverter>();
        CanvasDisplayer.enabled = true;
    }

    protected override void Update()
    {
        seconds -= Time.deltaTime;
        CheckInput();
        
        if (seconds <= 0)
        {
            textConverter.enabled = false;
            Debug.Log(wordCount);
            CanvasDisplayer.text = string.Format("Words Per Minute = {0}", (int) (wordCount / (1f/6)));

            Debug.Log("done");
        }
        
    }


    protected override void CheckInput()
    {
        if (!CheckInternalInput())
        {
            return;
        }



        for (int i = (int)KeyCode.A; i <= (int)KeyCode.Z; i++)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (LetterCorrect(' '))
                {
                    ReplaceLetter();
                    currentcounter++;
                    PerformAction();
                    break;
                }
                
            }

            if (Input.GetKeyDown((KeyCode)i))
            {
                Validator((char)i);
            }

        }

    }


    protected override void Validator(char inputchar)
    {
        if (LetterCorrect(inputchar))
        {
            ReplaceLetter();
            currentcounter++;
            if (isWordComplete())
            {

                GenerateNewWord();
                ResetCounter();
            }

        }
    }

    protected override bool CheckInternalInput()
    {
        return true;
    }

    protected override void GenerateNewWord()
    {
        return;
    }

    protected override void PerformAction()
    {
        wordCount++;
        remainingword = remainingword.Substring(currentcounter);
        ResetCounter();
    }
}
