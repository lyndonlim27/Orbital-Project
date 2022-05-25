using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/**
 * TextConverter of General Entities.
 */
public class TextConverter : MonoBehaviour
{
    private TextMeshPro tmp;
    private Dictionary<char, string> dict = new Dictionary<char, string>();

    /**
     * Retrieving data.
     */
    protected virtual void Awake()
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

    private void Update()
    {
        tmp.text = ConvertToCustomSprites(tmp.text);

    }


    /**
     * Convert to Custom Sprites.
     * @param word
     * Convert the word into our custom sprites.
     * @return converted word.
     */
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
