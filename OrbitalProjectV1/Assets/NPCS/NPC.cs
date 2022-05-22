using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPC : MonoBehaviour
{
    [SerializeField] private NPC_Stats nPC_Stats;
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        this.spriteRenderer.sprite = nPC_Stats.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal virtual void Fulfill()
    {
       
    }


}
