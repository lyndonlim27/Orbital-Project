using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPC_Script : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void Fulfill()
    {
        ItemTextDisplayer itd = GetComponentInChildren<ItemTextDisplayer>();
        itd.enabled = true;
        itd.GetComponentInParent<SpriteRenderer>().enabled = true;
        itd.GetComponent<TextMeshPro>().enabled = true;
        GetComponentInChildren<DialogueDetection>().enabled = false;
        //this.gameObject.SetActive(false);
    }


}
