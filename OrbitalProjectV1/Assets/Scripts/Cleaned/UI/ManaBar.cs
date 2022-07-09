using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ManaBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;
    [SerializeField] private TextMeshProUGUI percentage;

    /**
     * Setting Max Health in the UI.
     */
    public void SetMaxMana(int mana)
    {
        slider.maxValue = mana;
       // slider.value = mana;
        fill.color = gradient.Evaluate(1f);
        percentage.text = slider.value.ToString() + " MP";
    }

    /**
     * For changing the health UI upon damaged.
     */
    public void SetMana(int mana)
    {
        slider.value = mana;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        percentage.text = mana.ToString() + " MP";
    }
}
