using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using TMPro;
using Random = UnityEngine.Random;
using System.Reflection;

/// <summary>
/// This is a general class for item with texts.
/// It handles the different behaviours for all items with texts.
/// </summary>

public class ItemWithTextBehaviour : EntityBehaviour, Freezable
{
    [SerializeField] protected ItemWithTextData data;
    [SerializeField] protected List<ConsumableItemData> consumableItemDatas;

   
    private FieldInfo _LightCookieSprite = typeof(Light2D).GetField("m_LightCookieSprite", BindingFlags.NonPublic | BindingFlags.Instance);
    protected Player player;
    protected Light2D light2D;
    protected Rigidbody2D _rb;
    protected CapsuleCollider2D _col;
    protected GameObject secondarylightsource;
    protected ItemTextLogic _tl;
    protected UITextDescription uITextDescription;
    protected TorchPuzzle torchPuzzle;
    protected Vector2 originalpos;
    protected float origintensity;
    private DataPersistenceManager dataPersistenceManager;
    public Animator animator { get; private set; }
    public bool lit { get; private set; }
    

    /** The first instance the gameobject is being activated.
     *  Retrieves all relevant data.
     */
    protected override void Awake()
    {
        base.Awake();
        light2D = GetComponent<Light2D>();
        if (light2D != null)
        {
            origintensity = light2D.intensity;
        }
        
        animator = GetComponent<Animator>();
        animator.keepAnimatorControllerStateOnDisable = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<CapsuleCollider2D>();
        _tl = GetComponentInChildren<ItemTextLogic>();
        uITextDescription = FindObjectOfType<UITextDescription>(true);
        torchPuzzle = FindObjectOfType<TorchPuzzle>(true);     
    }

    /**
     * For general items which is used in all entities.
     */
    private void Start()
    {
        dataPersistenceManager = FindObjectOfType<DataPersistenceManager>(true);
    }

    /** OnEnable method.
     *  To intialize more specific entity behaviours for ObjectPooling.
     */
    protected virtual void OnEnable()
    {
        ResetLight();
        Color c = spriteRenderer.material.color;
        c.a = 1;
        spriteRenderer.material.color = c;
        isDead = false;
        DisableAnimator();
        SetItemBody();
        EnableAnimator();
        spriteRenderer.sortingOrder = 2;
    }

    /**
     * Left it blank for now since most stuffs can be done inside onenable.
     */
    protected void OnDisable()
    {

    }

    /**
     * Checking of URP objects.
     */
    public void CheckURP()
    {
        if (data.NotURP)
        {
            light2D.enabled = false;
        }
    }

    
    /**
     * Enabling of Animator.
     */
    protected virtual void EnableAnimator()
    {
        animator.enabled = true;
        animator.runtimeAnimatorController = Resources.Load(string.Format("Animations/AnimatorControllers/{0}", data.ac_name)) as RuntimeAnimatorController;

    }


    /**
     * Disabling of Animator.
     */
    protected void DisableAnimator()
    {
        animator.enabled = false;
        animator.runtimeAnimatorController = null;
    }

    /**
     * Setting ItemData.
     */
    private void SetItemBody()
    {
        switch (data.item_type)
        {
            default:
            case ItemWithTextData.ITEM_TYPE.CHEST:
                light2D.enabled = true;
                light2D.pointLightOuterRadius = 2f;
                _rb.bodyType = RigidbodyType2D.Kinematic;
                break;
            case ItemWithTextData.ITEM_TYPE.WEAPON:
                _rb.bodyType = RigidbodyType2D.Kinematic;
                light2D.enabled = true;
                break;
            case ItemWithTextData.ITEM_TYPE.PUSHABLE:
                _rb.bodyType = RigidbodyType2D.Dynamic;
                _rb.gravityScale = 0;
                _rb.drag = 1.5f;
                _rb.mass = 45f;
                originalpos = transform.position;
                _rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
                _rb.freezeRotation = true;
                light2D.enabled = true;
                break;
            case ItemWithTextData.ITEM_TYPE.TOMB:
                secondarylightsource = new GameObject();
                SpriteRenderer _spr = secondarylightsource.AddComponent<SpriteRenderer>();
                SettingUpSecondaryLight(secondarylightsource, _spr);
                _rb.bodyType = RigidbodyType2D.Kinematic;
                light2D.enabled = false;
                break;
            case ItemWithTextData.ITEM_TYPE.SAVEPOINT:
                _col.enabled = false;
                light2D.enabled = true;
                light2D.pointLightOuterRadius = 6f;
                light2D.intensity = 0.5f;
                light2D.color = data.defaultcolor;
                break;
            case ItemWithTextData.ITEM_TYPE.PUZZLETORCH:
                torchPuzzle.AddPuzzleTorch(this);
                light2D.enabled = true;
                break;

        }
        SettingUpColliders();
        CheckURP();
    }

    /**
     * Setting Up Secondary Lights if any.
     */
    private void SettingUpSecondaryLight(GameObject go, SpriteRenderer _spr)
    {
        _spr.sprite = data.secondarysprite;
        _spr.sortingOrder = 3;
        Light2D l2d = go.AddComponent<Light2D>();
        l2d.lightType = Light2D.LightType.Sprite;
        l2d.intensity = 0.58f;
        l2d.shadowIntensity = 0.75f;
        _LightCookieSprite.SetValue(l2d, data.secondarysprite);
        go.transform.SetParent(transform);
        go.transform.localPosition = new Vector2(0, -0.5f);
        go.SetActive(false);
    }

    /**
     * Setting Up Colliders.
     */
    private void SettingUpColliders()
    {
        _col.isTrigger = false;
        
        _col.size = data.sprite.bounds.size;
        _col.offset = new Vector2(0, 0);
 

    }

