using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryOnExit : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        //animator.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
       // animator.gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0;
        Destroy(animator.gameObject, stateInfo.length);
    }

}
