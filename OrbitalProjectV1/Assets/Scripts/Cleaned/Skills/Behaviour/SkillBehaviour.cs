using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class SkillBehaviour : MonoBehaviour
{
    [SerializeField] protected SkillData skillData;
    protected Player _player;
    protected float currCooldown;
    public abstract void ActivateSkill();
    public abstract void ChangeSkill(string skillName);

    public virtual void Start()
    {
        currCooldown = 0;
        _player = FindObjectOfType<Player>();
        GetComponent<Image>().overrideSprite = skillData.sprite;
    }

    public virtual void Update()
    {
        if(currCooldown > 0)
        {
            Tick();
        }

        if(currCooldown < 0)
        {
            currCooldown = 0;
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ChangeSkill("SlowData");
            Debug.Log("CHANGED");
        }
    }

    public bool CheckEnoughMana()
    {
        return  _player.GetMana() >= skillData.manaCost;
    }

    public void ResetCooldown()
    {
        currCooldown = skillData.cooldown;
    }

    private void Tick()
    {
        currCooldown -= Time.deltaTime;
    }
}
