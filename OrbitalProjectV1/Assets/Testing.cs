using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    ParticleSystem part;
    // Start is called before the first frame update
    void Start()
    {
        part = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(GetComponent<Renderer>().material.mainTexture.texelSize);
        Debug.Log((GetComponent<Renderer>().material.mainTexture.dimension.ToString()));
    }
}
