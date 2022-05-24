using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubMenu : MonoBehaviour
{
    [SerializeField] private MainSettings settings;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            this.gameObject.SetActive(false);
            settings.gameObject.SetActive(true);
        }
    }

    public void MenuActive()
    {
        this.gameObject.SetActive(true);
    }
}
