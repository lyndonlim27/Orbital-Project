using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueDetection : MonoBehaviour
{
    public GameObject dialogueAlert;
    DetectionScript detectionScript;
    [SerializeField] private TextAsset inkJson;
    private Boolean canProceed;

    // Start is called before the first frame update

    private void Awake()
    {
        dialogueAlert.SetActive(false);
        canProceed = false;
    }

    void Start()
    {
        detectionScript = GetComponent<DetectionScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (detectionScript.playerDetected && !DialogueManager.GetInstance().playing)
        {
            
            Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            Debug.Log(canProceed);
            if (player != null && !player.isDead() && !player.inCombat && canProceed)
            {
                dialogueAlert.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.Log(DialogueManager.GetInstance().playing);
                    DialogueManager.GetInstance().enterDialogue(inkJson);
                }
            } else
            {
                dialogueAlert.SetActive(false);
            }
    
        } else
        {
            dialogueAlert.SetActive(false);
        }
        
        
      
    }

    public void Fulfilled()
    {
        this.canProceed = true;
    }
}
