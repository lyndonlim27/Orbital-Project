using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Swap(string weapon)
    {
        if (transform.Find(weapon).gameObject.activeInHierarchy == true)
        {
            return;
        }

        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            transform.Find(weapon).gameObject.SetActive(true);
        }

    }

    public GameObject ActiveWeapon()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeInHierarchy)
            {
                return transform.GetChild(i).gameObject;
            }
        }
        return null;
    }
    
}
