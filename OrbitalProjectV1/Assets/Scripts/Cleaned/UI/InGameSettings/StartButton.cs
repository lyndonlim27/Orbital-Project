using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    private Button _button;
    private DataPersistenceManager _dataManager;
    // Start is called before the first frame update
    void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(StartGame);
        _dataManager = FindObjectOfType<DataPersistenceManager>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StartGame()
    {
        if (_dataManager.LoggedIn)
        {
            FindObjectOfType<LoadScene>().LoadSceneFromData();
        }
        else
        {
            FindObjectOfType<LoginMenu>(true).gameObject.SetActive(true);
        }
    }
}
