using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{

    private Transform _player;
    private Camera _camera;
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>().transform;
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        CheckSize();
    }

    private void CheckSize()
    {
        if (_camera.orthographicSize > 150)
        {
            _camera.orthographicSize = 150;
        }

        if (_camera.orthographicSize < 37)
        {
            _camera.orthographicSize = 37;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 newPos = _player.position;
        // newPos.y = transform.position.y;
        newPos.z = -10;
        transform.position = newPos;
        transform.rotation = Quaternion.Euler(0f, _player.eulerAngles.y, 0);

    }
}