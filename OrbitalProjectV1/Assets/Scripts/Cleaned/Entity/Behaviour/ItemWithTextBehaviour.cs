using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;
using System.Reflection;

public class ItemWithTextBehaviour : EntityBehaviour
{
   [SerializeField] protected ItemWithTextData data;
   [SerializeField] protected List<ConsumableItemData> consumableItemDatas;
    //private FieldInfo _LightCookieSprite = typeof(Light2D).GetField("m_LightCookieSprite", BindingFlags.NonPublic | BindingFlags.Instance);
    Player player;
    Light2D light2D;
    private Animator _animator;
    

    protected override void Awake()
    {
        base.Awake();
        light2D = GetComponent<Light2D>();
        _animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void OnEnable()
    {
        //_LightCookieSprite.SetValue(light2D, data.sprite);
        Color c = spriteRenderer.material.color;
        c.a = 1;
        spriteRenderer.material.color = c;
        isDead = false;

    }

    protected void OnDisable()
    {
        

    }

    void Start()
    {
        

    }

    public override void Defeated()
    {
        isDead = true;
        if (data.isAWeapon)
        {
            FindObjectOfType<WeaponPickup>().Swap(data._name);
        } else
        {
            SpawnObjects();
            SpawnDrops();
        }
        
        
        
        
        FullFillCondition();
        HandleAnimation();

    }

    private void SpawnObjects()
    {
        List<ItemWithTextData> edClones = new List<ItemWithTextData>();
        Debug.Log(data);
        Array.ForEach(data.itemTextDatas, (d) =>
        {
            ItemWithTextData e = Instantiate(d); 
            e.random = false;
            e.spawnAtStart = true;
            Debug.Log(e);
            e.pos = transform.position + new Vector3(UnityEngine.Random.Range(-2, 2)*data.scale, UnityEngine.Random.Range(-2, 2) * data.scale, 1);
            edClones.Add(e);
            
        });

        currentRoom.SpawnObjects(edClones.ToArray());
        
    }

    protected void SpawnDrops()
    {
        int rand = Random.Range(0, 5);
        Debug.Log(rand);
        for (int i = 0; i < rand; i++)
        {
            Debug.Log("This is drop" + i);
            int rand2 = Random.Range(0, consumableItemDatas.Count);
            ConsumableItemBehaviour con = poolManager.GetObject(EntityData.TYPE.CONSUMABLE_ITEM) as ConsumableItemBehaviour;
            con.gameObject.SetActive(false);
            ConsumableItemData condata = consumableItemDatas[rand2];
            if (condata._consumableType == ConsumableItemData.CONSUMABLE.LETTER)
            {
                ConsumableItemData temp = Instantiate(condata);
                WordBank wordBank = FindObjectOfType<WordBank>();
                string passcode = wordBank.passcode;
                Debug.Log(passcode);
                int randomnum = Random.Range(0, passcode.Length);
                temp.letter = passcode[randomnum];
                temp.sprite = condata.letters[(int)temp.letter - 81];
                wordBank.passcode = passcode.Substring(0, randomnum) + passcode.Substring(randomnum, passcode.Length - (randomnum + 1));
                con.SetEntityStats(temp);
            }
            con.transform.position = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
            con.SetTarget(player.gameObject);
            con.gameObject.SetActive(true);
        }
    }

    private void HandleAnimation()
    {
        if (data.ac_name == "")
        {
            StartCoroutine(FadeOut());
        }
        else
        {
            _animator.SetTrigger(data._trigger);
        }
    }

    protected void FullFillCondition()
    {
        if (data.condition == 1)
        {

            currentRoom.FulfillCondition(data._name);
        }
    }


    public override EntityData GetData()
    {
        return data;
    }

    public override void SetEntityStats(EntityData stats)
    {

        ItemWithTextData temp = (ItemWithTextData)stats;
        Debug.Log("TEMP" + temp);
        if (temp != null)
        {
            data = temp;
        }
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