    /**
     * Despawn behaviour.
     */
    public override void Defeated()
    {
        isDead = true;
        
        StartCoroutine(ConditionsChecking());

    }

    /**
     * Conditions checking before releasing to pool.
     */
    private IEnumerator ConditionsChecking()
    {
        if (data.description != "")
        {
            uITextDescription.StartDescription(data.description);
            yield return new WaitForSeconds(5f);

        }
        FullFillCondition();
        switch (data.item_type)
        {
            default:
                HandleAnimation();
                break;
            case ItemWithTextData.ITEM_TYPE.WEAPON:
                FindObjectOfType<WeaponPickup>().Swap(data._name);
                HandleAnimation();
                break;
            case ItemWithTextData.ITEM_TYPE.CHEST:
                SpawnObjects();
                SpawnDrops();
                HandleAnimation();
                break;
            case ItemWithTextData.ITEM_TYPE.PUSHABLE:
                transform.position = originalpos;
                _tl.ResetWord();
                isDead = false;
                break;
            case ItemWithTextData.ITEM_TYPE.TOMB:
                secondarylightsource.SetActive(true);
                break;
            case ItemWithTextData.ITEM_TYPE.SAVEPOINT:
                player.SetSavePoint(transform.position);
                dataPersistenceManager.SaveGame();
                _tl.ResetWord();
                isDead = false;
                break;
            case ItemWithTextData.ITEM_TYPE.PUZZLETORCH:
                torchPuzzle.Input(this);
                _tl.ResetWord();
                isDead = false;
                break;
         
        }
    }

    /**
     * Spawn itemwithtextbehaviours if any.
     */
    private void SpawnObjects()
    {
        List<ItemWithTextData> edClones = new List<ItemWithTextData>();
        
        Array.ForEach(data.itemTextDatas, (d) =>
        {
            ItemWithTextData e = Instantiate(d); 
            e.random = false;
            e.spawnAtStart = true;
            
            e.pos = transform.position + new Vector3(UnityEngine.Random.Range(-2, 2)*data.scale, UnityEngine.Random.Range(-2, 2) * data.scale, 1);
            edClones.Add(e);
            
        });

        currentRoom.SpawnObjects(edClones.ToArray());
        
    }

    /**
     * Spawn consumableitembehaviour if any.
     */
    protected void SpawnDrops()
    {
        int rand = Random.Range(0, 5);
        
        for (int i = 0; i < rand; i++)
        {
            
            int rand2 = Random.Range(0, consumableItemDatas.Count);
            ConsumableItemBehaviour con = poolManager.GetObject(EntityData.TYPE.CONSUMABLE_ITEM) as ConsumableItemBehaviour;
            con.gameObject.SetActive(false);
            ConsumableItemData condata = consumableItemDatas[rand2];
            WordBank wordBank = FindObjectOfType<WordBank>();
            string passcode = wordBank.passcode;
            ConsumableItemData temp = Instantiate(condata);
            if (condata._consumableType == ConsumableItemData.CONSUMABLE.LETTER && passcode.Length > 0)
            {
                int randomnum = Random.Range(0, passcode.Length);
                temp.letter = passcode[randomnum];
                temp.sprite = condata.letters[(int)temp.letter - 81];
                wordBank.passcode = passcode.Substring(0, randomnum) + passcode.Substring(randomnum, passcode.Length - (randomnum + 1));
                
            }
            con.SetEntityStats(temp);
            con.transform.position = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
            con.SetTarget(player.gameObject);
            con.gameObject.SetActive(true);
        }
    }

    /**
     * Handling of animations.
     */
    private void HandleAnimation()
    {
        if (data.ac_name == "" || data._trigger == "")
        {
            StartCoroutine(FadeOut());
        }
        else
        {
            animator.SetTrigger(data._trigger);
        }
    }

    /**
     * Fulfilling condition in rooms.
     */
    protected void FullFillCondition()
    {
        if (data.condition == 1)
        {

            currentRoom.FulfillCondition(data._name + data.GetInstanceID());
        }
    }

    /**
     * Retrieving item data.
     */
    public override EntityData GetData()
    {
        return data;
    }

    /**
     * Setting item data.
     */
    public override void SetEntityStats(EntityData stats)
    {

        ItemWithTextData temp = (ItemWithTextData)stats;
        
        if (temp != null)
        {
            data = temp;
        }
    }

    /**
     * Freezing item movement.
     */
    public virtual void Freeze()
    {
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;
        animator.speed = 0;
    }

    /**
    * UnFreezing item movement.
    */
    public virtual void UnFreeze()
    {
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        animator.speed = 1;
    }

    /**
    * Activating lights animation if any.
    */
    private IEnumerator ActivationAnimation(Light2D light)
    {
        for (float f = 0; f < 3; f += 0.1f)
        {
            light.pointLightInnerRadius = f;
            yield return null;
        }
    }

    /**
    * Static light, no animation.
    */
    public void LightUp()
    {
        lit = true;
        light2D.intensity = 5f;
        light2D.color = data.defaultcolor;
    }

    /**
    * Resetting light intensity.
    */
    public void ResetLight()
    {
        light2D.intensity = origintensity;
        light2D.color = new Color(1, 1, 1, 1);
        lit = false;
    }

    

    //IEnumerator FadeOut()
    //{
    //    for (float f = 1f; f >= -0.05f; f -= 0.05f)
    //    {
    //        Color c = spriteRenderer.material.color;
    //        c.a = f;
    //        spriteRenderer.material.color = c;
    //        yield return new WaitForSeconds(0.05f);
    //    }
    //    Destroy(this.gameObject);
    //}
}



