using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : EliteMeleeFodder
{
    TypingTestTL typingTestTL;

    protected override void Awake()
    {
        base.Awake();
        typingTestTL = FindObjectOfType<TypingTestTL>(true);

    }

    protected override void OnEnable()
    {
        base.OnEnable();
        typingTestTL.transform.root.gameObject.SetActive(true);
        ranged.GetComponent<CircleCollider2D>().radius = enemyData.sprite.bounds.size.x * 100f;
        detectionScript.GetComponent<CircleCollider2D>().radius = enemyData.sprite.bounds.size.x * 100f;
    }


}
