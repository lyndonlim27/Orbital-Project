using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class MiniMapImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool _hovering;
    public static MiniMapImage instance { get; private set; }
    [SerializeField] private Camera _miniMapCamera;

    [SerializeField]
    private float offset;

    public List<Transform> minimapIcons;

    private float originalSize;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _hovering = false;
       
        originalSize = _miniMapCamera.orthographicSize;

    }

    // Update is called once per frame
    void Update()
    {
        if(_hovering && IsMouseWheelRolling())
        {
            Zoom();
        }
    }

    private void Zoom()
    {
        offset = 2 * Input.GetAxis("Mouse ScrollWheel");
        _miniMapCamera.orthographicSize -= offset;
        foreach(Transform minimapIcon in minimapIcons)
        {
            var scale = minimapIcon.localScale;
            scale /= Mathf.Abs(((originalSize + 2 * offset) / originalSize));
            if (scale.x < 5)
            {
                minimapIcon.localScale = new Vector2(Mathf.Max(scale.x, 5), Mathf.Max(scale.y, 5));
            }
            else if (scale.x > 20)
            {
                minimapIcon.localScale = new Vector2(Mathf.Min(scale.x, 20), Mathf.Min(scale.y, 20));
            }
            else
            {
                minimapIcon.localScale = scale;
            }
        }     
        
    }

    private bool IsMouseWheelRolling()
    {
        return Input.GetAxis("Mouse ScrollWheel") != 0;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _hovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _hovering = false;
    }
}
