using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitScene : MonoBehaviour
{

    [SerializeField] private string _nextSceneName;
    private LoadScene _loadScene;

    // Start is called before the first frame update
    void Start()
    {
        _loadScene = FindObjectOfType<LoadScene>();
    }

    private void OnTriggerEnter2D()
    {
        _loadScene.NextScene(_nextSceneName);
    }
}
