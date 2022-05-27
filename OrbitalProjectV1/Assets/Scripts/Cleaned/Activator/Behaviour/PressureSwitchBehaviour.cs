using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureSwitchBehaviour : ActivatorBehaviour
{
    private Animator _animator;
    //private bool _status;
    [SerializeField] private SwitchData data;
    public LayerMask layerMask;
    private List<Collider2D> colliders;
    private Coroutine _coroutine = null;
    /*
    private void Update()
    {
        Collider2D col = Physics2D.OverlapCircle(transform.position, 1, layerMask);
        Debug.Log(col);
        if (Physics2D.OverlapCircle(transform.position, 1, layerMask) != null)
        {
            OnSwitch();
        }
        else
        {
            StartCoroutine(offDelay());
        }
    }

    void OnDrawGizmos() { Gizmos.DrawWireSphere(transform.position, 1); }*/
    private void Start()
    {
        _animator = GetComponent<Animator>();
        colliders = new List<Collider2D>();
    }
    public override EntityData GetData()
    {
        return data;
    }


    /*
    private void FixedUpdate()
    {
        Debug.Log(colliders.Count);
        if(colliders.Count == 0)
        {
            StopAllCoroutines();
            StartCoroutine(offDelay());
          //offSwitch();
        }
        else
        {
            OnSwitch();
        }
    }*/
    public bool IsOn()
    {
        return _animator.GetBool("Collision");
    }
    
    public void OnTriggerEnter2D(Collider2D collider)
    {
        colliders.Add(collider);
        if (colliders.Count == 1)
        {
            OnSwitch();
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
        }
    }


 
    public void OnTriggerExit2D(Collider2D collider)
    {

        colliders.Remove(collider);
        if (colliders.Count == 0)
        {
            _coroutine = StartCoroutine(offDelay());
        }
        //OffSwitch();
      //  StartCoroutine(offDelay());
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
        currentRoom.FulfillCondition(data._name);
    }

    private void OffSwitch()
    {
        _animator.SetBool("Collision", false);
        currentRoom.UnfulfillCondition(data._name);
    }

    public override void Defeated()
    {
    }

}
