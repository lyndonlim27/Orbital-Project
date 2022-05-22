using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPC_1 : NPC
{

    // Update is called once per frame
    void Update()
    {
        
    }

    internal override void Fulfill()
    {
        ItemTextDisplayer itd = GetComponentInChildren<ItemTextDisplayer>();
        itd.enabled = true;
        itd.GetComponentInParent<SpriteRenderer>().enabled = true;
        itd.GetComponent<TextMeshPro>().enabled = true;
        GetComponentInChildren<DialogueDetection>().enabled = false;
    }
}
