using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIObjectivePointer : MonoBehaviour
{

    private Vector2 objectivePos;
    private RectTransform pointer;
    private float offset;
    [SerializeField] private Sprite arrow;
    [SerializeField] private Image pointerImage;

    private void Awake()
    {
        pointer = GetComponent<RectTransform>();
        offset = 100f;
        pointerImage = GetComponentInChildren<Image>();
        pointerImage.sprite = arrow;
        gameObject.SetActive(false);
        
    }

    public void StartNavi(Vector2 pos)
    {
        gameObject.SetActive(true);
        objectivePos = pos;

    }

    public void StopNavi()
    {
        if (Vector2.Distance(pointer.position, objectivePos) <= 3f)
        {
            gameObject.SetActive(false);
        }
        
    }


    private void Update()
    {
        RotateTowardsTarget();
        StartArrowNavi();
        StopNavi();

    }

    private void RotateTowardsTarget()
    {
        Vector2 dir = (objectivePos - (Vector2)Camera.main.transform.position).normalized;
        pointer.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg % 360);
    }

    private bool CheckOffScreen()
    {
        
        Vector2 targetPos = Camera.main.WorldToScreenPoint(objectivePos);
        bool notInXScreen = targetPos.x <= offset || targetPos.x >= Screen.width - offset;
        bool notInYScreen = targetPos.y <= offset || targetPos.y >= Screen.height - offset;
        return notInXScreen || notInYScreen; 
    }

    private void StartArrowNavi()
    {
        Vector2 pointerWorldPos;
        if (CheckOffScreen())
        {
            Vector2 pointerPos = pointer.position;
            if (objectivePos.x <= offset || objectivePos.y <= 0)
            {
                pointerPos = new Vector2(Mathf.Max(offset, pointerPos.x), Mathf.Max(offset, pointerPos.y));
            }

            if (objectivePos.x >= Screen.width - offset)
            {
                pointerPos.x = Screen.width - offset;
            }

            if (objectivePos.y >= Screen.height - offset)
            {
                pointerPos.y = Screen.height - offset;
            }
            pointerWorldPos = Camera.main.ScreenToWorldPoint(pointerPos);
            pointer.position = pointerWorldPos;

        } else
        {
            pointerWorldPos = Camera.main.ScreenToWorldPoint(objectivePos);
            pointer.position = pointerWorldPos;
            pointer.localEulerAngles = Vector3.zero;

        }
        
                
    }

}
