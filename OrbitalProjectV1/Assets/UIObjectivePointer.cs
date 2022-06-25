using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIObjectivePointer : MonoBehaviour
{

    private Vector2 objectivePos;
    private RectTransform pointer;
    private float offset;
    private Vector2 targetPos;
    //private Player player;
    [SerializeField] private Sprite arrow;
    [SerializeField] private Sprite destsprite;
    [SerializeField] private Image pointerImage;


    private void Awake()
    {
        
        pointer = GameObject.Find("Pointer").GetComponent<RectTransform>();
        offset = 100f;
        pointerImage = GetComponentInChildren<Image>();
        pointerImage.sprite = arrow;
        gameObject.SetActive(false);
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        
    }

    private void OnEnable()
    {
        pointerImage.color = new Color(1, 1, 1, 1);
    }

    public void StartNavi(Vector2 pos)
    {
        gameObject.SetActive(true);
        objectivePos = pos;

    }


    private void Update()
    {
        targetPos = Camera.main.WorldToScreenPoint(objectivePos);
        RotateTowardsTarget();
        StartArrowNavi();

    }

    private void RotateTowardsTarget()
    {
        Vector2 dir = (objectivePos - (Vector2)Camera.main.transform.position).normalized;
        pointer.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg % 360);
    }

    public bool CheckOffScreen()
    { 
        bool notInXScreen = targetPos.x <= offset || targetPos.x >= Screen.width - offset;
        bool notInYScreen = targetPos.y <= offset || targetPos.y >= Screen.height - offset;
        return notInXScreen || notInYScreen; 
    }

    private void StartArrowNavi()
    {
        
        if (CheckOffScreen())
        {
            Vector2 pointerPos = pointer.position;
            
            if (targetPos.x <= offset)
            {
                pointerPos.x = offset;
            }

            if (targetPos.y <= offset)
            {
                pointerPos.y = offset;
            }

            if (targetPos.x >= Screen.width - offset)
            {
                pointerPos.x = Screen.width - offset;
            }

            if (targetPos.y >= Screen.height - offset)
            {
                pointerPos.y = Screen.height - offset;
            }
            
            pointer.position = pointerPos;
            pointerImage.sprite = arrow;
            pointer.localPosition = new Vector3(pointer.localPosition.x, pointer.localPosition.y, 0f);

        } else
        {
            pointer.position = targetPos;
            pointerImage.sprite = destsprite;
            pointer.localPosition = new Vector3(pointer.localPosition.x, pointer.localPosition.y, 0f);
            pointer.eulerAngles = Vector3.zero;
            //StartCoroutine(Wait());
             

        }
        
                
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);

    }

    public void StopNavi()
    {
        gameObject.SetActive(false);
    }

    //private CheckIfPlayerCrossed()
    //{
    //    if (player.transform.position == objectivePos)
    //    {
    //        gameObject.
    //    }
    //}

    //private IEnumerator Wait()
    //{
    //    for (float a = 1f; a > 0f; a-=0.1f)
    //    {
    //        Color c = pointerImage.color;
    //        c.a = a;
    //        pointerImage.color = c;
    //        yield return new WaitForSeconds(0.1f);
    //    }
    //    gameObject.SetActive(false);
    //}

}
