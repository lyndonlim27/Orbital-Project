using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class NPC_Stats : ScriptableObject
{
    public string NPC_name;
    public Sprite sprite;
    public Animator _animator;
    //wants to add vector2int pos, but probably not a good idea. 
}
