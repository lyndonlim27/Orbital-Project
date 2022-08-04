using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityDataMgt
{
    [CreateAssetMenu]
    public class NPCData : EntityData
    {
        public TextAsset story;
        public string _animator;
        public EntityData[] dropData;
        public ItemWithTextData prereq;
        public Sprite dialogueFace;
        public enum NPCActions
        {
            DEFAULT,
            TYPINGTEST,
        }
        public NPCActions _npcAction;

    }
}
