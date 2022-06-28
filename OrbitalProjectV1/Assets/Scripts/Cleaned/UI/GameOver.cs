using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*GameOver UI Page.
 */
public class GameOver : MonoBehaviour
{
    private DataPersistenceManager _dataManager;
    private LoadScene _loadScene;
    private Button[] _buttons;

    private void Start()
    {
        _dataManager = FindObjectOfType<DataPersistenceManager>();
        _loadScene = FindObjectOfType<LoadScene>();
        _buttons = GetComponentsInChildren<Button>();
        _buttons[0].onClick.AddListener(RestartButton);
        _buttons[1].onClick.AddListener(ExitButton);

    }

    /* Disable all active gameObjects and Setup GameOver Page.
     */
    public void Setup()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.CompareTag("Enemy") || obj.CompareTag("Player"))
            {
                //if (!(obj.CompareTag("MainCamera")  obj.CompareTag("CMCam") || obj.CompareTag("Audio")))
                obj.SetActive(false);
            }
        }
        gameObject.SetActive(true);

    }


    /* Restart Button.
     */
    public void RestartButton()
    {
        _dataManager.LoadGame();
    }

    /* Exit Button.
     */
    public void ExitButton()
    {
        _loadScene.LoadMainMenu();
    }
}