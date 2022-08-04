using UnityEngine;

namespace EntityDataMgt
{
    [CreateAssetMenu]
    public class RangedData : EntityData
    {
        public int damage;
        public float speed, rotation, lifetime;
        public string trigger;
        public bool followTarget;
        public bool loop;
        public bool stationary;
        public string impact_trigger;
        [TextArea]
        public string weapdescription;
    }
}
