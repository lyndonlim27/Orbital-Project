using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float _length, _startPos;
    [SerializeField] private GameObject cam;
    [SerializeField] private float _parallaxEffect;

    // Start is called before the first frame update
    void Start()
    {
        _startPos = transform.position.x;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float temp = cam.transform.position.x * (1 - _parallaxEffect);
        float distance = cam.transform.position.x * _parallaxEffect;
        transform.position = new Vector3(_startPos + distance,
            transform.position.y, transform.position.z);

        if(temp > _startPos + _length)
        {
            _startPos += _length;
        }

        if(temp < _startPos - _length)
        {
            _startPos -= _length;
        }
    }


}
