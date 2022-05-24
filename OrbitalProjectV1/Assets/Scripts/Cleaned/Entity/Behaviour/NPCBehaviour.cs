using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCBehaviour : EntityBehaviour
{
    [SerializeField] private NPCData data;
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        this.spriteRenderer.sprite = data.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal virtual void Fulfill()
    {
       
    }


}
