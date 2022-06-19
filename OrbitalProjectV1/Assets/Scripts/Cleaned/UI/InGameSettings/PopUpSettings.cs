using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpSettings : MenuBehaviour
{

    private Player _player;
    private SkillManager _skillManager;
    private AttackSkillBehaviour _attackSkillBehaviour;
    private DebuffBehaviour _debuffBehaviour;
    private BuffBehaviour _buffBehaviour;

    private void Awake()
    {
        _player = FindObjectOfType<Player>(true);
        _skillManager = FindObjectOfType<SkillManager>(true);
        _debuffBehaviour = FindObjectOfType<DebuffBehaviour>(true);
        _buffBehaviour = FindObjectOfType<BuffBehaviour>(true);
        _attackSkillBehaviour = FindObjectOfType<AttackSkillBehaviour>(true);
    }


    public override void Active()
    {
        this.gameObject.SetActive(true);
        this.transform.Find("Background").gameObject.SetActive(true);
        this.transform.GetComponentInChildren<MainSettings>(true).Active();
        this.transform.GetComponentInChildren<WarningMenu>(true).Inactive();
        this.transform.GetComponentInChildren<ControlMenu>(true).Inactive();
        this.transform.GetComponentInChildren<Shop>(true).Inactive();
        this.transform.GetComponentInChildren<StorageMenu>(true).Inactive();
        _player.GetCurrentRoom().PauseGame();
        _skillManager.enabled = false;
        _debuffBehaviour.enabled = false;
        _buffBehaviour.enabled = false;
        _attackSkillBehaviour.enabled = false;
    }

    public override void Inactive()
    {
        this.gameObject.SetActive(false);
        _player.GetCurrentRoom().ResumeGame();
        _skillManager.enabled = true;
        _debuffBehaviour.enabled = true;
        _buffBehaviour.enabled = true;
        _attackSkillBehaviour.enabled = true;
    }
}
