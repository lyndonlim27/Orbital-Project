using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActivatorBehaviour : EntityBehaviour
{
    public SwitchData data;
    public LayerMask layerMask;
    public Animator animator;
    public DetectionScript detectionScript;
    /*
 
    public abstract void OnTriggerExit2D();
    public abstract void OnTriggerEnter2D();
    public abstract void OnTriggerStay2D();
    */
}
