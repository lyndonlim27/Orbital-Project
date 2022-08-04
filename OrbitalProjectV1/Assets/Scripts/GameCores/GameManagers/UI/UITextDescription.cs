using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITextDescription : MonoBehaviour
{
    TextMeshProUGUI textDescription;
    public static UITextDescription instance { get; private set; }

    private void Awake()
    {

        textDescription = GetComponentInChildren<TextMeshProUGUI>();
        textDescription.enabled = false;
        if (instance == null)
        {
            instance = this;
        }
    }
    //private void OnEnable()
    //{
    //    StopAllCoroutines();
    //}

    //private void OnDisable()
    //{
    //    StopAllCoroutines();
    //}

    /// <summary>
    /// Show Text Indefinitely.
    /// </summary>
    /// <param name="text"></param>
    public void ShowTextIndefinite(string text)
    {
        textDescription.text = text;
    }

    /// <summary>
    /// Show text for a small amt of time.
    /// </summary>
    /// <param name="text"></param>
    public void StartDescription(string text)
    {
        
        StartCoroutine(ShowDescription(text));
        
            
    }

    public IEnumerator ShowDescription(string description)
    {
           
            textDescription.text = description;
            textDescription.enabled = true;
            StartCoroutine(FadeInText());
            yield return new WaitForSeconds(3f);
            StartCoroutine(FadeOutText());
            yield return new WaitForSeconds(1f);
            textDescription.enabled = false;
            

        

    }

    private IEnumerator FadeInText()
    {
        for (float f = 0.1f; f < 1f; f += 0.1f)
        {
            Color c = textDescription.color;
            c.a = f;
            textDescription.color = c;
            yield return new WaitForSeconds(0.1f);
        }

    }

    private IEnumerator FadeOutText()
    {
        for (float f = 1f; f > 0.1f; f -= 0.1f)
        {
            Color c = textDescription.color;
            c.a = f;
            textDescription.color = c;
            yield return new WaitForSeconds(0.1f);
        }

    }
}
