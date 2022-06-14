using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SkillPurchaseButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    protected Shop shop;
    protected Player player;
    protected SpriteRenderer spriteRenderer;
    private ToggleOnOff _text;
    protected Image image;
    protected TextMeshProUGUI textDisplay;

    protected virtual void Start()
    {
        player = FindObjectOfType<Player>();
        shop = FindObjectOfType<Shop>(true);
        spriteRenderer = GetComponent<SpriteRenderer>();
        _text = GetComponentInChildren<ToggleOnOff>(true);
        image = GetComponent<Image>();
        textDisplay = GetComponentInChildren<TextMeshProUGUI>(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _text.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _text.gameObject.SetActive(false);
    }

}
