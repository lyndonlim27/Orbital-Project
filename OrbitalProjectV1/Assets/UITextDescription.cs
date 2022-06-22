using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITextDescription : MonoBehaviour
{
    TextMeshProUGUI textDescription;

    private void Awake()
    {

        textDescription = GetComponentInChildren<TextMeshProUGUI>();
        textDescription.enabled = false;
    }

    public void StartDescription(string text)
    {
        StartCoroutine(ShowDescription(text));
    }

    private IEnumerator ShowDescription(string description)
    {
        
        {
            textDescription.text = description;
            textDescription.enabled = true;
            StartCoroutine(FadeInText());
            yield return new WaitForSeconds(2f);
            StartCoroutine(FadeOutText());
            yield return new WaitForSeconds(1f);
            
            textDescription.enabled = false;

        }

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
        Debug.Log("Did we enter fade out?");
        for (float f = 1f; f > 0.1f; f -= 0.1f)
        {
            Color c = textDescription.color;
            c.a = f;
            textDescription.color = c;
            yield return new WaitForSeconds(0.1f);
        }

    }
}
