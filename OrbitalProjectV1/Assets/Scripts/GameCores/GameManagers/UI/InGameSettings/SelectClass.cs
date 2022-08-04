using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameManagement.UIComps
{
    public class SelectClass : MonoBehaviour
    {
        private DataPersistenceManager _dataPersistenceManager;
        private Button[] _buttons;
        // Start is called before the first frame update
        void Start()
        {
            _dataPersistenceManager = FindObjectOfType<DataPersistenceManager>();
            _buttons = GetComponentsInChildren<Button>();
            _buttons[0].onClick.AddListener(_dataPersistenceManager.SetAssassin);
            _buttons[0].onClick.AddListener(Inactive);
            _buttons[1].onClick.AddListener(_dataPersistenceManager.SetMage);
            _buttons[1].onClick.AddListener(Inactive);
        }

        // Update is called once per frame
        void Update()
        {

        }


        private void Inactive()
        {
            this.gameObject.SetActive(false);
        }
    }
}
