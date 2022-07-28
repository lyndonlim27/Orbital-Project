using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    #region Variables
    [SerializeField] EntityBehaviour[] entityPrefabs;

    public static PoolManager instance { get; private set; }

    //[SerializeField] GameObject prefab;
    //private ObjectPool<EntityBehaviour> entities;
    /**
     * ObjectPools
     */
    private Dictionary<EntityData.TYPE, ObjectPool<EntityBehaviour>> objectPools;

    #endregion

    #region Internal Methods

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
        EntityBehaviour e1 = Instantiate(entity, transform.position, Quaternion.identity);
        e1.gameObject.transform.SetParent(transform);
        e1.gameObject.SetActive(false);
        return e1;

    }

    private void InitializePool()
    {
        foreach (EntityBehaviour entity in entityPrefabs)
        {
            if (entity.GetType() == typeof(RangedBehaviour))
            {
                ObjectPool<EntityBehaviour> pool = new ObjectPool<EntityBehaviour>(() => CreatePooledEntity(entity),
                OnGetEntity, OnReleaseEntity, OnDestroyEntity, false, 200, 1000);
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
            }
            else
            {
                objectPools.Add(entity.GetData()._type, new ObjectPool<EntityBehaviour>(() => CreatePooledEntity(entity),
                OnGetEntity, OnReleaseEntity, OnDestroyEntity, false, 100, 500));
            }

        }
    }

    #endregion

    #region Client-Accessible Methods
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
        return ranged;

        
        
    }

    public void ReleaseObject(EntityBehaviour instance)
    {
        instance.StopCoroutine(instance.FadeOut());
        objectPools[instance.GetData()._type].Release(instance);
    }
    #endregion

    #region Monobehaviour
    private void Awake()
    {
        objectPools = new Dictionary<EntityData.TYPE, ObjectPool<EntityBehaviour>>();
        InitializePool();
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion

}

