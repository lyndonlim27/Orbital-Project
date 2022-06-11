using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator),typeof(Collider2D))]
public class DoorBehaviour : EntityBehaviour
{
    private Animator animator;
    private bool unlocked;
    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        Debug.Log(animator);
    }

    private void Start()
    {
        unlocked = false;
    }

    public override void Defeated()
    {
      
    }

    public override EntityData GetData()
    {
        throw new System.NotImplementedException();
    }

    public override void SetEntityStats(EntityData stats)
    {
        throw new System.NotImplementedException();
    }

    public void UnlockDoor()
    {
        if (!unlocked)
        {
            unlocked = true;
            if (animator.runtimeAnimatorController != null)
            {
                animator.SetBool(gameObject.name.Substring(0, 4), true);
            }
            else
            {
                StartCoroutine(FadeOut());

            }
            GetComponent<Collider2D>().enabled = false;
        }
        
    }

    public void LockDoor()
    {
        if (unlocked)
        {
            unlocked = false;
            if (animator.runtimeAnimatorController != null)
            {
                animator.SetBool(gameObject.name.Substring(0, 4), false);
            }
            else
            {
                StartCoroutine(FadeIn());
            }
            GetComponent<Collider2D>().enabled = true;
        }
    }
        


    protected override IEnumerator FadeOut()
    {
        for (float f = 1f; f > 0 ; f -= 0.05f)
        {
            Color c = spriteRenderer.material.color;
            c.a = f;
            spriteRenderer.material.color = c;
            yield return new WaitForSeconds(0.05f);
        }
    }

    protected IEnumerator FadeIn()
    {
        for (float f = 0f; f < 1f; f += 0.05f)
        {
            Color c = spriteRenderer.material.color;
            c.a = f;
            spriteRenderer.material.color = c;
            yield return new WaitForSeconds(0.05f);
        }
    }


}
