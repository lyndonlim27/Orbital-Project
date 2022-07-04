using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    private Canvas canvas;
    private LetterSlotUI letterSlotUI;
    // Start is called before the first frame update

    public void Awake()
    {
        canvas = transform.root.GetComponent<Canvas>();
        letterSlotUI = GetComponentInChildren<LetterSlotUI>();
    }


    // Update is called once per frame
    void Update()
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            Input.mousePosition,
            canvas.worldCamera,
            out position
                );
        transform.position = canvas.transform.TransformPoint(position);
    }
}
