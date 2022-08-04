using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameManagement.UIComps
{
    public class MenuButton : MonoBehaviour
    {

        // Start is called before the first frame update
        void Start()
        {
            GetComponent<Button>().onClick.AddListener(FindObjectOfType<PopUpSettings>(true).Active);

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
