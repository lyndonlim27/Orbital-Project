using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedComponent : AttackComponent
{
    public Spell spell;
    public DetectionScript detectionScript;
    

    public override void Init()
    {
        base.Init();
        detectionScript = GetComponent<DetectionScript>();

    }

    public override void Attack(Player target)
    {
        if (spell != null) // if weap == null, means spell animation stored in parent animator.
        {
            GetComponentInParent<Animator>().SetTrigger("Cast");
            GameObject.Instantiate(spell, target.transform.position, Quaternion.identity);
        }

    }

    public bool inRange()
    {
        return this.detectionScript.playerDetected != null;
    }

    
}
