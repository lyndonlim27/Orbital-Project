using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProps : ItemWithTextBehaviour
{
    EliteMonsterA boss;

    public override void Defeated()
    {
        if (boss == null)
        {
            Debug.Log("dafuq");
        }
        Debug.Log(boss.allProps);
        boss.allProps.Remove(this);
        Destroy(this.gameObject);
        
    }

    public override EntityData GetData()
    {
        return base.GetData();
    }

    public override void SetEntityStats(EntityData stats)
    {
        base.SetEntityStats(stats);
    }

    public void SetBoss(EliteMonsterA boss)
    {
        this.boss = boss;
    }
}
