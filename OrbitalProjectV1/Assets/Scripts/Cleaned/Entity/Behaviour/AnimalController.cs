using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D),typeof(Rigidbody2D))]
public class AnimalController : EntityBehaviour
{
    
    
    private Animator animator;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private Vector3 randomPosition;

    [SerializeField]
    private float idleCounter;

    private Rigidbody2D _rb;
    private Transform _transform;
    [SerializeField]
    private Vector2 dir;
    private CircleCollider2D _col;

    // Start is called before the first frame update

    protected override void Awake()
    {
        base.Awake();
        idleCounter = 3f;
        animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        animator.keepAnimatorControllerStateOnDisable = false;
        _transform = transform;
        _col = GetComponent<CircleCollider2D>();

    }

    protected override void OnEnable()
    {
        //do nothing, im too lazy to create entity datas.
    }

    void Start()
    {
        currentRoom = GetComponentInParent<RoomManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!inAnimation)
        {
            if (idleCounter >= 0)
            {
                Tick();
            } else
            {
                MovetoEmptyPointOnFarm();
            }     
        } else 
        {
            HandleAnimation();
            CheckReachPoint();
        }
        
    }

    private void Tick()
    {
        idleCounter -= Time.deltaTime;
    }

    private void CheckReachPoint()
    {
        if (Vector2.Distance(_transform.position, randomPosition) <= 0.05f)
        {
            ResetIdleCounter();
        }
    }

    private void ResetIdleCounter()
    {
        idleCounter = 3f;
        inAnimation = false;
        animator.enabled = false;
    }

    private void HandleAnimation()
    {

        animator.SetFloat("x", dir.x);
        animator.SetFloat("y", dir.y);
        
        
    }

    private void FixedUpdate()
    {
        if (inAnimation)
        {
            dir = (randomPosition - (Vector3) _rb.position).normalized;
            _rb.MovePosition(_rb.position + dir * Time.fixedDeltaTime) ;
            
        }
      
    }

    private void MovetoEmptyPointOnFarm()
    {
        inAnimation = true;
        animator.enabled = true;
        randomPosition = LookForEmptyPoint();
        //int rand = UnityEngine.Random.Range(0, 3);
        //if (dir != Vector2.zero)
        //{
        //    dir *= rand == 0 ? new Vector2(-1, -1) : rand == 1 ? new Vector2(-1, 1) : new Vector2(1, -1);
        //randomPosition = currentRoom.GetRandomObjectPoint() * dir;
        //}
        

    }

    private Vector2 LookForEmptyPoint()
    {
        List<Vector2> possibledirs = new List<Vector2>();
        for (int i = -1; i <= 1; i ++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }
                var nextposition = _rb.position + new Vector2(i, j) * (_col.radius + 0.01f);
                bool hit = Physics2D.OverlapPoint(nextposition, LayerMask.GetMask("Obstacles", "HouseExterior", "HouseInterior"));
                if (!hit)
                {
                    possibledirs.Add(nextposition + new Vector2(i,j) * 10f);
                }
                
                //var currposition = _rb.position + _col.radius * new Vector2(i, j);
                //Debug.DrawLine(currposition,nextposition, Color.white, 2.5f);
                //RaycastHit2D hit2D = Physics2D.Linecast(currposition, nextposition, LayerMask.GetMask("Obstacles", "HouseExterior", "HouseInterior"));
                //if (Vector2.Distance(hit2D.point,_rb.position) >= _col.radius)
                //{
                //    possibledirs.Add(nextposition);
                    
                //} 
            }
        }
        //Debug.Log("//start These are all the possible positions, " + possibledirs.Count);
        //foreach(Vector2 position in possibledirs)
        //{
        //    Debug.Log(position);
        //}
        //Debug.Log("//end");
        //if spawned in an area enclosed in all 8 dirs, then just return their own position.
        if (possibledirs.Count == 0)
        {
            return _rb.position;
        }
        return possibledirs[UnityEngine.Random.Range(0, possibledirs.Count)];

    }

    // maybe i wanna use entitydata to save the stuffs but for now no. 
    public override void SetEntityStats(EntityData stats)
    {
        throw new NotImplementedException();
    }

    // maybe next time can cull the animals for meat. -> to drop some meat objects.
    public override void Defeated()
    {
        throw new NotImplementedException();
    }

    public override EntityData GetData()
    {
        throw new NotImplementedException();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        ResetIdleCounter();
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {

        ResetIdleCounter();

    }

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    ResetIdleCounter();
    //}
}
