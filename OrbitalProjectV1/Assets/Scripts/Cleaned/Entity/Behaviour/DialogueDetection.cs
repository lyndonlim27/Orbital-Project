using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueDetection : MonoBehaviour
{
    private Player player;
    private NPCBehaviour currentNPC;
    private DialogueManager dialMgr;
    private Boolean canProceed;
    private DetectionScript detectionScript;

    [SerializeField] private GameObject dialogueAlert;


    /**
     * Retrieving Data.
     */
    private void Awake()
    {
        dialMgr = GameObject.FindObjectOfType<DialogueManager>(false);
        dialogueAlert.SetActive(false);
        canProceed = false;
        
    }
    /**
     * Initializing gameObject.
     */
    void Start()
    {
        detectionScript = GetComponent<DetectionScript>();
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        currentNPC = GetComponentInParent<NPCBehaviour>();

    }

    /** Update is called once per frame
     */
    void Update()
    {
        if (CheckForPlayer())
        {
            return;
        } 
        dialogueAlert.SetActive(true);        
        CheckForInteractionButton();
              
    }

    /**
     * CheckForPlayer's validity.
     * return true if player is alive, not in combat and can proceed.
     */
    private bool CheckForPlayer()
    { 
        if (!detectionScript.playerDetected)
        {
            return false;

        } else
        {
            if (player.isDead() || player.inCombat || !canProceed)
            {
                return false;
            }
        }

        return true;
    }

    /**
     * CheckForInteractionButton.
     */
    private void CheckForInteractionButton()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            dialMgr.EnterDialogue(currentNPC);
        }
    }

    /**
     * To check if current prerequisite is fulfilled before allowing interaction between player and NPC.
     */
    public void Fulfilled()
    {
        this.canProceed = true;
    }
}
