using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoomManager : MonoBehaviour
{
    //shared conditions among all room managers. 
    public static List<string> conditions;
    protected List<Entity> entities;
    protected List<NPC> NPCS;
    protected List<GameObject> spawnAreas;
    
    [SerializeField] Entity[] enemyPrefabs;
    [SerializeField] Item itemPrefab;
    [SerializeField] ItemStats[] itemStats;
    [SerializeField] NPC[] npcPrefabs;
    [SerializeField] LayerMask spawnMask;

    //lets fix this part for now
    [Header("Randomizing factors")] //maybe next time can use delanauy triang
    [Range(0, 10)]
    [SerializeField] protected int Enemyspawns = 0; //set a default of 0

    [Range(0, 10)]
    [SerializeField] protected int maxEnemies = 0;

    [Range(0f, 10f)]
    [SerializeField] protected float minRadius = 2f;

    [Range(0f, 10f)]
    [SerializeField] protected float maxRadius = 7f;

    private Collider2D roomArea;
    protected bool activated;
    // basically minbounds and maxbounds are wrt center of the object. 
    private Vector2 areaminBound;
    private Vector2 areamaxBound;
    private float offset = 3f;
    private List<float> RandomRadiuses;
    


    protected virtual void Awake()
    {
        roomArea = GetComponent<Collider2D>();
        spawnAreas = new List<GameObject>();
        activated = false;
        RandomRadiuses = new List<float>();
        conditions = new List<string>();
        this.areaminBound = roomArea.bounds.min;
        this.areamaxBound = roomArea.bounds.max;
        entities = new List<Entity>();
        GenerateRandomSpawns(minRadius,maxRadius);
        Debug.Log(spawnAreas.Count);
    }

    public virtual void FulfillCondition(string key)
    {

        conditions.Remove(key);
    }

    //@param count : number of enemies to spawn
    protected void SpawnEnemies(int count)
    {

        for (int j = 0; j < maxEnemies; j++)
        {


            int randNum = Random.Range(0, spawnAreas.Count);
            CircleCollider2D col = spawnAreas[randNum].GetComponent<CircleCollider2D>();
            Vector3 randomPoint = GetRandomPoint(col.bounds.min, col.bounds.max);
            Entity entity = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], randomPoint, Quaternion.identity);
            entities.Add(entity);
        }
        
    }

    //get randompoint within the radius
    private Vector2 GetRandomPoint(Vector2 minBound, Vector2 maxBound)
    {
        Vector2 randomPoint;
        do
        {
            //AABB axis - 4 possible bounds, top left, top right, bl, br
            //tl = min.x, max.y;
            //bl = min.x, min.y;
            //tr = max.x, max.y;
            //br = max.x, min.y;
            //anywhere within these 4 bounds are possible pts;

            randomPoint = new Vector2(
            Random.Range(minBound.x, maxBound.x),
            Random.Range(minBound.y, maxBound.y));

        } while (!roomArea.OverlapPoint(randomPoint));
        return randomPoint;
    }


    private void GenerateRandomSpawns(float minRadius, float maxRadius)
    {
        Vector2 randPoint;
        float randRadius;
        do
        {
            randPoint = GetRandomPoint(areaminBound, areamaxBound);
            randRadius = Random.Range(minRadius, maxRadius);
            float offset = 3f; //to provide a greater spread between areas;
            if (Physics2D.OverlapCircle(randPoint, randRadius + offset, spawnMask) == null)
            {
                var col = CreateCollider(randPoint, randRadius);
                
                spawnAreas.Add(col);
                Enemyspawns--;
            }
            else
            {
                Debug.Log("No position found" + randPoint + randRadius);
            }

        } while (Enemyspawns > 0);
        
    }


    protected void SpawnObjects()
    {
        Debug.Log(roomArea);
        
        for(int i = 0; i < itemStats.Length; i++)
        {
            ItemStats _item = itemStats[i];
            Vector2 randomPoint = GetRandomPoint(areaminBound,areamaxBound);
            Item initprop = GameObject.Instantiate(itemPrefab,
                                                          randomPoint,
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

    private GameObject CreateCollider(Vector2 randomPoint, float rad)
    {
        GameObject spawnArea = new GameObject("Area" + Enemyspawns);
        CircleCollider2D col = spawnArea.AddComponent<CircleCollider2D>() as CircleCollider2D;  
        col.radius = rad;
        col.transform.position = randomPoint;
        spawnArea.layer = LayerMask.NameToLayer("spawnArea");
        return spawnArea;
    }

}
