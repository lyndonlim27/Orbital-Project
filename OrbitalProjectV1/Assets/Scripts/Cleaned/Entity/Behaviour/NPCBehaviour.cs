using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCBehaviour : EntityBehaviour
{
    [SerializeField] private NPCData data;
    protected bool fulfilled;
    Animator animator;
    DialogueDetection dialogueDetection;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        dialogueDetection = GetComponentInChildren<DialogueDetection>();
    }
    void Start()
    {
        


    }

    private void OnEnable()
    {
        this.spriteRenderer.sprite = data.sprite;
        if (data._animator != "")
        {
            animator.runtimeAnimatorController = Resources.Load($"Animations/AnimatorController/{data._animator}") as RuntimeAnimatorController;
        }
        fulfilled = data.condition == 0;
        
 
        
    }

    private void OnDisable()
    {
        animator.runtimeAnimatorController = null;
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal virtual void Proceed()
    {
        if (!fulfilled)
        {
            dialogueDetection.gameObject.SetActive(true);
        } else
        {
            dialogueDetection.gameObject.SetActive(false);
        }
        
    }

    internal virtual void Fulfill()
    {
        fulfilled = true;
        this.gameObject.SetActive(false);
        dialogueDetection.enabled = false;
        animator.enabled = false;
        if (data.condition == 1)
        {
            currentRoom.conditions.Remove(data._name);
        }
        
        currentRoom.SpawnObjects(data.dropData);
        
    }

    public override void Defeated()
    {
        return;
    }

    public override void SetEntityStats(EntityData stats)
    {
        this.data = (NPCData) stats;
    }

    public override EntityData GetData()
    {
        return data;
    }

}
