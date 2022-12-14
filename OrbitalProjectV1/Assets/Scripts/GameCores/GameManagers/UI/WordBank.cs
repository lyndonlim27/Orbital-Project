using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordBank : MonoBehaviour
{
    public TextAsset file;
    public string passcode;
    List<string> wordBank = new List<string>();
    //List<string> originalwords = new List<string>()
    //{
    //    "asd","abd","weq"
    //};
    // Start is called before the first frame update


    private void Awake()
    {
        var content = file.text;
        var AllWords = content.Split('\n', ' ', '.', '!', '?', ';', ':');
        //wordBank.AddRange(AllWords);
        foreach(string word in AllWords)
        {
            if (word.Length < 5)
            {
                wordBank.Add(word);
            }
        }
        //wordBank.AddRange(originalwords);
        Shuffle();
        ToLower();
        GeneratePassCode();
    }

    private void GeneratePassCode()
    {
        for (int i = 0; i < 4; i++)
        {
            passcode += GetWord().ToLower();
        }
    }

    private void Shuffle()
    {
        for (int i = 0; i < wordBank.Count; i++)
        {
            int random = Random.Range(0, wordBank.Count);
            string temp = wordBank[i];

            wordBank[i] = wordBank[random];
            wordBank[random] = temp;

        }

    }

    private void ToLower()
    {
        for (int i = 0; i < wordBank.Count; i++)
        {
            wordBank[i] = wordBank[i].ToLower();
        }

    }

    public string GetWord()
    {
        if (wordBank.Count != 0)
        {
            string word = wordBank[0];
            wordBank.Remove(wordBank[0]);
            return word.TrimEnd(); 
        } else
        {
            return "";
        }
    }

    

  
    // Update is called once per frame
    void Update()
    {
        
    }
}
