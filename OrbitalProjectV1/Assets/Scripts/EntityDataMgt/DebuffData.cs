using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityDataMgt
{
    [CreateAssetMenu]
    public class DebuffData : SkillData
    {
        public float slowAmount;

        public enum DEBUFF_TYPE
        {
            SLOW,
            STUN
        }
        public DEBUFF_TYPE debuffType;
        public ParticleSystem particleSystem;
    }
}
