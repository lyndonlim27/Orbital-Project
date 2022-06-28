using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class
    NPCBehaviour : EntityBehaviour
{
    [SerializeField] private NPCData data;
    //public bool fulfilled;
    protected bool proceedable;
    protected Animator animator;
    protected bool fulfilled;
    protected DialogueDetection dialogueDetection;
    
    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        dialogueDetection = GetComponentInChildren<DialogueDetection>();
        
    }

    private void OnEnable()
    {
        this.spriteRenderer.sprite = data.sprite;
        if (data._animator != "")
        {
            animator.runtimeAnimatorController = Resources.Load($"Animations/AnimatorControllers/{data._animator}") as RuntimeAnimatorController;
        }
        proceedable = data.prereq == null;
        fulfilled = false;
        //dialogueDetection.enabled = proceedable;
        
 
        
    }

    

    private void OnDisable()
    {
        animator.runtimeAnimatorController = null;
    }

    // Update is called once per frame
    void Update()
    {
        dialogueDetection.enabled = proceedable;
    }

    internal virtual void Proceed()
    {
        if (!proceedable)
        {
            proceedable = true;
            
        }
        //} else
        //{
        //    dialogueDetection.enabled = false;
        //}
        
    }

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

    //public IEnumerator InteractedAction()
    //{
    //    yield return StartCoroutine(FindObjectOfType<DialogueManager>().ExitDialogue());
    //    yield return null;
        
        
    //}

}
