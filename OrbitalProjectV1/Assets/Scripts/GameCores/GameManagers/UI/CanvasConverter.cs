using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


/**
 * TextConverter of General Entities.
 */
namespace GameManagement.UIComps
{
    public class CanvasConverter : MonoBehaviour
    {
        private TextMeshProUGUI tmp;
        private TypingTestTL textlogic;
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

            dict.Add(' ', " ");
            textlogic = GetComponent<TypingTestTL>();
            tmp = GetComponent<TextMeshProUGUI>();

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

                result += dict[c];
            }

            return result;
        }
    }
}
