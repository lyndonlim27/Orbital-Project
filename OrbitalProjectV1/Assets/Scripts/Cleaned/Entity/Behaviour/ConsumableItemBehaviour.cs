using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableItemBehaviour : EntityBehaviour
{
    [SerializeField] private ConsumableItemData _itemData;
    [SerializeField] AudioSource soundeffect;
    private Animator animator;
    private Transform target;
    private Transform _tf;
    private Vector3 followvelocity = Vector3.zero;
    private Vector3 jumpvelocity;
    private Vector3 angularVelocity;
    private Vector3 startingpos;
    private bool finishedBouncing;
    private WordStorageManagerUI wordStorageManager;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        _tf = GetComponent<Transform>();
        wordStorageManager = FindObjectOfType<WordStorageManagerUI>(true);
    }

    void Start()
    {
       
    }

    private void SetUpDropArea()
    {
        startingpos = _tf.position;
        jumpvelocity = Random.insideUnitSphere;
        jumpvelocity.y = 0;
        jumpvelocity.Normalize();
        jumpvelocity.y = 5;
        angularVelocity = Random.insideUnitSphere.normalized;
    }

    private void BounceEffect()
    {
        
        jumpvelocity.y += Physics.gravity.y * Time.deltaTime;

        transform.position += jumpvelocity * Time.deltaTime;
        transform.Rotate(angularVelocity * Time.deltaTime);
        if (jumpvelocity.y < 0 )
        {
            // build a raycast to test if we hit something.
            float dist = jumpvelocity.magnitude * Time.deltaTime;
            RaycastHit hit;
            Ray ray = new Ray(transform.position, jumpvelocity);
            //Debug.Log("This is raycast: " + Physics.Raycast(ray, out hit, dist, LayerMask.GetMask("Obstacles")));
            if (Physics.Raycast(ray, out hit, dist, LayerMask.GetMask("Obstacles")) || transform.position.y <= startingpos.y)
            {
                // if we hit something then set the transform to the hit point.
                //transform.position = hit.point;
                //// adjust the object to look right on the ground.
                //transform.LookAt(transform.position + transform.forward, hit.normal);
                finishedBouncing = true;
            }
        }
        
    }

    private void OnEnable()
    {
        Debug.Log(_itemData.sprite);
        if (_itemData._consumableType == ConsumableItemData.CONSUMABLE.LETTER)
        {
            animator.enabled = false;
        } else
        {
            animator.enabled = true;
        }
        spriteRenderer.sprite = _itemData.sprite;
        finishedBouncing = false;
        GetComponent<TrailRenderer>().startColor = spriteRenderer.color;
        SetUpDropArea();
    }

    private void Update()
    {
        if (!finishedBouncing)
        {
            BounceEffect();

        } else
        {
            transform.position = Vector3.SmoothDamp(_tf.position, target.position, ref followvelocity, Time.deltaTime * Random.Range(20,30));
        }
    }

    public override EntityData GetData()
    {   
        return _itemData;
    }

    public void SetTarget(GameObject go)
    {
        this.target = go.transform;
    }

    public override void SetEntityStats(EntityData stats)
    {
        this._itemData = (ConsumableItemData) stats;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            switch (_itemData._consumableType)
            {
                default:
                case ConsumableItemData.CONSUMABLE.HEALTH:
                    player.AddHealth(_itemData._health);
                    break;
                case ConsumableItemData.CONSUMABLE.GOLD:
                    player.AddGold(_itemData._gold);
                    break;
            }
            soundeffect.Play();
            StartCoroutine(FadeOut(soundeffect.clip.length));
            
        }
    }
    ////Fades sprite
    IEnumerator FadeOut(float f)
    {
        for (float g = f; g >= -0.05f; g -= 0.05f)
        {
            Color c = spriteRenderer.material.color;
            c.a = g;
            spriteRenderer.material.color = c;
            yield return new WaitForSeconds(0.05f);
        }

        AddLetterToStorage();
        Defeated();
    }

    private void AddLetterToStorage()
    {
        if (_itemData._consumableType == ConsumableItemData.CONSUMABLE.LETTER)
        {
            WordStorageManagerUI.instance.AddItem(_itemData);
        }
    }

    public override void Defeated()
    {
        poolManager.ReleaseObject(this);
    }



}
