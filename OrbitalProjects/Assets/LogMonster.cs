using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogMonster : MonoBehaviour
{
    public float speed;
    public float checkRadius;
    public float attackRadius;
    public Vector3 dir;
    public bool Rotate;

    public LayerMask Player;

    private Transform target;
    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 movenment;
    
    private bool inChaseRange;
    private bool inAttackRange;
    
    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
    }
}
