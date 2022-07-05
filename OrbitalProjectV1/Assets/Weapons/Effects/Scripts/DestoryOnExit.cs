using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryOnExit : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponent<Collider2D>().enabled = false;
        animator.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        Destroy(animator.gameObject, stateInfo.length);
    }

}
