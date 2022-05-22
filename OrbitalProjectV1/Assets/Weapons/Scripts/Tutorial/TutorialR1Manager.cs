using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialR1Manager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RoomChecker();
    }

    private void RoomChecker()
    {
        GameObject.Find("NPC 1").GetComponentInChildren<DialogueDetection>().Fulfilled();

        if(GameObject.Find("Weapon").GetComponent<WeaponPickup>().ActiveWeapon() != null)
        {

        }

    }
}
