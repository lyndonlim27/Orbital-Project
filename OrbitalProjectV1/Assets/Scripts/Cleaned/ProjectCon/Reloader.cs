using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reloader : MonoBehaviour
{
    private Reloader instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetScene()
    {
        StartCoroutine(SceneChecker());
    }

    IEnumerator SceneChecker()
    {
        AsyncOperation sm = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        while (!sm.isDone)
        {
            yield return null;
        }
        FindObjectOfType<Player>().transform.position = new Vector2(-22.56f, 42.68f);
        FindObjectOfType<WeaponPickup>().Swap("Cannon");
        FindObjectOfType<PopUpSettings>().gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
