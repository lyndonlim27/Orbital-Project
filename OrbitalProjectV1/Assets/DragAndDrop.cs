using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    CanvasGroup canvasGroup;
    public RectTransform rectTransform;
    public LetterSlotUI currSlot;
    private Vector3 originalposition;
    private Image icon;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        originalposition = rectTransform.anchoredPosition;
        icon = GetComponent<Image>();
        icon.sprite = null;
    }

    private void Start()
    {
        currSlot = GetComponentInParent<LetterSlotUI>();
    }

    public void SetImage(Sprite sprite)
    {
        icon.sprite = sprite;
        Color c = icon.color;
        c.a = 1f;
        icon.color = c;
    }

    public void ClearImage()
    {
        icon.sprite = null;
        Color c = icon.color;
        c.a = 0f;
        icon.color = c;
    }

    public void OnPointerDown(PointerEventData pointerData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.5f;
        canvasGroup.blocksRaycasts = false;

    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position;
    }

    public void ResetPosition()
    {
        rectTransform.anchoredPosition = originalposition;
    }
}
