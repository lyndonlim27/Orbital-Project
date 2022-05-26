using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActivatorBehaviour : EntityBehaviour
{
    public abstract void OnTriggerExit2D();
    public abstract void OnTriggerEnter2D();

}
