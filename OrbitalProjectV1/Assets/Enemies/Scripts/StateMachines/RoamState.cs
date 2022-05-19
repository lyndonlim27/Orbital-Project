using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class RoamState : StateClass
{

    public RoamState(Entity entity, StateMachine stateMachine) : base(entity, stateMachine)
    {
    
    }

    public override void Enter(object data)
    {
        entity.animator.SetBool("isWalking", true);
        entity.getNewRoamPosition();

        Roam();
        
    }

    public override void Update()
    {
        Roam();
    }

    public void Roam()
    {
        GameObject go = entity.detectionScript.playerDetected;
        if (go != null)
        {
            
            stateMachine.ChangeState(StateMachine.STATE.CHASE, null);
        }
        else if (entity.isReached())
        {
            stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
        }
        else
        {
            //entity.SetVelocityRoam();
            entity.moveToRoam();
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Exit()
    {
        base.Exit();
    }

}

//class MoveState : StateClass
//{
//    public MoveState(GameObject gameObject, StateMachine stateMachine) : base(gameObject, stateMachine) { }
//    public enum MOVETYPE
//    {
//        TARGET,
//        POSITION
//    }
//    MOVETYPE moveType = MOVETYPE.POSITION;
//    GameObject target;
//    Vector2 roamPos;
//    Animator animator;
//    float speed;
//    GameObject detectionRange;
//    SpriteRenderer spriteRenderer;
//    GameObject weapon;

//    public override void Enter(object data)
//    {
//        moveType = (MOVETYPE)data;
//        MyEnemy script = gameObject.GetComponent<MyEnemy>();
//        animator = gameObject.GetComponent<Animator>();
//        detectionRange = script.detectionRange;
//        spriteRenderer = script.spriteRenderer;
//        speed = script.moveSpd;
//        weapon = script.customWeapon;

//        if (moveType == MOVETYPE.POSITION)
//        {
//            Vector2 roamMinArea = script.roamMinArea;
//            Vector2 roamMaxArea = script.roamMaxArea;

//            roamPos = new Vector2(Random.Range(roamMinArea.x, roamMaxArea.x), Random.Range(roamMinArea.y, roamMaxArea.y));
//        }
//        else
//        {
//            DetectionScript col = detectionRange.GetComponent<DetectionScript>();
//            target = col.playerDetected;
//            roamPos = target.transform.position;
//        }
//        //MoveStateData msData = (MoveStateData)data;

//        animator.SetBool("isWalking", true);
//    }
//    public override void Update()
//    {
//        Transform transform = gameObject.transform;
//        if (moveType == MOVETYPE.TARGET)
//            roamPos = target.transform.position;
//        Vector2 dir = roamPos - new Vector2(transform.position.x, transform.position.y);
//        float dist = dir.magnitude;
//        dir.Normalize();

//        //If player is within attack range
//        WeaponScript weapScript = weapon.GetComponent<WeaponScript>();
//        if (weapScript.playerDetected)
//        {
//            stateMachine.ChangeState(StateMachine.STATE.ATTACK, null);
//            return;
//        }

//        if (dist < speed * Time.deltaTime)
//        {
//            this.gameObject.transform.position = new Vector3(roamPos.x, roamPos.y);
//            stateMachine.ChangeState(StateMachine.STATE.MOVE, MOVETYPE.POSITION);
//            return;
//        }
//        else
//            this.gameObject.transform.position += new Vector3(dir.x, dir.y) * speed * Time.deltaTime;


//        DetectionScript col = detectionRange.GetComponent<DetectionScript>();
//        if (col.playerDetected)
//        {
//            if (moveType == MOVETYPE.POSITION) //if im roaming, then i chase.
//            {
//                stateMachine.ChangeState(StateMachine.STATE.MOVE, MOVETYPE.TARGET);
//                return;
//            }
//        }
//        else
//        {
//            if (moveType == MOVETYPE.TARGET) //if im chasing, then i roam
//            {
//                stateMachine.ChangeState(StateMachine.STATE.MOVE, MOVETYPE.POSITION);
//                return;
//            }
//        }



//        spriteRenderer.flipX = (dir.x > 0);
//    }

//    public override void Exit()
//    {
//    }
//}
