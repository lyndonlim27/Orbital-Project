using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IEnemy
{
    // Start is called before the first frame update
    private IWeapon weapon;
    private Vector3 aggroRadius;
    private enum State {
        Idle,
        Chase,
        Stop,
        Attack,
        Death,
    }
    private State state;
    private int health;
    private float speed;
    private Rigidbody2D rb;
    private Collider2D collider2d;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool armed;
    private GameObject target;

    void IEnemy.Attack(GameObject target)
    {
        throw new System.NotImplementedException();
    }

}
