using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class MiniMapImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool _hovering;
    [SerializeField] private Camera _miniMapCamera;


    // Start is called before the first frame update
    void Start()
    {
        _hovering = false;
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
        _miniMapCamera.orthographicSize -= 2 * Input.GetAxis("Mouse ScrollWheel");
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
