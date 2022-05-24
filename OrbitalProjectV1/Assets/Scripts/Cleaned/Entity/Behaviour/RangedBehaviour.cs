using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangedBehaviour : MonoBehaviour
{
    public RangedData rangedData;
    protected Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger(rangedData.ac_name);
    }
}
