using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoomManager : MonoBehaviour
{
    //shared conditions among all room managers. 
    public static List<string> conditions;
    protected List<Entity> entities;
    protected List<NPC> NPCS;
    [SerializeField] protected List<SpawnArea> spawnAreas;
    [SerializeField] Entity[] enemyPrefabs;
    [SerializeField] Item itemPrefab;
    [SerializeField] ItemStats[] itemStats;

    [SerializeField] NPC[] npcPrefabs;
    private Collider2D roomArea;
    protected bool activated;
    protected int maxEnemies;

    protected virtual void Awake()
    {
        roomArea = GetComponent<Collider2D>();
        activated = false;
        conditions = new List<string>();
    }
   
    public virtual void FulfillCondition(string key)
    {

        conditions.Remove(key);
    }

    //@param count : number of enemies to spawn
    protected void SpawnEnemies(int count)
    {
        for (int i = 0; i < spawnAreas.Count; i++)
        {
            Bounds bounds = spawnAreas[i].col.bounds;
            float offsetX = Random.Range(-bounds.extents.x, bounds.extents.x);
            float offsetY = Random.Range(-bounds.extents.y, bounds.extents.y);
            for (int j = 0; j < spawnAreas[j].enemycounts; i++)
            {
                Entity entity = GameObject.Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], new Vector2(offsetX, offsetY), Quaternion.identity);
                entities.Add(entity);
            }
        }
    }

    protected void SpawnObjects()
    {
        Debug.Log(roomArea);
        
        for(int i = 0; i < itemStats.Length; i++)
        {
            ItemStats _item = itemStats[i];
            Bounds bounds = roomArea.bounds;
            float offsetX = Random.Range(bounds.min.x, bounds.max.x);
            float offsetY = Random.Range(bounds.min.y, bounds.max.y);
            Item initprop = GameObject.Instantiate(itemPrefab,
                                                          new Vector2(offsetX, offsetY),
                                                          Quaternion.identity);
            initprop.setItemStats(_item);
            if (_item.conditional == 1)
            {
                conditions.Add(_item._name);
            }


        }
    }

    protected void SpawnNPCS()
    {

    }


    protected bool CanProceed()
    {

        return conditions.Count == 0 && CheckEnemiesDead();
    }


    protected bool CheckEnemiesDead()
    {
        return entities.TrueForAll(entity => entity.isDead);
    }

    protected void CheckDialogue()
    {
        if (GameObject.FindObjectOfType<DialogueManager>().playing)
        {
            //// for now we just use enemy tags, i think next time if got other stuffs then see how
            PauseGame();
        }
        else
        {
            ResumeGame();

        }
    }

    private static void ResumeGame()
    {
        foreach (Entity entity in GameObject.FindObjectsOfType<Entity>())
        {
            entity.enabled = true;
            entity.animator.enabled = true;
        }
    }

    private static void PauseGame()
    {
        foreach (Entity entity in GameObject.FindObjectsOfType<Entity>())
        {
            entity.enabled = false;
            entity.animator.enabled = false;

        }
    }

    protected virtual void InitializeRoom()
    {
        //can add some default initializers here like spawning of objects;
        SpawnObjects();
        SpawnEnemies(maxEnemies); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (activated == false)
            {
                activated = true;
                InitializeRoom();
            }
            
        }
        
    }

}
