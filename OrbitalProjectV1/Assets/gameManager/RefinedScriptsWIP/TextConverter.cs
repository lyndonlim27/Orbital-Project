using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextConverter : MonoBehaviour
{
    // mainly just to convert the texts to sprites //
    private Dictionary<char, string> dict = new Dictionary<char, string>();

    private TextMeshPro wordDisplayer;
    private WordTagger wordTag;
    // Start is called before the first frame update

    void Awake()
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

    // Update is called once per frame
    void Update()
    {
        
        this.wordDisplayer.text = ConvertToCustomSprites(wordTag.remainingword);
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
