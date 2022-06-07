using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpSettings : MenuBehaviour
{

    private Player _player;
    private SkillManager _skillManager;

    private void Awake()
    {
        _player = FindObjectOfType<Player>(true);
        _skillManager = FindObjectOfType<SkillManager>(true);
    }


    public override void Active()
    {
        this.gameObject.SetActive(true);
        this.transform.Find("Background").gameObject.SetActive(true);
        this.transform.GetComponentInChildren<MainSettings>(true).Active();
        this.transform.GetComponentInChildren<WarningMenu>(true).Inactive();
        this.transform.GetComponentInChildren<ControlMenu>(true).Inactive();
        this.transform.GetComponentInChildren<Shop>(true).Inactive();
        _player.GetCurrentRoom().PauseGame();
        _skillManager.enabled = false;
    }

    public override void Inactive()
    {
        this.gameObject.SetActive(false);
        _player.GetCurrentRoom().ResumeGame();
        _skillManager.enabled = true;
    }
}
