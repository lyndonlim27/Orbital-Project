using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : MonoBehaviour
{
    public SpellStats SpellStats;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger("Cast");
    }

}
