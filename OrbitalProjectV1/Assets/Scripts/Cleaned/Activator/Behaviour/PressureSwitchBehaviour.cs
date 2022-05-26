using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureSwitchBehaviour : ActivatorBehaviour
{
    private Animator _animator;
    //private bool _status;
    [SerializeField] private SwitchData data;


    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    public override EntityData GetData()
    {
        return data;
    }

    public bool IsOn()
    {
        return _animator.GetBool("Collision");
    }

    public override void OnTriggerEnter2D()
    {
        OnSwitch();
    }

    public override void OnTriggerExit2D()
    {
        StartCoroutine(offDelay());
    }

    public override void SetEntityStats(EntityData stats)
    {
        this.data = (SwitchData) stats;
    }

 

    private IEnumerator offDelay()
    {
        yield return new WaitForSeconds(data.duration);
        OffSwitch();
    }

    private void OnSwitch()
    {
        _animator.SetBool("Collision", true);
    }

    private void OffSwitch()
    {
        _animator.SetBool("Collision", false);
    }

    public override void Defeated()
    {
    }

}
