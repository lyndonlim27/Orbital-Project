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
    {   /*
        transform.GetChild(0).gameObject.transform.position = Vector3.zero;
        transform.GetChild(0).gameObject.transform.localScale = Vector3.one;
        transform.GetChild(0).gameObject.GetComponent<Collider2D>().enabled = false;
        */
    }

    public void Swap(Weapon weapon)
    {
        Destroy(this.gameObject.transform.GetChild(0).gameObject);
        weapon.transform.SetParent(this.transform);
        // Weapon weap = Instantiate(weapon, this.transform.position, this.transform.rotation);
        //  weap.transform.SetParent(this.gameObject.transform);
        // Destroy(weapon);
        weapon.SetPosition(this.transform);
        //Debug.Log("goody");
    }
    
}
