using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{

    private Transform _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>().transform;
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