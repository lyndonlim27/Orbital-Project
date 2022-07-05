using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Parallax Camera Movement.
 */
public class Parallax : MonoBehaviour
{
    private float _length, _startPos;
    [SerializeField] private GameObject cam;
    [SerializeField] private float _parallaxEffect;

    /**
     * Initialize and fit sprite to window size.
     */
    void Start()
    {
        _startPos = transform.position.x;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    /**
     * Move camera view.
     */
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
