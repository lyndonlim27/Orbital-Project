using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemWithTextBehaviour : EntityBehaviour
{
   [SerializeField] private ItemWithTextData data;
   [SerializeField] protected List<ConsumableItemData> consumableItemDatas;
    Player player;
    private Animator _animator;
    

    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void OnEnable()
    {
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
            ConsumableItemData condata = consumableItemDatas[rand2];
            if (condata._consumableType == ConsumableItemData.CONSUMABLE.LETTER)
            {
                ConsumableItemData temp = condata;
                string passcode = FindObjectOfType<WordBank>().passcode;
                Debug.Log(passcode);
                int randomnum = Random.Range(0, passcode.Length);
                temp.letter = passcode[randomnum];
                passcode = passcode.Substring(0, randomnum) + passcode.Substring(randomnum, passcode.Length - (randomnum + 1));
                temp.sprite = temp.letters[(int)temp.letter - 81];
            }

            con.SetEntityStats(condata);
            con.transform.position = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
            con.SetTarget(player.gameObject);
            con.gameObject.SetActive(true);
        }
    }

    private void HandleAnimation()
    {
        if (_animator.runtimeAnimatorController == null)
        {
            StartCoroutine(FadeOut());
        }
        else
        {
            _animator.SetTrigger(data._trigger);
        }
    }

    private void FullFillCondition()
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



