using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadProjMainScene : MonoBehaviour
{


    public void LoadScene()
    {
        SceneManager.LoadScene("Assets/Scenes/ProjConMainMenu.unity");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}