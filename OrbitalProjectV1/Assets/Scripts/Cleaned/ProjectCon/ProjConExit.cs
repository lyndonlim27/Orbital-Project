using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class ProjConExit : MonoBehaviour
{
    private Player _player;
    private WeaponPickup _weaponManager;
    private CinemachineVirtualCamera _camera;
    private Ship ship;

    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>();
        _camera = FindObjectOfType<CinemachineVirtualCamera>();
        _weaponManager = FindObjectOfType<WeaponPickup>();
        
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
            FindObjectOfType<WeaponPickup>().Swap("Fists");
            //_player.gameObject.SetActive(false);
            _player.GetComponent<SpriteRenderer>().enabled = false;
            _weaponManager.ActiveWeapon().GetComponent<SpriteRenderer>().enabled = false;
            _camera.Follow = ship.transform;
            ship.enabled = true;
            ship.GetComponent<AudioSource>().enabled = true;
        }
    }
}
