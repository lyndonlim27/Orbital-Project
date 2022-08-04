using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityDataMgt
{
    [CreateAssetMenu]
    public class AttackData : SkillData
    {
        public int numOfProjectiles;
        public int damage;

        public enum ATTACK_TYPE
        {
            FIRE,
            SHURIKEN,
            DASH,
            SHOCKWAVE
        }
        public ATTACK_TYPE attackType;
        public GameObject prefab;

    }
}

