using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LoginMenu : MonoBehaviour
{
    private Button[] buttons;
    private DataPersistenceManager _dataManager;
    private EventSystem _eventSystem;


    // Start is called before the first frame update
    void Start()
    {
        _dataManager = FindObjectOfType<DataPersistenceManager>();
        buttons = GetComponentsInChildren<Button>();
        buttons[0].onClick.AddListener(_dataManager.Login);
        buttons[1].onClick.AddListener(_dataManager.Register);
        _eventSystem = EventSystem.current;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && _eventSystem.currentSelectedGameObject != null)
        {
            Selectable next = _eventSystem.currentSelectedGameObject.GetComponent<Selectable>();

            if(next.FindSelectableOnDown() != null)
            {
                next.FindSelectableOnDown().Select();
            }
  
        }
    }
}
