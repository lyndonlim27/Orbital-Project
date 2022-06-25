using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;

public class  TypingTestTL : TextLogic
{
    [SerializeField] private TextAsset text;

    private float seconds;
    private int wordCount;
    private CanvasConverter textConverter;
    private TextMeshProUGUI CanvasDisplayer;
    private RoomManager roomManager;
    private List<string> storybank = new List<string>();
    private DialogueDetection dialogueDetection;


    protected override void Awake()
    {
        string story = text.text;
        string word = "";
        foreach (char c in story)
        {
            if (char.IsLetter(c))
            {
                word += c;
            }
            else
            {
                storybank.Add(word);
                word = "";
            }

        }
        
        CanvasDisplayer = GetComponent<TextMeshProUGUI>();
        textConverter = GetComponent<CanvasConverter>();
        dialogueDetection = FindObjectOfType<DialogueDetection>(true);
        
    }

    protected override void Start()
    {
        transform.root.gameObject.SetActive(false);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        InstantiateAudio();
        currentword = "";
        remainingword = "";
        seconds = 100;
        CanvasDisplayer.enabled = true;
        StartCoroutine(CountDown());
    }

    protected override void Update()
    {
        seconds -= Time.deltaTime;
        CheckInput();
        
        if (seconds <= 0)
        {
            textConverter.enabled = false;
            CanvasDisplayer.text = string.Format("Words Per Minute = {0}",  (wordCount * 2));
            StartCoroutine(WaitForStats());
        }
        
    }

    protected override void CheckInput()
    {
        if (!CheckInternalInput())
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (LetterCorrect(' '))
            {
                currentcounter++;
                GenerateNewWord();
            }

            
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


    protected override void Validator(char inputchar)
    {
        if (LetterCorrect(inputchar))
        {
            ReplaceLetter();
            currentcounter++;
        }
    }

    protected override bool CheckInternalInput()
    {
        return true;
    }

    protected override void GenerateNewWord()
    {
        wordCount++;
        string newword;
        while(storybank[0] == "")
        {
            storybank.Remove(storybank[0]);
        }
        newword = storybank[0];
        remainingword = remainingword.Substring(currentcounter) + " " + newword.ToLower();
        storybank.Remove(newword);
        ResetCounter();
        
    }

    private IEnumerator CountDown()
    {
        
        List<string> counter = new List<string>() { "THREE", "TWO", "ONE" };
        for (int i = 0; i < counter.Count; i++)
        {
            audioSources[KeyCode.A].Play();
            remainingword = counter[i];
            yield return new WaitForSeconds(1f);
        }

        wordCount = 0;
        seconds = 30;
        remainingword = storybank[0];
        storybank.Remove(remainingword);
        int j = 4;
        while (j-- > 0)
        {
            string cword = storybank[0];
            storybank.Remove(cword);
            remainingword += " " + cword;
        }
        remainingword = remainingword.ToLower();


    }

    protected override void PerformAction()
    {
        throw new System.NotImplementedException();
    }


    private IEnumerator WaitForStats()
    {
        yield return new WaitForSeconds(2f);
        
        this.transform.parent.parent.gameObject.SetActive(false);
        this.enabled = false;
        //go.SetActive(false);

    }
}
