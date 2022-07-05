using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
// This is the general class for consumable items.
// It handles the different behaviours for all consumables.
// </summary>

public class ConsumableItemBehaviour : EntityBehaviour
{

    [Header("Properties")]
    [SerializeField] private ConsumableItemData _itemData;
    [SerializeField] AudioSource soundeffect;

    /**
     * Inventory.
     */
    private WordStorageManagerUI wordStorageManager;


    /**
     * Animator and Targets.
     */
    private Animator animator;
    private Transform target;
    private Transform _tf;

    /**
     * Bounce Physics.
     */
    private Vector3 followvelocity = Vector3.zero;
    private Vector3 jumpvelocity;
    private Vector3 angularVelocity;
    private Vector3 startingpos;
    private bool finishedBouncing;

    /** The first instance the gameobject is being activated.
     *  Retrieves all relevant data.
     */
    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        animator.keepAnimatorControllerStateOnDisable = false;
        _tf = GetComponent<Transform>();
        wordStorageManager = FindObjectOfType<WordStorageManagerUI>(true);
    }

    /**
    * For general items which is used in all entities.
    */
    void Start()
    {
       
    }

    /**
     * Setting Up Drop Area.
     */
    private void SetUpDropArea()
    {
        startingpos = _tf.position;
        jumpvelocity = Random.insideUnitSphere;
        jumpvelocity.y = 0;
        jumpvelocity.Normalize();
        jumpvelocity.y = 5;
        angularVelocity = Random.insideUnitSphere.normalized;
    }

    /**
     * Animating Bounce Effect.
     */
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


    /** OnEnable method.
     *  To intialize more specific entity behaviours for ObjectPooling.
     */
    private void OnEnable()
    {
        //RuntimeAnimatorController oganimator = animator.runtimeAnimatorController;
        //animator = null;
        spriteRenderer.sprite = _itemData.sprite;
        animator.Play(_itemData._name);
        finishedBouncing = false;
        GetComponent<TrailRenderer>().startColor = _itemData.defaultcolor;
        SetUpDropArea();
    }

    /**
     * Checking Bounce Physics every frame.
     */
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

    /**
     * Retrieving item data.
     */
    public override EntityData GetData()
    {   
        return _itemData;
    }

    /**
     * Setting Target Transform.
     */
    public void SetTarget(GameObject go)
    {
        this.target = go.transform;
    }

    /**
     * Setting item data.
     */
    public override void SetEntityStats(EntityData stats)
    {
        this._itemData = (ConsumableItemData) stats;
    }

    /**
     * Check if player in range.
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            switch (_itemData._consumableType)
            {
                default:
                case ConsumableItemData.CONSUMABLE.HEALTH:
                    player.AddHealth(Random.Range(0, _itemData._health));
                    break;
                case ConsumableItemData.CONSUMABLE.MANA:
                    player.AddMana(Random.Range(0, _itemData._mana));
                    break;
                case ConsumableItemData.CONSUMABLE.GOLD:
                    player.AddGold(Random.Range(0, _itemData._gold));
                    break;
                case ConsumableItemData.CONSUMABLE.LETTER:
                    WordStorageManagerUI.instance.AddItem(_itemData);
                    break;
            }
            StartCoroutine(WaitForAwhileBeforeRelease());

        }
    }

    /**
     * Wait for audio to finish before releasing to pool.
     */
    private IEnumerator WaitForAwhileBeforeRelease()
    {
        yield return StartCoroutine(LoadSingleAudio(_itemData.interactionAudios[0]));
        Defeated();
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

        Defeated();
    }

    /**
     * Release object to pool.
     */
    public override void Defeated()
    {
        poolManager.ReleaseObject(this);
    }



}
