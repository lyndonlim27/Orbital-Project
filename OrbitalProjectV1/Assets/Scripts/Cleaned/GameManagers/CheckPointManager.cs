using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    private static CheckPointManager instance;
    private Vector2 lastCheckPoint;


    private void Awake()
    {
        if(instance == null)
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

    public void setCheckPoint(Vector2 cp)
    {
        lastCheckPoint = cp;
    }

    public Vector2 getCheckPoint()
    {
        return lastCheckPoint;
    }
}
