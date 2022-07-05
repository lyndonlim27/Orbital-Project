using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

/**
 * TextConverter of General Entities.
 */
public class TextConverter : MonoBehaviour
{
    private TextMeshProUGUI tmp;
    private TextLogic textlogic;
    [SerializeField] private TMP_SpriteAsset unhighlighted;
    [SerializeField] private TMP_SpriteAsset highlighted;

    private Dictionary<char, string> dict = new Dictionary<char, string>();

    /**
     * Retrieving data.
     */
    protected virtual void Awake()
    {

        textlogic = GetComponent<TextLogic>();
        tmp = GetComponent<TextMeshProUGUI>();
        tmp.enableKerning = false;
        SortSpriteAsset();



    }


    private void SortSpriteAsset()
    {

        List<TMP_SpriteCharacter> unhighlightedTable = unhighlighted.spriteCharacterTable;
        List<TMP_SpriteCharacter> highlightedTable = highlighted.spriteCharacterTable;



        unhighlightedTable.Sort((a, b) => int.Parse(a.name.Substring(a.name.LastIndexOf('_') + 1)).CompareTo(int.Parse(b.name.Substring(b.name.LastIndexOf('_') + 1))));
        highlightedTable.Sort((a, b) => int.Parse(a.name.Substring(a.name.LastIndexOf('_') + 1)).CompareTo(int.Parse(b.name.Substring(b.name.LastIndexOf('_') + 1))));

        for (char c = 'a'; c <= 'z'; c++)
        {
            int val = c - 87;
            string temp = string.Format("<sprite=\"unhighlighted\" index={0}>", val);
            dict.Add(c, temp);
        }

        for (char c = 'A'; c <= 'Z'; c++)
        {
            int val = c - 55;
            string temp = string.Format("<sprite=\"highlighted\" index={0}>", val);
            dict.Add(c, temp);
        }
        dict.Add(' ', " ");
    }


    private void Start()
    {

        if (tmp != null)
        {
            tmp.text = ConvertToCustomSprites(textlogic.remainingword);
        }
    }

    private void Update()
    {
        if (tmp != null)
        {
            tmp.text = ConvertToCustomSprites(textlogic.remainingword);
        }

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
            //CheckCharacter(c);
            result += dict[c];
        }

        return result;
    }

    //private void CheckCharacter(char c)
    //{
    //    if (char.IsUpper(c))
    //    {
    //        tmp.spriteAsset = highlighted;
    //    }
    //    else
    //    {
    //        tmp.spriteAsset = unhighlighted;
    //    }
    //}


}
