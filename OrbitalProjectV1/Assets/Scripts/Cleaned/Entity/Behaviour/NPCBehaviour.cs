using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// This is a general class for NPCs.
/// It handles the different behaviours for all NPCs.
/// </summary>

public class NPCBehaviour : EntityBehaviour
{
    [SerializeField] private NPCData data;
    //public bool fulfilled;
    protected bool proceedable;
    protected Animator animator;
    protected bool fulfilled;
    protected DialogueDetection dialogueDetection;

    /** The first instance the gameobject is being activated.
     *  Retrieves all relevant data.
     */
    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        dialogueDetection = GetComponentInChildren<DialogueDetection>();
        
    }

    /** OnEnable method.
     *  To intialize more specific entity behaviours for ObjectPooling.
     */
    private void OnEnable()
    {
        this.spriteRenderer.sprite = data.sprite;
        if (data._animator != "")
        {
            animator.runtimeAnimatorController = Resources.Load($"Animations/AnimatorControllers/{data._animator}") as RuntimeAnimatorController;
        }
        proceedable = data.prereq == null;
        fulfilled = false;
        this.gameObject.layer = LayerMask.NameToLayer("NPC");
      
    }

    
    /** OnDisable method.
     *  To reset the runtime animator controller of gameobject.
     */
    private void OnDisable()
    {
        animator.runtimeAnimatorController = null;
    }

    /**
     * Check for proceedable of NPC every frame.
     */
    void Update()
    {
        dialogueDetection.enabled = proceedable;
    }

    /**
     * Proceed NPC.
     */
    internal virtual void Proceed()
    {
        if (!proceedable && !fulfilled)
        {
            proceedable = true;
            
        }
        
    }

    /**
     * Fulfill NPC condition.
     */
    internal virtual void Fulfill()
    {
        //this.gameObject.SetActive(false);
        fulfilled = true;
        proceedable = false;
        animator.enabled = false;
        if (data.condition == 1)
        {
            currentRoom.conditions.Remove(data._name + data.GetInstanceID());
        }
        if (data.dropData.Length > 0)
        {
            currentRoom.SpawnObjects(data.dropData);
        }
       
 
    }

    /**
     * Despawn behaviour.
     */
    public override void Defeated()
    {
        return;
    }

    /**
     * Setting NPC stats.
     */
    public override void SetEntityStats(EntityData stats)
    {
        this.data = (NPCData) stats;
    }

    /**
     * Getting NPC stats.
     */
    public override EntityData GetData()
    {
        return data;
    }

    /**
     * NPC AfterAction Behaviour.
     */
    public void NPCAfterAction()
    {
        if (fulfilled)
        {
            switch (data._npcAction)
            {
                case NPCData.NPCActions.TYPINGTEST:
                    TypingTestTL _tl = FindObjectOfType<TypingTestTL>(true);
                    _tl.SetActive();
                    break;
            }


            this.enabled = false;
        }
        
        

    }

}
