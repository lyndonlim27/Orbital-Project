using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EntityCores;
using GameManagement.UIComps;

namespace GameManagement.RoomManagers
{
    public class PCRoom1 : RoomManager
    {
        private TypingTestTL _tl;

        // Start is called before the first frame update
        protected override void Start()
        {
            FindObjectOfType<PopUpSettings>(true).Active();
            FindObjectOfType<ControlMenu>().Active();
            _tl = GameObject.FindObjectOfType<TypingTestTL>(true);
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
            CheckRunningEvents();
            RoomChecker();

        }

        public override void FulfillCondition(string key)
        {
            conditions.Remove(key);
            _tl.transform.parent.parent.gameObject.SetActive(true);

        }

        protected override void RoomChecker()
        {

            if (conditions.Count == 0 && !_tl.gameObject.activeInHierarchy)
            {
                foreach (DoorBehaviour door in doors)
                {
                    door.GetComponent<Animator>().SetBool("Open", true);
                    door.GetComponent<Collider2D>().enabled = false;
                }
            }
        }

        IEnumerator CountDown()
        {
            yield return new WaitForSecondsRealtime(1f);
        }

    }
}
