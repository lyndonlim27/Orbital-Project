using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    [SerializeField] EntityBehaviour[] entityPrefabs;
    //[SerializeField] GameObject prefab;
    //private ObjectPool<EntityBehaviour> entities;
    /**
     * ObjectPools
     */
    private Dictionary<EntityData.TYPE, ObjectPool<EntityBehaviour>> objectPools = new Dictionary<EntityData.TYPE, ObjectPool<EntityBehaviour>>();




    private void Awake()
    {
        InitializePool();
    }
        


    private void OnGetEntity(EntityBehaviour instance)
    {
      
    }

    private void OnReleaseEntity(EntityBehaviour instance)
    {
        instance.transform.SetParent(transform);
        instance.gameObject.SetActive(false);
    }

    private void OnDestroyEntity(EntityBehaviour instance)
    {
        Destroy(instance.gameObject);
    }

    private EntityBehaviour CreatePooledEntity(EntityBehaviour entity)
    {
        Debug.Log("Dafug>");
        EntityBehaviour e1 = Instantiate(entity, transform.position, Quaternion.identity);
        e1.gameObject.transform.SetParent(transform);
        e1.gameObject.SetActive(false);
        return e1;

    }

    public EntityBehaviour GetObject(EntityData.TYPE _type)
    {
        return objectPools[_type].Get();
    }

    public RangedBehaviour GetProjectile(RangedData rangedData, GameObject target, GameObject go)
    {
        
        RangedBehaviour ranged = objectPools[rangedData._type].Get() as RangedBehaviour;
        ranged.TargetEntity(target);
        ranged.SetEntityStats(rangedData);
        ranged.GetComponent<SpriteRenderer>().sprite = rangedData.sprite;
        ranged.transform.SetParent(null);
        //if (rangedData._type == EntityData.TYPE.PROJECTILE)
        //{
        //    ranged.transform.SetParent(null);
        //}
        //else
        //{
        //    ranged.transform.SetParent(go.transform);
        //}


        return ranged;

        
        
    }

    public void ReleaseObject(EntityBehaviour instance)
    {
        Debug.Log(instance);
        instance.StopCoroutine(instance.FadeOut());
        objectPools[instance.GetData()._type].Release(instance);
    }


    private void InitializePool()
    {
        foreach (EntityBehaviour entity in entityPrefabs)
        {
            Debug.Log("?????");
            if (entity.GetType() == typeof(RangedBehaviour))
            {
                Debug.Log("entered");
                Debug.Log(entity);
                ObjectPool<EntityBehaviour> pool = new ObjectPool<EntityBehaviour>(() => CreatePooledEntity(entity),
                OnGetEntity, OnReleaseEntity, OnDestroyEntity, false, 200, 1000);
                Debug.Log(pool.CountAll);
                objectPools.Add(EntityData.TYPE.CAST_ONTARGET, pool);
                objectPools.Add(EntityData.TYPE.CAST_SELF, pool);
                objectPools.Add(EntityData.TYPE.PROJECTILE, pool);
            }
            else if (entity.GetType() == typeof(ItemWithTextBehaviour)) 
            {

                ObjectPool<EntityBehaviour> pool = new ObjectPool<EntityBehaviour>(() => CreatePooledEntity(entity),
                OnGetEntity, OnReleaseEntity, OnDestroyEntity, false, 100, 500);
                objectPools.Add(EntityData.TYPE.ITEM, pool);
                objectPools.Add(EntityData.TYPE.BOSSPROPS, pool);
            } else
            {
                objectPools.Add(entity.GetData()._type, new ObjectPool<EntityBehaviour>(() => CreatePooledEntity(entity),
                OnGetEntity, OnReleaseEntity, OnDestroyEntity, false, 100, 500));
            }

        }

    }

    /**
     * Debugging
     */
    private void OnGUI()
    {
        foreach(EntityData.TYPE type in objectPools.Keys)
        {
            GUI.Label(new Rect(10, 10, 150, 50), $"PoolSize: {type} = {objectPools[type].CountAll}");
        }
        

    }
}
//}
