using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint2 : MonoBehaviour { 

    private CheckPointManager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("CheckPointManager").GetComponent<CheckPointManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            manager.setCheckPoint(this.transform.position);
        }
    }
}
