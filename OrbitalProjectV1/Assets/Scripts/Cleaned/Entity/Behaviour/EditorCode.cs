using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Video;

[ExecuteInEditMode]
public class EditorCode : MonoBehaviour
{

    public TMP_SpriteAsset spriteAsset;
    Dictionary<int, int> glyphs;
    private void OnEnable()
    {
        SkillData[] skillDatas = Resources.FindObjectsOfTypeAll<SkillData>();
        foreach(SkillData skill in skillDatas)
        {
            skill.goldCost -= 10;
        }
    }
}
