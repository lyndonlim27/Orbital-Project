using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ProjConExit : MonoBehaviour
{
    private Player _player;
    private CinemachineVirtualCamera _camera;
    private Ship ship;

    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>();
        _camera = FindObjectOfType<CinemachineVirtualCamera>();
        ship = FindObjectOfType<Ship>(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _player.gameObject.SetActive(false);
            _camera.Follow = ship.transform;
            ship.enabled = true;
            ship.GetComponent<AudioSource>().enabled = true;
        }
    }
}
