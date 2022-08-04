//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using EntityCores.Enemy;

//namespace EntityCores.StateMachines
//{
//    public class S_DiceState : StateClass
//    {
//        private EliteMonsterS eliteMonsterS;

//        public S_DiceState(EnemyBehaviour entity, StateMachine stateMachine) : base(entity, stateMachine)
//        {
//            eliteMonsterS = (EliteMonsterS)entity;
//        }

//        public override void Enter(object stateData)
//        {
//            base.Enter(stateData);
//            RollDice();
//        }

//        public override void Exit()
//        {
//            base.Exit();
//        }

//        public override void FixedUpdate()
//        {
//            base.FixedUpdate();
//        }

//        public override void Update()
//        {

//            if (enemy.detectionScript.playerDetected && !eliteMonsterS.playing && !eliteMonsterS.animator.GetBool("MageState"))
//            {
//                enemy.MoveToTarget(enemy.player);
//            }

//            RollDice();
//            //stateMachine.ChangeState(StateMachine.STATE.IDLE, null);
//        }

//        public void RollDice()
//        {
//            Debug.Log("Rolled but not entered");
//            //if object detected but is not player, 
//            if (!enemy.detectionScript.playerDetected && !eliteMonsterS.playing)
//            {
//                stateMachine.ChangeState(StateMachine.STATE.ROAMING, null);
//            }

//            else if (Vector2.Distance(enemy.transform.position, enemy.startingpos) >= enemy.maxDist && !eliteMonsterS.playing)
//            {
//                stateMachine.ChangeState(StateMachine.STATE.STOP, null);
//            }
//            else
//            {
//                Debug.Log("Entered");
//                if (eliteMonsterS.detectionScript.playerDetected && !eliteMonsterS.playing)
//                {
//                    int rand = Random.Range(2, 7);
//                    // if odd, we go to knight state, else go to necromancer state;
//                    eliteMonsterS.playing = true;
//                    if (rand % 2 != 0)
//                    {
//                        Debug.Log("Melee State");
//                        if (eliteMonsterS.animator.GetBool("MageState"))
//                        {
//                            eliteMonsterS.StartCoroutine(eliteMonsterS.TransformKnight(rand));
//                        }
//                        else
//                        {
//                            stateMachine.ChangeState(StateMachine.STATE.ATTACK1, rand);
//                        }

//                    }
//                    else
//                    {
//                        Debug.Log("Ranged State");
//                        if (!eliteMonsterS.animator.GetBool("MageState"))
//                        {
//                            eliteMonsterS.StartCoroutine(eliteMonsterS.TransformMage(rand));
//                        }
//                        else
//                        {
//                            eliteMonsterS.StartCoroutine(eliteMonsterS.TeleportMage(rand));
//                        }


//                    }

//                }
//            }
//            enemy.Tick();
//        }
//    }
//}
