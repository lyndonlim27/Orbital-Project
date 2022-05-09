using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBox : MonoBehaviour
{
    public List<GameObject> WeaponList;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private GameObject GenerateWeapon()
    {
        return GameObject.Instantiate(WeaponList[Random.Range(0, WeaponList.Count)]);
    }
}
