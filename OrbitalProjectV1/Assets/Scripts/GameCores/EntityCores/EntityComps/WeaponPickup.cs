using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityCores
{
    public class WeaponPickup : MonoBehaviour
    {

        Player player;
        private void Awake()
        {
            player = GetComponentInParent<Player>(true);
        }
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
            //If current weapon is the same as pickup weapon, do nothing
            Transform _tr = transform.Find(weapon);
            if (_tr != null)
            {
                if (_tr.gameObject.activeInHierarchy)
                {
                    return;
                }

                //Else set all weapon inactive then set pickup weapon active
                else
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        transform.GetChild(i).gameObject.SetActive(false);
                    }
                    Weapon weap = transform.Find(weapon).gameObject.GetComponent<Weapon>();
                    weap.gameObject.SetActive(true);
                    player.SetWeapon(weap);
                }

            }



        }

        //Returns the current active weapon
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
}
