//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using EntityDataMgt;
//using EntityCores.Enemy;

//namespace EntityCores.Enemy
//{
//    public class BossProps : ItemWithTextBehaviour
//    {
//        //EliteMonsterA EliteMonsterA;
//        //EliteMonsterS EliteMonsterS;

//        public override void Defeated()
//        {
//            EliteMonsterA.allProps.Remove(this);
//            Destroy(this.gameObject);

//        }

//        public override EntityData GetData()
//        {
//            return base.GetData();
//        }

//        public override void SetEntityStats(EntityData stats)
//        {
//            base.SetEntityStats(stats);
//        }

//        public void SetBoss(EliteMonsterA boss)
//        {
//            this.EliteMonsterA = boss;
//        }

//        public void SetBoss(EliteMonsterS boss)
//        {
//            this.EliteMonsterS = boss;
//        }
//    }
//}
