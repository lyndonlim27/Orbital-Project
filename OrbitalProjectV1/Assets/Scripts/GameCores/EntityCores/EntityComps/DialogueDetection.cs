using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManagement;

namespace EntityCores.Behavioural
{
    public class DialogueDetection : MonoBehaviour
    {
        private Player player;
        private NPCBehaviour currentNPC;
        private DialogueManager dialMgr;
        private DetectionScript detectionScript;

        [SerializeField] private GameObject dialogueAlert;


        /**
         * Retrieving Data.
         */
        private void Awake()
        {

            dialogueAlert.SetActive(false);

        }
        /**
         * Initializing gameObject.
         */
        void Start()
        {
            detectionScript = GetComponent<DetectionScript>();
            dialMgr = DialogueManager.instance;
            player = Player.instance;
            currentNPC = GetComponentInParent<NPCBehaviour>();

        }

        /** Update is called once per frame
         */
        void Update()
        {/*
        if (CheckForPlayer())
        {
            return;
        } 
        dialogueAlert.SetActive(true);        
        CheckForInteractionButton();*/
            if (detectionScript.playerDetected && !dialMgr.playing)
            {
                if (player != null && !player.IsDead() && !player.InCombat)
                {
                    //                Debug.Log("We are in this detection");
                    dialogueAlert.SetActive(true);
                    if (Input.GetKeyDown(KeyCode.Space) && !dialMgr.playing)
                    {
                        dialMgr.EnterDialogue(currentNPC);
                    }
                }
                else
                {
                    dialogueAlert.SetActive(false);
                }

            }
            else
            {
                dialogueAlert.SetActive(false);
            }
        }

        ///**
        // * CheckForPlayer's validity.
        // * return true if player is alive, not in combat and can proceed.
        // */
        //private bool CheckForPlayer()
        //{ 
        //    if (!detectionScript.playerDetected)
        //    {
        //        return false;

        //    } else
        //    {
        //        if (player.isDead() || player.inCombat)
        //        {
        //            return false;
        //        }
        //    }

        //    return true;
        //}

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

        ///**
        // * To check if current prerequisite is fulfilled before allowing interaction between player and NPC.
        // */
        //public void Fulfilled()
        //{
        //    this.canProceed = true;
        //}
    }
}
